using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class GamePrepareFlowController
    {
        public static bool IsChinaIP;                                                           // �ͻ���IP��ϸ����
        public static IPGeolocationDetail iPGeolocationDetail;
        public const string UseTestMode = "GamePrepareFlow_UseTestMode";                        // �Ƿ���������ģʽ�����ڲ����ȸ��·������ȣ�
        public const string TestDownloadRegionURL = "GamePrepareFlow_TestDownloadRegionURL";    // ����һ�����Դ����б����õ�

        private static SimpleFlowManager m_FlowManager;
        public static SimpleFlowManager FlowManager
        {
            get
            {
                if (m_FlowManager == null)
                    m_FlowManager = new SimpleFlowManager();
                return m_FlowManager;
            }
        }

        // ��ʼ����Ĭ�ϴ����б����ص�ַ���ش���󽫲���ʹ��
        public static void Init(string[] pathArr)
        {
            Debug.Log("GamePrepareFlowController��ʼ��");
            bool isTestMode = PlayerPrefs.GetInt(UseTestMode, 0) == 0 ? false : true;
            if (isTestMode)
            {
                string url = PlayerPrefs.GetString(TestDownloadRegionURL, "");
                Debug.Log("����GamePrepareFlowController test mode��\n URL:" + url);
                if (string.IsNullOrEmpty(url))
                {
                    Debug.LogError("GamePrepareFlowController test mode ��ȡ�Ĳ���URL Ϊ null");
                }
                pathArr = new string[] { url };
            }
            else
            {
                string selectNetworkPath = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_SelectNetworkPath, "");
                if (!string.IsNullOrEmpty(selectNetworkPath))
                {
                    pathArr = selectNetworkPath.Split('|');
                }
            }
            FlowManager.AddFlowItems(new FlowItemBase[]{ new DownloadRegionServerListFlowItem(),
                    new HotupdateFlowItem(), new PreLoadResFlowItem(), new SelectServerFlowItem()});

            FlowManager.GetFlowItem<DownloadRegionServerListFlowItem>().SetURLs(pathArr);
            FlowManager.GetFlowItem<DownloadRegionServerListFlowItem>().OnFinished += OnDownloadRegionServerListFlowItemFinish;
        }

        private static void OnDownloadRegionServerListFlowItemFinish(FlowItemBase arg1, string arg2)
        {
            iPGeolocationDetail = FlowManager.GetFlowItem<DownloadRegionServerListFlowItem>().iPGeolocationDetail;
            IsChinaIP = FlowManager.GetFlowItem<DownloadRegionServerListFlowItem>().IsChinaIP;
        }

        public static void Start(System.Type flowItemType)
        {
            FlowManager.RunFlowItem(flowItemType);
        }

        public static void RetryCurrenStatus()
        {
            FlowManager.RunFlowItem(FlowManager.CurrentRunFlowItem.Name, true);
        }

        // �����������ǹرղ���ģʽ
        public static void SetServerTestMode(bool isTestMode, string testURL = null)
        {
            int intState = isTestMode ? 1 : 0;
            PlayerPrefs.SetInt(UseTestMode, intState);
            if (!string.IsNullOrEmpty(testURL))
                PlayerPrefs.SetString(TestDownloadRegionURL, testURL);
        }
    }
}