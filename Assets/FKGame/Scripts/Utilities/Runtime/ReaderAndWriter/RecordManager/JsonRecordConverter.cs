namespace FKGame
{
    public class JsonRecordConverter : IRecordConverter
    {
        public string GetFileExtend()
        {
            return ".json";
        }

        public string GetSaveDirectoryName()
        {
            return GlobeDefine.SAVE_RECORD_DIRECTORY;
        }

        public string Object2String(object obj)
        {
            return JsonSerializer.ToJson(obj);
        }

        public T String2Object<T>(string content)
        {
            T t = default(T);
            JsonSerializer.TryFromJson(out t, content);
            return t;
        }
    }
}