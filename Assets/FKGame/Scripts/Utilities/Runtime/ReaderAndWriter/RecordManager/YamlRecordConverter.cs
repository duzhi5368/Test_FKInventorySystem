namespace FKGame
{
    public class YamlRecordConverter : IRecordConverter
    {
        public string GetFileExtend()
        {
            return ".yaml";
        }

        public string GetSaveDirectoryName()
        {
            return "Record";
        }

        public string Object2String(object obj)
        {
            return YamlUtils.ToYaml(obj);
        }

        public T String2Object<T>(string content)
        {
            return YamlUtils.FromYaml<T>(content);
        }
    }
}