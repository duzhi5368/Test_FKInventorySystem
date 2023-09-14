using System;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class NetConfiguration
    {
        private INetworkTransport transport;
        public INetworkTransport Transport
        {
            get
            {
                return transport;
            }
        }
        public NetConfiguration(INetworkTransport transport)
        {
            this.transport = transport;
        }

        // 设置大端，小端，字节顺序
        public ByteOrder byteOrder { get; private set; }
        public NetConfiguration SetByteOrder(ByteOrder byteOrder)
        {
            this.byteOrder = byteOrder;
            return this;
        }

        // 启用统计
        public bool UseStatistics { get; private set; }
        public NetConfiguration EnableStatistics()
        {
            UseStatistics = true;
            return this;
        }

        // 消息处理句柄
        private IMessageHandler messageHander;
        public NetConfiguration AddMessageHander(IMessageHandler messageHander)
        {
            this.messageHander = messageHander;
            return this;
        }
        public IMessageHandler GetMessageHander()
        {
            return messageHander;
        }
        
        // 消息序列化
        private INetMsgSerializer serializer;
        public NetConfiguration AddMsgSerializer(INetMsgSerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }
        public INetMsgSerializer GetMsgSerializer()
        {
            return serializer;
        }

        // 消息类型处理插件
        private Dictionary<byte, INetMsgProcessPluginBase> plugins = new Dictionary<byte, INetMsgProcessPluginBase>();
        private List<INetMsgProcessPluginBase> pluginList = new List<INetMsgProcessPluginBase>();
        public NetConfiguration AddPlugin(INetMsgProcessPluginBase plugin)
        {
            if (plugin == null)
            {
                throw new System.Exception("NetConfiguration.AddPlugin Exception");
            }
            if (plugins.ContainsKey(plugin.GetNetProperty()))
            {
                INetMsgProcessPluginBase old = plugins[plugin.GetNetProperty()];
                plugins.Remove(plugin.GetNetProperty());
                pluginList.Remove(old);
            }
            plugins.Add(plugin.GetNetProperty(), plugin);
            pluginList.Add(plugin);
            return this;
        }

        public INetMsgProcessPluginBase GetPlugin(byte property)
        {
            INetMsgProcessPluginBase plugin = null;
            plugins.TryGetValue(property, out plugin);
            return plugin;
        }

        public List<INetMsgProcessPluginBase> GetNetMsgProcessPlugins()
        {
            return pluginList;
        }


        private IMsgEncryptionBase msgEncryption = new MsgEncryptionRC4(0);
        public bool IsEncryption { get; private set; }
        // 开启加密
        public NetConfiguration EnableEncryption()
        {
            IsEncryption = true;
            return this;
        }

        public IMsgEncryptionBase GetMsgEncryption()
        {
            return this.msgEncryption;
        }

        private Dictionary<byte, INetworkCoreMsgCompressBase> compressFun = new Dictionary<byte, INetworkCoreMsgCompressBase>();
        private INetworkCoreMsgCompressBase sendMsgCompress = null;
        private bool isAddAllCompressFun = false;
        private void AddAllCompressFun()
        {
            if (isAddAllCompressFun)
                return;
            isAddAllCompressFun = true;
            System.Type[] types = ReflectionTool.FastGetChildTypes(typeof(INetworkCoreMsgCompressBase), false);
            foreach (var t in types)
            {
                INetworkCoreMsgCompressBase obj = (INetworkCoreMsgCompressBase)ReflectionTool.CreateDefultInstance(t);
                compressFun.Add(obj.CompressType, obj);
            }

        }

        // 设置默认发消息压缩方式
        public NetConfiguration SetMsgCompress(string compressTypeName = "gzip")
        {
            AddAllCompressFun();
            if (string.IsNullOrEmpty(compressTypeName))
                return this;
            compressTypeName = compressTypeName.ToLower();
            foreach (var item in compressFun.Values)
            {
                if (item.CompressTypeName.ToLower() == compressTypeName)
                {
                    sendMsgCompress = item;
                    return this;
                }
            }
            throw new Exception("No Compress compressTypeName：" + compressTypeName);
        }

        // 使用byte编号设置发消息压缩方式
        public NetConfiguration SetMsgCompress(byte compressType)
        {
            AddAllCompressFun();
            foreach (var item in compressFun.Values)
            {
                if (item.CompressType == compressType)
                {
                    sendMsgCompress = item;
                    return this;
                }
            }
            throw new Exception("No Compress compressType：" + compressType);
        }

        public INetworkCoreMsgCompressBase GetSendCompressFunction()
        {
            return sendMsgCompress;
        }

        public INetworkCoreMsgCompressBase GetCompressFunction(byte compressType)
        {
            INetworkCoreMsgCompressBase compress = null;
            compressFun.TryGetValue(compressType, out compress);
            return compress;
        }
 
        public bool UseMultithreading { get { return useMultithreading; } }
        private bool useMultithreading = true;

        // 不使用多线程收发消息
        public NetConfiguration DisableMultithreading()
        {
            useMultithreading = false;
            return this;
        }

        internal virtual void Init(NetworkCommon networkCommon)
        {
            foreach (var plugin in plugins)
            {
                plugin.Value.Init(networkCommon);
            }
            AddAllCompressFun();
            foreach (var compress in compressFun)
            {
                compress.Value.Init();
            }
            if (msgEncryption != null)
            {
                msgEncryption.Init(byteOrder);
            }
            if (serializer != null)
            {
                serializer.Init(this);
            }
        }

        internal void Release()
        {
            foreach (var plugin in plugins)
            {
                plugin.Value.Release();
            }
            plugins.Clear();
            pluginList.Clear();

            foreach (var compress in compressFun)
            {
                compress.Value.Release();
            }
            compressFun.Clear();
            if (msgEncryption != null)
            {
                msgEncryption.Release();
                msgEncryption = null;
            }
            if (transport != null)
            {
                transport.Destroy();
                transport = null;
            }
        }
    }
}