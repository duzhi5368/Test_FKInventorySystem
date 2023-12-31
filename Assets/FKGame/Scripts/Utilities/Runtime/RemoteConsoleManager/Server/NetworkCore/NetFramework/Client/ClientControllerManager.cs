using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ClientControllerManager
    {
        private Dictionary<Type, ClientControllerBase> allService = new Dictionary<Type, ClientControllerBase>();
        public NetworkClientManager netManager;

        public void Init(NetworkClientManager netManager)
        {
            this.netManager = netManager;
            allService.Clear();
            Type[] childTypes = ReflectionTool.FastGetChildTypes(typeof(ClientControllerBase));
            foreach (var item in childTypes)
            {
                if (item.IsAbstract)
                    continue;
                Add(item);
            }
            foreach (var item in allService)
            {
                item.Value.OnInit();
            }
        }

        public ClientControllerBase Add(Type type)
        {
            ClientControllerBase t = null;
            if (allService.ContainsKey(type))
            {
                Debug.Log("Repeat to add service:" + type);
                t = allService[type];
            }
            else
            {
                t = (ClientControllerBase)Activator.CreateInstance(type);
                allService.Add(type, t);
                t.SetMessageManager(netManager.MsgManager);
                t.SetNetworkClientManager(netManager);
                t.SetNetControllerManager(this);
            }
            return t;
        }

        public T Add<T>() where T : ClientControllerBase, new()
        {
            Type type = typeof(T);
            return (T)Add(type);
        }

        public T Get<T>() where T : ClientControllerBase, new()
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
                item.OnStart();
                item.Enable = true;
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var item in allService.Values)
            {
                item.OnUpdate(deltaTime);
            }
        }

        public void StopAll()
        {
            foreach (var item in allService.Values)
            {
                item.Enable = false;
                item.OnStop();
            }
            allService.Clear();
        }
    }
}