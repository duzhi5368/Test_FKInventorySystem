namespace FKGame
{
    public class IPGeolocationDetail
    {
        public string formURI { get; set; }         // 从什么地址获取的
        public float useTime { get; set; }          // 获取时消耗时间
        public string ipv4 { get; set; }
        public string ipv6 { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }    // ISO 3166-1 alpha-2 
        public string city { get; set; }

        public void SetIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return;
            if (ip.Contains("."))
                ipv4 = ip;
            else if (ip.Contains(":"))
                ipv6 = ip;
        }
    }
}