using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ServiceManager
    {
        private Dictionary<Type, ServiceBase> allService = new Dictionary<Type, ServiceBase>();
        private NetworkServerManager netManager;
        private bool isStart = false;

        public void Init(NetworkServerManager netManager)
        {
            this.netManager = netManager;
            allService.Clear();
            Type[] childTypes = ReflectionTool.FastGetChildTypes(typeof(ServiceBase));
            foreach (var item in childTypes)
            {
                if (item.IsAbstract)
                    continue;
                Add(item);
            }
            foreach (var item in allService)
            {
                try
                {
                    item.Value.OnInit();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public ServiceBase Add(Type type)
        {
            ServiceBase t = null;
            if (allService.ContainsKey(type))
            {
                Debug.Log("Repeat to add service:" + type);
                t = allService[type];
            }
            else
            {
                t = (ServiceBase)Activator.CreateInstance(type);
                allService.Add(type, t);
                t.SetServiceManager(this);
                t.SetMessageManager(netManager.MsgManager);
                t.SetNetworkServerManager(netManager);
            }
            return t;
        }

        public T Add<T>() where T : ServiceBase, new()
        {
            Type type = typeof(T);
            return (T)Add(type);
        }

        public T Get<T>() where T : ServiceBase, new()
        {
            Type type = typeof(T);
            if (allService.ContainsKey(type))
            {
                return (T)allService[type];
            }
            else
            {
                return default(T);
            }
        }

        public void StartAll()
        {
            foreach (var item in allService.Values)
            {
                try
                {
                    item.OnStart();
                    item.Enable = true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            isStart = true;
        }

        public void Update(float deltaTime)
        {
            if (!isStart)
                return;
            foreach (var item in allService.Values)
            {
                try
                {
                    item.OnUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void StopAll()
        {
            foreach (var item in allService.Values)
            {
                try
                {
                    item.Enable = false;
                    item.OnStop();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

            }
            allService.Clear();
            isStart = false;
        }
    }
}