using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GlobalLogicManager
    {
        static Dictionary<string, IApplicationGlobalLogic> s_GlobalStatus = new Dictionary<string, IApplicationGlobalLogic>();
        static List<IApplicationGlobalLogic> s_logicList = new List<IApplicationGlobalLogic>();

        public static void Init()
        {
            ApplicationManager.s_OnApplicationUpdate += OnUpdate;
        }

        public static void InitLogic(string logicName)
        {
            if (s_GlobalStatus.ContainsKey(logicName))
            {
                throw new Exception(logicName + " is Inited!");
            }
            else
            {
                IApplicationGlobalLogic l_statusTmp = (IApplicationGlobalLogic)Activator.CreateInstance(Type.GetType(logicName));
                s_GlobalStatus.Add(logicName, l_statusTmp);
                s_logicList = new List<IApplicationGlobalLogic>(s_GlobalStatus.Values);
                try
                {
                    l_statusTmp.Init();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        public static T GetLogic<T>() where T : IApplicationGlobalLogic
        {
            string logicName = typeof(T).Name;
            if (s_GlobalStatus.ContainsKey(logicName))
            {
                IApplicationGlobalLogic logicTmp = s_GlobalStatus[logicName];
                return (T)logicTmp;
            }
            else
            {
                throw new Exception("not find " + logicName);
            }
        }

        static void OnUpdate()
        {
            for (int i = 0; i < s_logicList.Count; i++)
            {
                try
                {
                    s_logicList[i].Update();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
    }
}