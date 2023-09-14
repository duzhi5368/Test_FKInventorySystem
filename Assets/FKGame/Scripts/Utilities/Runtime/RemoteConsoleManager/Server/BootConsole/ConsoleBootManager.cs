using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 控制工具启动方式
    public class ConsoleBootManager
    {
        private static List<IBootFunctionBase> bootFunctions = new List<IBootFunctionBase>();

        public static void Init(RemoteConsoleSettingData config, System.Action OnTriggerBoot)
        {
            Type[] types = ReflectionTool.GetChildTypes(typeof(IBootFunctionBase));
            Debug.Log("types.cout:" + types.Length);
            foreach (var item in types)
            {
                object obj = ReflectionTool.CreateDefultInstance(item);
                if (obj != null)
                {
                    IBootFunctionBase function = (IBootFunctionBase)obj;
                    bootFunctions.Add(function);
                }
            }
            foreach (var item in bootFunctions)
            {
                try
                {
                    item.OnInit(config, OnTriggerBoot);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public static void OnUpdate()
        {
            foreach (var item in bootFunctions)
            {
                try
                {
                    item.OnUpdate();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public static void OnGUI()
        {
            foreach (var item in bootFunctions)
            {
                try
                {
                    item.OnGUI();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}