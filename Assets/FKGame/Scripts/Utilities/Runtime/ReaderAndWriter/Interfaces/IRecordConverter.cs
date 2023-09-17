namespace FKGame
{
    public interface IRecordConverter
    {
        string GetFileExtend();             // 获取文件后缀名
        string GetSaveDirectoryName();      // 保存目录的名字（不用加/）
        T String2Object<T>(string content); // 将text转换为对应数据对象
        string Object2String(object obj);   // 数据对象转换String的通用方法
    }
}