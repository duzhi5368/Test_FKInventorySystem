using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RemoteConsoleManager : MonoBehaviour
    {
        public static RemoteConsoleManager Instance;
        private bool isInit = false;
        private static bool isStart = false;

        private void Awake()
        {
            if (isInit)
                return;
            if (Instance)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            Init();
            ConsoleStart();
        }

        private static void Init()
        {
            if (Instance == null)
            {
                Instance = new GameObject("[ConsoleServer]").AddComponent<RemoteConsoleManager>();
            }
            if (Instance.isInit)
                return;
            Instance.isInit = true;
            DontDestroyOnLoad(Instance.gameObject);
        }
        public static bool ConsoleStart()
        {
            Init();
            if (isStart)
                return false;

            RemoteDeviceInfo deviceInfo = RemoteDeviceInfo.GetLocalDeviceInfo();
            deviceInfo.otherData.Add("ServerVersion", ServerVersionInfo.Version);
            deviceInfo.otherData.Add("MinClientVersion", ServerVersionInfo.MinClientVersion);
            RemoteConsoleSettingData config = RemoteConsoleSettingData.GetConfig();
            try
            {
                string deviceInfoStr = JsonSerializer.ToJson(deviceInfo);
                NetServer.Start(config.netPort);
                NetServer.DiscoverServer.Start(config.netPort, deviceInfoStr);
                LoginService loginService = NetServer.ServiceManager.Get<LoginService>();
                loginService.SetPlayerLoginHandler(new SimplePlayerLoginHandler());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            Debug.Log("Remote Console NetServer.port:" + config.netPort);
            isStart = true;
            return true;
        }

        private void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            NetServer.Update(deltaTime);
            ConsoleBootManager.OnUpdate();
        }

        private void OnGUI()
        {
            ConsoleBootManager.OnGUI();
        }

        private void OnApplicationQuit()
        {
            NetServer.Stop();
        }
    }
}