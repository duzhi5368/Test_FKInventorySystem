namespace FKGame
{
    public struct SystemInfoData
    {
        public string type;
        public string name;
        public string content;

        public SystemInfoData(string type, string name, string content)
        {
            this.type = type;
            this.name = name;
            this.content = content;
        }

        public override string ToString()
        {
            return name + "\t" + content;
        }
    }
}