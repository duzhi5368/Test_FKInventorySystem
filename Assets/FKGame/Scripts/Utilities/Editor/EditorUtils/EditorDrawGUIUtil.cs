using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Animations;
//------------------------------------------------------------------------
namespace FKGame
{
    public class EditorDrawGUIUtil
    {
        private const string SearchFieldSelect_NameOfFocusedControl = "SearchFieldSelect_TextField";
        public static bool IsDrawPropertyInClass = true;        // ȫ�ֿ���EditorDrawGUIUtil������ʱ�Ƿ��������
        public static bool CanEdit = true;                      // ȫ�ֿ���EditorDrawGUIUtil����ʱ�Ƿ�ɱ༭
        private static bool richTextSupport = false;            // �ؼ��ı��Ƿ�֧�ָ��ı�

        public static bool RichTextSupport
        {
            get { return richTextSupport; }
            set
            {
                if (richTextSupport != value)
                {
                    SetRichText(GUI.skin.GetType(), GUI.skin, value);
                    Type st = typeof(EditorStyles);
                    object v = st.GetField("s_Current", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                    SetRichText(st, v, value);
                    richTextSupport = value;
                }
            }
        }

        private static void SetRichText(Type type, object data, bool isOn)
        {
            FieldInfo[] fs = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var f in fs)
            {
                if (f.FieldType == typeof(GUIStyle))
                {
                    GUIStyle style = (GUIStyle)f.GetValue(null);
                    style.richText = isOn;
                    f.SetValue(null, style);
                }
            }
            PropertyInfo[] ps = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var p in ps)
            {
                if (p.CanRead && p.CanWrite && p.PropertyType == typeof(GUIStyle))
                {
                    GUIStyle style = (GUIStyle)p.GetValue(null, null);
                    style.richText = isOn;
                    p.SetValue(null, style, null);
                }
            }
            if (data != null)
            {
                fs = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var f in fs)
                {
                    if (f.FieldType == typeof(GUIStyle))
                    {
                        GUIStyle style = (GUIStyle)f.GetValue(data);
                        style.richText = isOn;
                        f.SetValue(data, style);
                    }
                }
                ps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var p in ps)
                {
                    if (p.CanRead && p.CanWrite && p.PropertyType == typeof(GUIStyle))
                    {
                        GUIStyle style = (GUIStyle)p.GetValue(data, null);
                        style.richText = isOn;
                        p.SetValue(data, style, null);
                    }
                }
            }
        }

        public static object DrawBaseValue(string content, object data)
        {
            return DrawBaseValue(new GUIContent(content), data);
        }

        // ����object
        public static object DrawBaseValue(GUIContent content, object data)
        {
            if (data == null)
                return data;
            Type type = data.GetType();
            content.text = richTextSupport ? GetFormatName(content.text, type, "green", data) : GetFormatName(content.text, type, "", data);
            object obj = data;
            if (CanEdit)
            {
                if (type == typeof(int))
                {
                    obj = EditorGUILayout.IntField(content, (int)data);
                }
                else if (type == typeof(short))
                {
                    obj = EditorGUILayout.IntField(content, (short)data);
                }
                else if (type == typeof(long))
                {
                    obj = EditorGUILayout.LongField(content, (long)data);
                }
                else if (type == typeof(double))
                {
                    obj = EditorGUILayout.DoubleField(content, (double)data);
                }
                else if (type == typeof(float))
                {
                    obj = EditorGUILayout.FloatField(content, (float)data);
                }
                else if (type == typeof(bool))
                {
                    obj = EditorGUILayout.Toggle(content, (bool)data);
                }
                else if (type == typeof(string))
                {
                    GUIStyle style = "TextArea";
                    style.wordWrap = true;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(content, GUILayout.MaxWidth(Screen.width / 2));
                    obj = EditorGUILayout.TextArea(data.ToString(), style, GUILayout.MaxWidth(Screen.width / 2));
                    GUILayout.EndHorizontal();
                }
                else if (type == typeof(AnimationClip) ||
                    type == typeof(Texture2D) ||
                    type == typeof(Texture) ||
                    type == typeof(Sprite) ||
                    type == typeof(AnimatorController) ||
                    type.BaseType == typeof(UnityEngine.Object) || type.BaseType == typeof(Component) || type.BaseType == typeof(MonoBehaviour))
                {
                    obj = EditorGUILayout.ObjectField(content, (UnityEngine.Object)data, type, true);
                }
                else if (type.BaseType == typeof(Enum))
                {
                    obj = EditorGUILayout.EnumPopup(content, (Enum)Enum.Parse(type, data.ToString()));
                }
                else if (type == typeof(Vector3))
                {
                    obj = EditorGUILayout.Vector3Field(content, (Vector3)data);
                }
                else if (type == typeof(Vector2))
                {
                    obj = EditorGUILayout.Vector2Field(content, (Vector2)data);
                }
                else if (type == typeof(Vector3Int))
                {
                    obj = EditorGUILayout.Vector3IntField(content, (Vector3Int)data);
                }
                else if (type == typeof(Vector2Int))
                {
                    obj = EditorGUILayout.Vector2IntField(content, (Vector2Int)data);
                }
                else if (type == typeof(Vector4))
                {
                    obj = EditorGUILayout.Vector4Field(content, (Vector4)data);
                }
                else if (type == typeof(Color))
                {
                    obj = EditorGUILayout.ColorField(content, (Color)data);
                }
                else if (type.Name == typeof(List<>).Name)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawList("", data, null, null);
                    });
                }
                else if (type.Name == typeof(Dictionary<,>).Name)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawDictionary("", data);
                    });
                }
                else if (type.IsArray)
                {
                    obj = DrawArray(content, data, null, null);
                }
                else if ((type.IsClass && type != typeof(string)) || type.IsValueType)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawClassData("", type.FullName, data);
                    });
                }
            }
            else
            {
                if (type.IsPrimitive
                    || type == typeof(string)
                    || type.IsEnum
                    || type == typeof(Vector2)
                   || type == typeof(Vector3)
                    || type == typeof(Vector2Int)
                   || type == typeof(Vector3Int)
                   || type == typeof(Vector4)
                   || type.FullName == typeof(Color).FullName)
                {
                    string showStr = data.ToString();
                    DrawLableString(content, showStr);
                }
                else if (
                    type == typeof(AnimationClip) ||
                    type == typeof(Texture2D) ||
                    type == typeof(Texture) ||
                    type == typeof(Sprite) ||
                    type == typeof(AnimatorController) ||
                    type.BaseType == typeof(UnityEngine.Object) ||
                    type.BaseType == typeof(Component) ||
                    type.BaseType == typeof(MonoBehaviour))
                {
                    EditorGUILayout.ObjectField(content, (UnityEngine.Object)data, type, true);
                }
                else if (type.Name == typeof(List<>).Name)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawList("", data, null, null);
                    });
                }
                else if (type.Name == typeof(Dictionary<,>).Name)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawDictionary("", data);
                    });
                }
                else if (type.IsArray)
                {
                    DrawArray(content, data, null, null);
                }
                else if (type.IsClass && type != typeof(string) || type.IsValueType)
                {
                    DrawFoldout(data, content, () =>
                    {
                        obj = DrawClassData("", type.FullName, data);
                    });
                }

            }
            return obj;
        }
        private static void DrawLableString(GUIContent name, string data)
        {
            GUILayout.BeginHorizontal("dockarea");
            EditorGUILayout.LabelField(name, GUILayout.MaxWidth(Screen.width / 2));
            EditorGUILayout.LabelField(data, GUILayout.MaxWidth(Screen.width / 2));
            GUILayout.EndHorizontal();
        }

        // �����ֶ�
        public static object DrawBaseValue(object data, FieldInfo field)
        {
            if (data == null || field == null)
                return null;
            object value = field.GetValue(data);
            if (value == null)
            {
                if (CanEdit)
                    value = ReflectionUtils.CreateDefultInstance(field.FieldType);
            }
            string tempName = CheckShowGUINameAttribute(field);
            if (value != null)
            {
                object d = DrawBaseValue(tempName, value);
                field.SetValue(data, d);
            }
            else
            {
                string name = GetFormatName(tempName, field.FieldType);
                DrawLableString(new GUIContent(name), "Null");
            }
            return data;
        }

        // ��������
        public static object DrawBaseValue(object data, PropertyInfo property)
        {
            if (data == null || property == null)
                return null;
            if (property.CanRead)
            {
                object value = property.GetValue(data, null);
                if (value == null)
                {
                    if (CanEdit)
                        value = ReflectionUtils.CreateDefultInstance(property.PropertyType);
                }
                string tempName = CheckShowGUINameAttribute(property);
                if (value != null)
                {
                    object d = DrawBaseValue(tempName, value);
                    if (property.CanWrite)
                    {
                        property.SetValue(data, d, null);
                    }
                }
                else
                {
                    string name = GetFormatName(tempName, property.PropertyType);
                    DrawLableString(new GUIContent(name), "Null");
                }
            }
            return data;
        }

        public static object DrawClassData(string name, string classFullName, object obj)
        {
            return DrawClassData(new GUIContent(name), classFullName, obj);
        }

        // ������
        public static object DrawClassData(GUIContent name, string classFullName, object obj)
        {
            Type t = ReflectionUtils.GetTypeByTypeFullName(classFullName);
            if (obj == null)
                obj = ReflectionUtils.CreateDefultInstance(t);
            else
            {
                if (t.FullName != obj.GetType().FullName)
                {
                    Debug.Log("fullName : " + t.FullName + "   :" + obj.GetType().FullName);
                    obj = ReflectionUtils.CreateDefultInstance(t);
                }
            }
            return DrawClassData(name, obj, null, null);
        }

        public static object DrawClassData(string name, object data, List<string> hideFieldPropertyNames = null, CallBack<MemberInfo> callAffterDrawField = null, CallBack callEndClassDraw = null)
        {
            return DrawClassData(new GUIContent(name), data, hideFieldPropertyNames, callAffterDrawField, callEndClassDraw);
        }

        /// <summary>
        /// �������ṹ��
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hideFieldPropertyNames">����ĳЩ�ֶβ�����</param>
        /// <param name="callAffterDrawField">ÿ����һ���ֶκ�ĵ��ú���</param>
        /// <returns></returns>
        public static object DrawClassData(GUIContent name, object data, List<string> hideFieldPropertyNames = null, CallBack<MemberInfo> callAffterDrawField = null, CallBack callEndClassDraw = null)
        {
            if (data == null)
            {
                DrawLableString(name, "Null");
                return null;
            }
            Type t = data.GetType();
            if (!string.IsNullOrEmpty(name.text))
                GUILayout.Label(name);
            if (SpecialClassDeal(name, data))
                return data;
            FieldInfo[] fs = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] propertys = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (fs.Length > 0)
            {
                List<FieldInfo> tempF = new List<FieldInfo>();
                foreach (var f in fs)
                {
                    if (hideFieldPropertyNames != null)
                    {
                        if (hideFieldPropertyNames.Contains(f.Name))
                            continue;
                    }
                    if (!CheckNoShowInEditorAttribute(f))
                    {
                        continue;
                    }
                    tempF.Add(f);
                }
                fs = tempF.ToArray();
            }
            if (propertys.Length > 0)
            {
                List<PropertyInfo> tempF = new List<PropertyInfo>();
                foreach (var f in propertys)
                {
                    if (hideFieldPropertyNames != null)
                    {
                        if (hideFieldPropertyNames.Contains(f.Name))
                            continue;
                    }
                    if (!CheckNoShowInEditorAttribute(f))
                    {
                        continue;
                    }
                    tempF.Add(f);
                }
                propertys = tempF.ToArray();
            }

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            if (fs.Length > 0)
                GUILayout.Box("�ֶ���Ϣ:");
            foreach (var f in fs)
            {
                DrawBaseValue(data, f);
                if (callAffterDrawField != null)
                    callAffterDrawField(f);
            }
            if (propertys.Length > 0 && IsDrawPropertyInClass)
            {
                GUILayout.Space(2);
                GUILayout.Box("������Ϣ:");
                foreach (var f in propertys)
                {
                    DrawBaseValue(data, f);
                    if (callAffterDrawField != null)
                        callAffterDrawField(f);
                }
            }
            if (callEndClassDraw != null)
            {
                callEndClassDraw();
            }
            GUILayout.EndVertical();
            return data;
        }

        // ���Ƶ����˵�
        public static T DrawPopup<T>(string name, T selectedStr, List<T> displayedOptions, CallBack<T> selectChangeCallBack = null, CallBackR<string, T> customShowItemTextCallBack = null)
        {
            if (displayedOptions == null || (displayedOptions.Count == 0))
                return default(T);
            int selectedIndex = -1;
            if (displayedOptions.Contains(selectedStr))
            {
                selectedIndex = displayedOptions.IndexOf(selectedStr);
            }
            int recode = selectedIndex;
            if (selectedIndex == -1)
                selectedIndex = 0;
            GUIStyle style = new GUIStyle("Popup");
            style.richText = true;
            List<string> tempListStr = new List<string>();
            foreach (var item in displayedOptions)
            {
                if (customShowItemTextCallBack != null)
                {
                    tempListStr.Add(customShowItemTextCallBack(item));
                }
                else
                    tempListStr.Add(item.ToString());
            }
            selectedIndex = EditorGUILayout.Popup(name, selectedIndex, tempListStr.ToArray(), style);

            selectedStr = displayedOptions[selectedIndex];
            if (selectedIndex != recode)
            {
                if (selectChangeCallBack != null)
                    selectChangeCallBack(selectedStr);
            }
            return selectedStr;
        }

        // List�� �����˵�
        public static object DrawStringPopupList(string name, object obj, List<string> displayedOptions)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();
            MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo1 = type.GetMethod("Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                object temp = "";
                methodInfo3.Invoke(obj, new object[] { temp });
            }
            GUILayout.EndHorizontal();
            PropertyInfo pro = type.GetProperty("Count");
            int cout = (int)pro.GetValue(obj, null);
            GUILayout.BeginVertical("box");
            for (int i = 0; i < cout; i++)
            {
                object da = methodInfo.Invoke(obj, new object[] { i });
                GUILayout.BeginHorizontal();
                da = DrawPopup("", da.ToString(), displayedOptions);
                methodInfo1.Invoke(obj, new object[] { i, da });

                if (GUILayout.Button("-", GUILayout.Width(50)))
                {
                    methodInfo2.Invoke(obj, new object[] { i });
                    break;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            return obj;
        }
        public static object DrawList(string name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, CallBackR<object, object> customDrawItem = null, CallBackR<string, object> itemTitleName = null)
        {
            return DrawList(new GUIContent(name), data, addCustomData, ItemChnageCallBack, customDrawItem, itemTitleName);
        }

        /// <summary>
        /// ����List����
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="addCustomData">�Զ�������µ�Item</param>
        /// <param name="ItemChnageCallBack">��Item��ӻ�ɾ��ʱ�ص�</param>
        /// <param name="customDrawItem">�Զ������Item</param>
        /// <param name="itemTitleName">�Զ������Itemm���� ��customDrawItem==nullʱ������</param>
        /// <returns></returns>
        public static object DrawList(GUIContent name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, CallBackR<object, object> customDrawItem = null, CallBackR<string, object> itemTitleName = null)
        {
            if (data == null)
            {
                DrawLableString(name, "Null");
                return null;
            }
            Type type = data.GetType();
            Type t = type.GetGenericArguments()[0];
            int count = (int)type.GetProperty("Count").GetValue(data, null);
            MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo1 = type.GetMethod("set_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo4 = type.GetMethod("Reverse", new Type[] { typeof(int), typeof(int) });
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(name);
                GUILayout.FlexibleSpace();
                if (CanEdit && GUILayout.Button("+", GUILayout.Width(50)))
                {
                    object temp = null;
                    if (addCustomData != null)
                    {
                        object tempVV = addCustomData();
                        if (tempVV == null || tempVV.GetType().FullName != t.FullName)
                            temp = ReflectionUtils.CreateDefultInstance(t);
                        else
                            temp = tempVV;
                    }
                    else
                        temp = ReflectionUtils.CreateDefultInstance(t);
                    methodInfo3.Invoke(data, new object[] { temp });
                    if (ItemChnageCallBack != null)
                        ItemChnageCallBack(true, temp);
                }
                GUILayout.EndHorizontal();
            }
            if (count > 0)
            {
                GUILayout.BeginVertical("box");
                for (int i = 0; i < count; i++)
                {
                    object da = methodInfo.Invoke(data, new object[] { i });
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.BeginVertical();
                            if (customDrawItem != null)
                            {
                                da = customDrawItem(da);
                            }
                            else
                            {
                                string itemTitle = "" + i;
                                if (itemTitleName != null)
                                    itemTitle = itemTitleName(da);

                                da = DrawBaseValue(itemTitle, da);
                            }
                            GUILayout.EndVertical();
                        }
                        methodInfo1.Invoke(data, new object[] { i, da });
                        if (CanEdit && GUILayout.Button("��", GUILayout.Width(15)))
                        {
                            if (i != 0)
                            {
                                methodInfo4.Invoke(data, new object[] { i - 1, 2 });
                            }
                            break;
                        }
                        if (CanEdit && GUILayout.Button("��", GUILayout.Width(15)))
                        {
                            if (i != (count - 1))
                            {
                                methodInfo4.Invoke(data, new object[] { i, 2 });
                            }
                            break;
                        }
                        if (CanEdit && GUILayout.Button("-", GUILayout.Width(30)))
                        {
                            methodInfo2.Invoke(data, new object[] { i });
                            if (ItemChnageCallBack != null)
                                ItemChnageCallBack(false, da);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            return data;
        }

        /// <summary>
        /// ���ƶ�ҳ��ʾList����
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="addCustomData">�Զ�������µ�Item</param>
        /// <param name="ItemChnageCallBack">��Item��ӻ�ɾ��ʱ�ص�</param>
        /// <param name="customDrawItem">�Զ������Item</param>
        /// <returns></returns>
        public static int DrawPage(GUIContent name, int index, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, CallBackR<object, object> customDrawItem = null)
        {
            if (data == null)
            {
                DrawLableString(name, "Null");
                return -1;
            }
            Type type = data.GetType();
            Type t = type.GetGenericArguments()[0];
            int count = (int)type.GetProperty("Count").GetValue(data, null);
            MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo1 = type.GetMethod("set_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo methodInfo4 = type.GetMethod("Reverse", new Type[] { typeof(int), typeof(int) });
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("<", GUILayout.Width(60)))
            {
                index--;
            }
            index = EditorGUILayout.IntField(index, GUILayout.Width(40));
            GUILayout.Label(index + "/" + (count - 1));
            if (GUILayout.Button(">", GUILayout.Width(60)))
            {
                index++;
            }
            GUILayout.FlexibleSpace();
            if (index <= 0)
                index = 0;
            if (index >= count)
                index = count - 1;
            if (CanEdit && GUILayout.Button("+", GUILayout.Width(50)))
            {
                object temp = null;
                if (addCustomData != null)
                {
                    object tempVV = addCustomData();
                    if (tempVV == null || tempVV.GetType().FullName != t.FullName)
                        temp = ReflectionUtils.CreateDefultInstance(t);
                    else
                        temp = tempVV;
                }
                else
                    temp = ReflectionUtils.CreateDefultInstance(t);
                methodInfo3.Invoke(data, new object[] { temp });
                if (ItemChnageCallBack != null)
                    ItemChnageCallBack(true, temp);
            }
            GUILayout.EndHorizontal();

            if (count > 0)
            {
                GUILayout.BeginVertical("box");

                int i = index;
                object da = methodInfo.Invoke(data, new object[] { i });
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                if (customDrawItem != null)
                {
                    da = customDrawItem(da);
                }
                else
                {
                    da = DrawBaseValue("" + i, da);
                }
                GUILayout.EndVertical();
                methodInfo1.Invoke(data, new object[] { i, da });
                if (CanEdit && GUILayout.Button("��", GUILayout.Width(15)))
                {
                    if (i != 0)
                    {
                        methodInfo4.Invoke(data, new object[] { i - 1, 2 });
                    }
                }
                if (CanEdit && GUILayout.Button("��", GUILayout.Width(15)))
                {
                    if (i != (count - 1))
                    {
                        methodInfo4.Invoke(data, new object[] { i, 2 });
                    }
                }
                if (CanEdit && GUILayout.Button("-", GUILayout.Width(30)))
                {
                    methodInfo2.Invoke(data, new object[] { i });
                    if (ItemChnageCallBack != null)
                        ItemChnageCallBack(false, da);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            return index;
        }

        public static object DrawArray(string name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, CallBackR<object, object> customDrawItem = null)
        {
            return DrawArray(new GUIContent(name), data, addCustomData, ItemChnageCallBack, customDrawItem);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="addCustomData">�Զ�������µ�Item</param>
        /// <param name="ItemChnageCallBack">��Item��ӻ�ɾ��ʱ�ص�</param>
        /// <param name="customDrawItem">�Զ������Item</param>
        /// <returns></returns>
        public static object DrawArray(GUIContent name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, CallBackR<object, object> customDrawItem = null)
        {
            if (data == null)
            {
                DrawLableString(name, "Null");
                return null;
            }
            Type type = data.GetType();
            string ts = type.FullName.Replace("[]", "");
            type = ReflectionUtils.GetTypeByTypeFullName(ts);
            var typeList = typeof(List<>);
            Type typeDataList = typeList.MakeGenericType(type);
            object instance = Activator.CreateInstance(typeDataList);
            MethodInfo AddRange = typeDataList.GetMethod("AddRange");
            AddRange.Invoke(instance, new object[] { data });
            instance = DrawList(name, instance, addCustomData, ItemChnageCallBack, customDrawItem);
            MethodInfo ToArray = typeDataList.GetMethod("ToArray");
            return ToArray.Invoke(instance, null);
        }

        public static object DrawDictionary(string name, object data)
        {
            return DrawDictionary(new GUIContent(name), data);
        }

        // �����ֵ�����
        public static object DrawDictionary(GUIContent name, object data)
        {
            if (data == null)
            {
                DrawLableString(name, "Null");
                return null;
            }
            Type type = data.GetType();
            PropertyInfo p = type.GetProperty("Count");
            int count = (int)p.GetValue(data, null);
            MethodInfo clearMe = type.GetMethod("Clear");
            p = type.GetProperty("Keys");
            object keys = p.GetValue(data, null);
            List<object> tempListKeys = new List<object>();
            MethodInfo GetEnumeratorMe = keys.GetType().GetMethod("GetEnumerator");
            PropertyInfo current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
            MethodInfo moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");
            object enumerator = GetEnumeratorMe.Invoke(keys, null);
            for (int i = 0; i < count; i++)
            {
                moveNext.Invoke(enumerator, null);
                object v = current.GetValue(enumerator, null);
                tempListKeys.Add(v);
            }
            p = type.GetProperty("Values");
            object values = p.GetValue(data, null);
            List<object> tempListValues = new List<object>();
            GetEnumeratorMe = values.GetType().GetMethod("GetEnumerator");
            current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
            moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");
            enumerator = GetEnumeratorMe.Invoke(values, null);
            for (int i = 0; i < count; i++)
            {
                moveNext.Invoke(enumerator, null);
                object v = current.GetValue(enumerator, null);
                tempListValues.Add(v);
            }
            MethodInfo addDicMe = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
            if (tempListKeys.Count > 0)
            {
                GUILayout.BeginVertical("box");
                for (int i = 0; i < tempListKeys.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(name);
                    GUILayout.FlexibleSpace();
                    if (CanEdit && GUILayout.Button("-", GUILayout.Width(40)))
                    {
                        tempListKeys.RemoveAt(i);
                        tempListValues.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.BeginVertical("box");
                    tempListKeys[i] = DrawBaseValue(i + ": key", tempListKeys[i]);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("box");
                    tempListValues[i] = DrawBaseValue("value", tempListValues[i]);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }
                GUILayout.EndVertical();
                clearMe.Invoke(data, null);
                for (int i = 0; i < tempListKeys.Count; i++)
                {
                    addDicMe.Invoke(data, new object[] { tempListKeys[i], tempListValues[i] });
                }
            }
            return data;
        }

        // ����������
        public static string DrawSearchField(string value, params GUILayoutOption[] options)
        {
            return DrawSearchField(value, null, options);
        }

        // ����������
        public static string DrawSearchField(string value, CallBack valueChangeCallBack, params GUILayoutOption[] options)
        {
            string res = value;
            MethodInfo info = typeof(EditorGUILayout).GetMethod("ToolbarSearchField", BindingFlags.NonPublic | BindingFlags.Static, null, new System.Type[] { typeof(string), typeof(GUILayoutOption[]) }, null);
            if (info != null)
            {
                res = (string)info.Invoke(null, new object[] { value, options });
                if (res != value)
                {
                    if (valueChangeCallBack != null)
                    {
                        valueChangeCallBack();
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// ����Type ��name �����ʾ������������ �� �� frame(List<int>)   Count:10
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t"></param>
        /// <param name="color">������������ɫ �� ��List<int> </param>
        /// <param name="data">���ڻ�ȡList Array�ȵ�����</param>
        /// <returns></returns>
        public static string GetFormatName(string name, Type t, string color = "", object data = null)
        {
            if (t == null && data == null)
                return name;
            string typeName = t.Name;
            if (t.IsClass || t.IsValueType)
            {
                if (t.IsGenericType)
                {
                    typeName = typeName.Remove(typeName.IndexOf('`'));
                    Type[] gTypes = t.GetGenericArguments();
                    string temp = "";
                    for (int i = 0; i < gTypes.Length; i++)
                    {
                        Type tempType = gTypes[i];
                        temp += tempType.Name;
                        if (i < gTypes.Length - 1)
                        {
                            temp += ",";
                        }
                    }
                    typeName = typeName + "<" + temp + ">";
                }
                typeName = "" + typeName + "";
            }
            else if (t.IsEnum)
            {
                typeName = " Enum : " + typeName + "";
            }
            if (!string.IsNullOrEmpty(color))
            {
                typeName = "(<color=" + color + ">" + typeName + "</color>)";
            }
            else
                typeName = "(" + typeName + ")";

            if (t.IsArray)
            {
                if (data != null)
                {
                    PropertyInfo f = t.GetProperty("Length");
                    int count = (int)f.GetValue(data, null);
                    typeName += "\tLength : " + count;
                }
            }
            if (t == typeof(List<>) || t == typeof(Dictionary<,>))
            {
                if (data != null)
                {
                    int count = (int)t.GetProperty("Count").GetValue(data, null);
                    typeName += "\tCount : " + count;
                }
            }
            return name + typeName;

        }

        // ����������࣬DateTime.IPAddress.....
        private static bool SpecialClassDeal(GUIContent name, object data)
        {
            Type t = data.GetType();
            if (t == typeof(DateTime))
            {
                DateTime dt = (DateTime)data;
                DrawLableString(name, dt.ToString() + ":" + dt.Millisecond);
                return true;
            }
            else if (t == typeof(IPAddress))
            {
                DrawLableString(name, data.ToString());
                return true;
            }
            return false;
        }

        public static void DrawFoldout(object taget, string content, CallBack drawGUI)
        {
            DrawFoldout(taget, new GUIContent(content), drawGUI);
        }

        // ������ҳ
        public static void DrawFoldout(object taget, GUIContent content, CallBack drawGUI)
        {
            bool isFolder = EditorGUILayout.Foldout(EditorGUIState.GetState(taget), content);
            if (isFolder)
            {
                EditorGUI.indentLevel += 1;
                if (drawGUI != null)
                    drawGUI();
                EditorGUI.indentLevel -= 1;
            }
            EditorGUIState.SetState(taget, isFolder);
        }

        /// <summary>
        /// ���ƹ�����ͼ
        /// </summary>
        /// <param name="taget">״̬�����Ŀ��object</param>
        /// <param name="drawGUI"></param>
        /// <param name="style"></param>
        public static void DrawScrollView(object taget, CallBack drawGUI, string style = "")
        {
            Vector2 pos = EditorGUIState.GetVector2(taget);
            if (string.IsNullOrEmpty(style))
            {
                pos = GUILayout.BeginScrollView(pos);
            }
            else
            {
                pos = GUILayout.BeginScrollView(pos, style);
            }
            if (drawGUI != null)
                drawGUI();
            GUILayout.EndScrollView();
            EditorGUIState.SetVector2(taget, pos);
        }

        // ������в���
        public static void DrawHorizontalCenter(CallBack drawGUI, string style = "")
        {
            if (string.IsNullOrEmpty(style))
                GUILayout.BeginHorizontal();
            else
                GUILayout.BeginHorizontal(style);
            GUILayout.FlexibleSpace();
            if (drawGUI != null)
            {
                drawGUI();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        public static string DrawSearchFieldSelect(string name, string selectStr, string[] displayedOptions, CallBack<string> selectCallback)
        {
            return DrawSearchFieldSelect(new GUIContent(name), selectStr, displayedOptions, selectCallback);
        }

        // ��������ѡ��������ı��ɳ��� displayedOptions �������ѡ��ѡ���˰�ť����selectCallback�ص�
        public static string DrawSearchFieldSelect(GUIContent name, string selectStr, string[] displayedOptions, CallBack<string> selectCallback)
        {
            string res = "";
            GUILayout.BeginHorizontal();
            GUILayout.Label(name, GUILayout.MaxWidth(160));
            GUILayout.BeginVertical();
            GUI.SetNextControlName(SearchFieldSelect_NameOfFocusedControl);
            res = EditorGUILayout.TextField(selectStr);
            bool isFocuseTextField = GUI.GetNameOfFocusedControl() == SearchFieldSelect_NameOfFocusedControl;
            Rect r = GUILayoutUtility.GetLastRect();
            List<GUIContent> list = new List<GUIContent>();
            if (!string.IsNullOrEmpty(res) && isFocuseTextField)
            {
                foreach (var item in displayedOptions)
                {
                    if (null == item)
                        continue;
                    if (item.Contains(res))
                        list.Add(new GUIContent(item));
                }
                r.y = r.y + r.height;
                r.height = list.Count > 3 ? list.Count * 21 : 21 * 2;
                r = GUILayoutUtility.GetRect(r.width, r.height);
                EditorGUI.DrawRect(r, Color.grey);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (!isFocuseTextField) 
                return res;
            for (int i = 0; i < list.Count; i++)
            {
                Rect rect = new Rect(r.x, r.y + i * 20 + 1, r.width, 20);
                if (GUI.Button(rect, list[i]))
                {
                    res = list[i].text;
                    selectCallback(res);
                    // ȥ���ı�����Ľ���
                    EditorGUI.FocusTextInControl("");
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// ���Ƹ��Ӳ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">����</param>
        /// <param name="rowCount">һ�и���</param>
        /// <param name="spacing">item��ļ��</param>
        /// <param name="drawItemCallBack">����ÿ��Item�ص�</param>
        public static void DrawGrid<T>(List<T> data, int rowCount, Vector2 spacing, CallBack<T> drawItemCallBack)
        {
            if (data == null)
                return;
            int columnCount = data.Count / rowCount;
            int exraNumber = data.Count % rowCount;
            if (exraNumber > 0)
                columnCount++;
            int index = 0;
            DrawScrollView(data, () =>
            {
                for (int i = 0; i < columnCount; i++)
                {
                    GUILayout.BeginHorizontal();
                    int count = rowCount;
                    if (exraNumber > 0 && i == (columnCount - 1))
                        count = exraNumber;
                    GUILayout.Space(spacing.x);

                    for (int j = 0; j < count; j++)
                    {
                        if (drawItemCallBack != null)
                            drawItemCallBack(data[index]);
                        GUILayout.Space(spacing.x);
                        index++;
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(spacing.y);
                }
            });
        }

        /// <summary>
        /// ����Ƿ���NoShowInEditorAttribute����
        /// </summary>
        /// <param name="member"></param>
        /// <returns>�����Ƿ���ʾ</returns>
        private static bool CheckNoShowInEditorAttribute(MemberInfo member)
        {
            object[] attrs = member.GetCustomAttributes(false);
            bool isShow = true;
            foreach (var att in attrs)
            {
                if (att.GetType() == typeof(NoShowInEditorAttribute))
                {
                    isShow = false;
                    break;
                }
            }
            return isShow;
        }

        /// <summary>
        /// ΪDrawClass�ֶλ����Ըı�Ϊ�ض�������
        /// </summary>
        /// <param name="member"></param>
        /// <returns>��ShowGUINameAttribute����ʹ������������֣�����ʹ��Ĭ���ֶλ�������</returns>
        private static string CheckShowGUINameAttribute(MemberInfo member)
        {
            object[] attrs = member.GetCustomAttributes(false);
            string name = member.Name;
            foreach (var att in attrs)
            {
                Type t = att.GetType();
                if (t == typeof(ShowGUINameAttribute))
                {
                    PropertyInfo p = t.GetProperty("Name");
                    object obj = p.GetValue(att, null);
                    name = (obj == null ? "" : obj.ToString());
                    break;
                }
            }
            return name;
        }
    }
}
