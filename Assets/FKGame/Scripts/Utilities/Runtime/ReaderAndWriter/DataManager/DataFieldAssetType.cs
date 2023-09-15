namespace FKGame
{
    // 字段的用途区分
    public enum DataFieldAssetType
    {
        Data,                   // 单纯的数据
        LocalizedLanguage,      // 多语言字段
        Prefab,                 // 预制
        TableKey,               // 关联其他表格的key
        Texture,                // 图片资源
    }
}