using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GetServerDefine
    {
        public const String ClientVersion = "ClientVersion";                // 客户端版本
        public const String ClientPlatform = "ClientPlatform";              // 运行平台
        public const String Group = "Group";                                // 分组
        public const String SelectNetworkData = "SelectNetworkData";        // 返回可选择的服务器
        public const String ListenContext_clearCache = "/clearCache";       // 监听过滤字段
        public const String ListenContext_getAllServers = "/getAllServers"; // 监听过滤字段
        public const String ListenContext_getServer = "/getServer";         // 监听过滤字段
    }
}