using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Samples.Helpers;
//------------------------------------------------------------------------
namespace YamlDotNet.Samples
{
    public class DeserializingMultipleDocuments
    {
        private readonly ITestOutputHelper output;

        public DeserializingMultipleDocuments(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Sample(
            Title = "反序列化流式多文档",
            Description = "演示如何从stream中加载多个YAML文档."
        )]
        public void Main()
        {
            var input = new StringReader(Document);

            var deserializer = new DeserializerBuilder().Build();

            var parser = new Parser(input);

            // Consume the stream start event "manually"
            parser.Expect<StreamStart>();

            while (parser.Accept<DocumentStart>())
            {
                // Deserialize the document
                var doc = deserializer.Deserialize<List<string>>(parser);

                output.WriteLine("## Document");
                foreach (var item in doc)
                {
                    output.WriteLine(item);
                }
            }
        }

        private const string Document = @"---
- Prisoner
- Goblet
- Phoenix
---
- Memoirs
- Snow 
- Ghost		
...";
    }
}
