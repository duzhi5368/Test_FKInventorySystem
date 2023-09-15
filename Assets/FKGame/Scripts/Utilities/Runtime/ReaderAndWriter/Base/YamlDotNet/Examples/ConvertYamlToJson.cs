using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Samples.Helpers;
//------------------------------------------------------------------------
namespace YamlDotNet.Samples
{
    public class ConvertYamlToJson
    {
        private readonly ITestOutputHelper output;

        public ConvertYamlToJson(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Sample(
            Title = "转换 YAML->JSON 格式",
            Description = "演示如何将 YAML 文档转换为 JSON 格式."
        )]
        public void Main()
        {
            // convert string/file to YAML object
            var r = new StringReader(@"
                scalar: a scalar
                sequence:
                  - one
                  - two
                ");
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(r);

            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            string json = serializer.Serialize(yamlObject);

            output.WriteLine(json);

            var reader = new StringReader(json);
             yamlObject = deserializer.Deserialize(reader);
            var yamlSerializer = new SerializerBuilder().Build();
           var yaml=  yamlSerializer.Serialize(yamlObject);
            output.WriteLine(yaml);
        }
    }
}
