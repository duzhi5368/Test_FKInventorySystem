using System;
using System.Collections.Generic;
using System.Reflection;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ReflectionTool
    {
        // 是否是代理类型
        public static bool IsDelegate(Type type)
        {
            return type.BaseType == typeof(MulticastDelegate);
        }

        private static Dictionary<Type, Type[]> childTypesCacheContainsChild = new Dictionary<Type, Type[]>();
        private static Dictionary<Type, Type[]> childTypesCacheNoContainsChild = new Dictionary<Type, Type[]>();
        private const BindingFlags flagsInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private const BindingFlags flagsStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private static Type[] allTypeInCurrentDomain = GetAllTypeFromCurrentDomain();

        private static Type[] GetAllTypeFromCurrentDomain()
        {
            List<Type> lstType = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assem in assemblies)
            {
                Type[] types = assem.GetTypes();
                lstType.AddRange(types);
            }
            return lstType.ToArray();
        }

        public static Type[] FastGetChildTypes(Type parentType, bool isContainsAllChild = true)
        {
            Type[] types = null;
            if (isContainsAllChild)
            {
                if (childTypesCacheContainsChild.ContainsKey(parentType))
                {
                    types = childTypesCacheContainsChild[parentType];
                }
                else
                {
                    types = FindFastGetChildTypes(parentType, isContainsAllChild);
                    childTypesCacheContainsChild.Add(parentType, types);
                }
            }
            else
            {
                if (childTypesCacheNoContainsChild.ContainsKey(parentType))
                {
                    types = childTypesCacheNoContainsChild[parentType];
                }
                else
                {
                    types = FindFastGetChildTypes(parentType, isContainsAllChild);
                    childTypesCacheNoContainsChild.Add(parentType, types);
                }
            }
            return types;
        }

        private static Type[] FindFastGetChildTypes(Type parentType, bool isContainsAllChild = true)
        {
            List<Type> lstType = new List<Type>();
            foreach (Type tChild in allTypeInCurrentDomain)
            {
                Type[] baseTypes;
                if (parentType.IsInterface)
                {
                    baseTypes = tChild.GetInterfaces();
                    foreach (var t in baseTypes)
                    {
                        if (t == parentType)
                        {
                            lstType.Add(tChild);
                            if (isContainsAllChild)
                            {
                                Type[] temp = FindFastGetChildTypes(tChild, isContainsAllChild);
                                if (temp.Length > 0)
                                    lstType.AddRange(temp);
                            }
                        }
                    }
                }
                else
                {
                    if (tChild.BaseType == parentType)
                    {
                        lstType.Add(tChild);
                        if (isContainsAllChild)
                        {
                            Type[] temp = FindFastGetChildTypes(tChild, isContainsAllChild);
                            if (temp.Length > 0)
                                lstType.AddRange(temp);
                        }
                    }
                }
            }
            return lstType.ToArray();
        }

        // 获取父类的所有子类
        public static Type[] GetChildTypes(Type parentType, bool isContainsAllChild = true)
        {
            List<Type> lstType = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assem in assemblies)
            {
                Type[] types = assem.GetTypes();
                foreach (Type tChild in types)
                {
                    Type[] baseTypes;
                    if (parentType.IsInterface)
                    {
                        baseTypes = tChild.GetInterfaces();
                    }
                    else
                    {
                        baseTypes = new Type[] { tChild.BaseType };
                    }
                    foreach (var t in baseTypes)
                    {
                        if (t == parentType)
                        {
                            lstType.Add(tChild);
                            if (isContainsAllChild)
                            {
                                Type[] temp = GetChildTypes(tChild, isContainsAllChild);
                                if (temp.Length > 0)
                                    lstType.AddRange(temp);
                            }
                        }
                    }
                }
            }
            return lstType.ToArray();
        }

        // 通过Type类全名来获取Type类
        public static Type GetTypeByTypeFullName(string typeFullName, bool isShowErrorLog = true)
        {
            if (string.IsNullOrEmpty(typeFullName))
                return null;
            Type type = Type.GetType(typeFullName);
            if (type == null)
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                type = ass.GetType(typeFullName);
                if (type == null)
                {
                    Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
                    for (int i = 0; i < assemblys.Length; i++)
                    {
                        Assembly assembly = assemblys[i];
                        type = assemblys[i].GetType(typeFullName);
                        if (type != null)
                            break;
                    }
                }
            }
            if (type == null && isShowErrorLog)
                NetDebug.LogError("无法找到类型：" + typeFullName);
            return type;
        }

        public static Type GetTypefromAssemblyFullName(string AssemblyName, string fullName)
        {
            Assembly tmp = Assembly.Load(AssemblyName);
            return tmp.GetType(fullName);
        }

        // 创建默认实例，参数 isDeepParameters = true：给构造方法传默认值，false：给构造方法传
        private static object CreateDefultInstanceAll(Type type, bool isDeepParameters = false)
        {
            object instance = null;
            string errInfo = "";
            if (type.IsArray)
            {
                //获得维度
                int rank = type.GetArrayRank();
                Type elementType = type.GetElementType();
                int[] par = new int[rank];
                instance = Array.CreateInstance(elementType, par);
            }
            else
            {
                if (type.IsEnum)
                {
                    Array array = Enum.GetValues(type);
                    if (array.Length > 0)
                        instance = array.GetValue(0);
                }
                else
                {
                    instance = CreateBaseTypeDefultInstance(type);
                }
                if (instance == null)

                {
                    try
                    {
                        instance = Activator.CreateInstance(type);
                    }
                    catch (Exception e)
                    {
                        errInfo += e.ToString() + "\n";
                    }
                }
                if (instance == null)
                {
                    ConstructorInfo[] construArrs = type.GetConstructors(flagsInstance);
                    for (int i = 0; i < construArrs.Length; i++)
                    {
                        ConstructorInfo cInfo = construArrs[i];
                        ParameterInfo[] pArr = cInfo.GetParameters();
                        object[] parmsArr = new object[pArr.Length];
                        try
                        {
                            for (int j = 0; j < parmsArr.Length; j++)
                            {
                                ParameterInfo pf = pArr[j];
                                if (isDeepParameters)
                                {
                                    parmsArr[j] = CreateDefultInstance(pf.ParameterType);
                                }
                                else
                                    parmsArr[j] = null;
                            }
                        }
                        catch (Exception e)
                        {
                            errInfo += e.ToString() + "\n";
                            continue;
                        }
                        try
                        {
                            instance = Activator.CreateInstance(type, flagsInstance, null, parmsArr, null);
                            if (instance == null)
                                continue;
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            errInfo += e.ToString() + "\n";
                            continue;
                        }
                    }
                }
            }
            if (instance == null)
                NetDebug.LogError(errInfo);
            return instance;
        }

        public readonly static string StringType = typeof(string).FullName;
        public readonly static string BoolType = typeof(bool).FullName;
        public readonly static string ByteType = typeof(byte).FullName;
        public readonly static string CharType = typeof(char).FullName;
        public readonly static string DecimalType = typeof(decimal).FullName;
        public readonly static string FloatType = typeof(float).FullName;
        public readonly static string IntType = typeof(int).FullName;
        public readonly static string LongType = typeof(long).FullName;
        public readonly static string SbyteType = typeof(sbyte).FullName;
        public readonly static string ShortType = typeof(short).FullName;
        public readonly static string UintType = typeof(uint).FullName;
        public readonly static string UlongType = typeof(ulong).FullName;
        public readonly static string UshortType = typeof(ushort).FullName;
        public static object CreateBaseTypeDefultInstance(Type typeClass)
        {
            if (typeClass == null)
                return null;
            string type = typeClass.FullName;

            if (type.Equals(StringType))
                return "";
            if (type.Equals(BoolType))
                return false;
            if (type.Equals(ByteType))
                return (byte)0;
            if (type.Equals(CharType))
                return '\0';
            if (type.Equals(DecimalType))
                return 0M;
            if (type.Equals(FloatType))
                return 0f;
            if (type.Equals(IntType))
                return 0;
            if (type.Equals(LongType))
                return 0L;
            if (type.Equals(SbyteType))
                return (sbyte)0;
            if (type.Equals(ShortType))
                return (short)0;
            if (type.Equals(UintType))
                return (uint)0;
            if (type.Equals(UlongType))
                return (ulong)0;
            if (type.Equals(UshortType))
                return (ushort)0;
            return null;
        }

        public static object CreateDefultInstance(Type type)
        {
            if (type == null)
            {
                NetDebug.LogError("Type不可为：null");
                return null;
            }
            object instance = CreateDefultInstanceAll(type, true); ;

            if (instance == null)
                NetDebug.LogError("创建默认实例失败！Type:" + type.FullName);
            return instance;
        }
    }
}