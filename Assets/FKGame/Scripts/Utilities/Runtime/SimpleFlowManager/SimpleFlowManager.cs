using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SimpleFlowManager
    {
        private Dictionary<string, FlowItemBase> allFlowItems = new Dictionary<string, FlowItemBase>();
        private Dictionary<string, object> globalVariables = new Dictionary<string, object>();

        public Action<FlowItemBase> OnStart;            // �ڵ㿪ʼ�ص����ڵ����֣�GetType().Name����
        public Action<FlowItemBase, string> OnFinished; // �ڵ���ɻص����ڵ����֣�GetType().Name����error������Ϣ��

        public FlowItemBase CurrentRunFlowItem;

        public void SetVariable(string key, object value)
        {
            if (globalVariables.ContainsKey(key))
            {
                globalVariables[key] = value;
            }
            else
            {
                globalVariables.Add(key, value);
            }
        }

        public object GetVariable(string key)
        {
            object value = null;
            globalVariables.TryGetValue(key, out value);
            return value;
        }

        public T GetVariable<T>(string key)
        {
            object value = null;
            globalVariables.TryGetValue(key, out value);
            if (value == null)
                return default(T);
            return (T)value;
        }

        public void AddFlowItems(FlowItemBase[] flowItems)
        {
            foreach (var item in flowItems)
            {
                AddFlowItem(item);
            }
        }
        public void AddFlowItem(FlowItemBase flowItem)
        {
            flowItem.flowManager = this;
            if (allFlowItems.ContainsKey(flowItem.Name))
            {
                allFlowItems[flowItem.Name] = flowItem;
            }
            else
            {
                allFlowItems.Add(flowItem.Name, flowItem);
            }
        }

        public void RemoveFlowItem(string name)
        {
            if (allFlowItems.ContainsKey(name))
            {
                allFlowItems.Remove(name);
            }
        }

        public T GetFlowItem<T>() where T : FlowItemBase
        {
            string name = typeof(T).Name;
            if (allFlowItems.ContainsKey(name))
            {
                return (T)allFlowItems[name];
            }
            return null;
        }

        public void RunFlowItem<T>(bool forceRestartIfSameName = false, params object[] paras)
        {
            RunFlowItem(typeof(T), forceRestartIfSameName, paras);
        }

        public void RunFlowItem(Type type, bool forceRestartIfSameName = false, params object[] paras)
        {
            RunFlowItem(type.Name, forceRestartIfSameName, paras);
        }

        public void RunFlowItem(string name, bool forceRestartIfSameName = false, params object[] paras)
        {
            FlowItemBase newItem = null;
            if (allFlowItems.ContainsKey(name))
            {
                newItem = allFlowItems[name];
            }
            if (newItem == null)
            {
                Debug.LogError("No Flow Item��" + name);
                return;
            }
            if (CurrentRunFlowItem != null)
            {
                if (CurrentRunFlowItem.Name == name)
                {
                    if (!forceRestartIfSameName)
                        return;
                    else
                    {
                        CurrentRunFlowItem.Start(paras);
                        return;
                    }
                }
            }
            CurrentRunFlowItem = newItem;
            CurrentRunFlowItem.Start(paras);
        }
    }
}