using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SchemeData
    {
        public string SchemeName;
        public bool UseNewSDKManager;

        public List<SDKConfigData> LogScheme = new List<SDKConfigData>();
        public List<SDKConfigData> LoginScheme = new List<SDKConfigData>();
        public List<SDKConfigData> ADScheme = new List<SDKConfigData>();
        public List<SDKConfigData> PayScheme = new List<SDKConfigData>();
        public List<SDKConfigData> RealNameScheme = new List<SDKConfigData>();
        public List<SDKConfigData> OtherScheme = new List<SDKConfigData>();
    }
}