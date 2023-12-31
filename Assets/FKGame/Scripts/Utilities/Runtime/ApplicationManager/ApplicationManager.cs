using System;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public delegate void ApplicationBoolCallback(bool status);
    public delegate void ApplicationVoidCallback();

    public class ApplicationManager : MonoBehaviour
    {
        private static ApplicationManager instance;
        public static ApplicationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ApplicationManager>();
                }
                return ApplicationManager.instance;
            }
            set { ApplicationManager.instance = value; }
        }

        public AppMode m_AppMode = AppMode.Developing;
        public static AppMode AppMode
        {
            get
            {
#if APPMODE_DEV
                return AppMode.Developing;
#elif APPMODE_QA
                return AppMode.QA;
#elif APPMODE_REL
                return AppMode.Release;
#else
                return instance.m_AppMode;
#endif
            }
        }

        public bool m_useAssetsBundle = false;
        public bool UseAssetsBundle
        {
            get
            {
#if USE_BUNDLE
                return true;
#else
                return m_useAssetsBundle;
#endif
            }
        }

        public static string Version
        {
            get
            {
                return Application.version + "." + HotUpdateManager.GetHotUpdateVersion();
            }
        }

        [Tooltip("是否记录输入到本地")]
        public bool m_recordInput = true;
        [Tooltip("是否快速启动")]
        public bool m_quickLunch = true;
        [HideInInspector]
        public string m_Status = "";
        [HideInInspector]
        public List<string> m_globalLogic;
        [HideInInspector]
        public string currentStatus;
        [Tooltip("是否显示语言值")]
        public bool showLanguageValue = false;
        [Tooltip("加载资源时是否使用缓存，Bundle加载不起作用(都为使用)")]
        public bool useCacheWhenLoadResource = true;
        public void Awake()
        {
            instance = this;

            if (Application.platform != RuntimePlatform.WindowsEditor &&
                Application.platform != RuntimePlatform.OSXEditor)
            {
                try
                {
                    string modeStr = PlayerPrefs.GetString("AppMode", m_AppMode.ToString());
                    m_AppMode = (AppMode)Enum.Parse(typeof(AppMode), modeStr);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            AppLaunch();
        }

        // 程序启动
        public void AppLaunch()
        {
            DontDestroyOnLoad(gameObject);
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            SetResourceLoadType(useCacheWhenLoadResource);              //设置资源加载类型
            AudioManager.Init();
            MemoryManager.Init();                                       //内存管理初始化
            TimerManager.Init();                                        //计时器启动
            InputManager.Init();                                        //输入管理器启动
#if !UNITY_WEBGL
            UIManager.Init();                                           //UIManager启动
#else
            UIManager.InitAsync();                                      //异步加载UIManager
#endif
            ApplicationStatusManager.Init();                            //游戏流程状态机初始化
            GlobalLogicManager.Init();                                  //初始化全局逻辑

            if (AppMode != AppMode.Release)
            {
                GUIConsole.Init();                                      //运行时Console
                DevelopReplayManager.OnLunchCallBack += () =>
                {
                    SDKManager.Init();                                  //初始化SDKManger
                    InitGlobalLogic();                                  //全局逻辑
                    ApplicationStatusManager.EnterTestModel(m_Status);  //可以从此处进入测试流程
                };

                DevelopReplayManager.Init(m_quickLunch);                //开发者复盘管理器
                LanguageManager.Init();
            }
            else
            {
                Log.Init(false);                                        //关闭 Debug
                SDKManager.Init();                                      //初始化SDKManger
                InitGlobalLogic();                                      //全局逻辑
                ApplicationStatusManager.EnterStatus(m_Status);         //游戏流程状态机，开始第一个状态
                LanguageManager.Init();
            }

            if (s_OnApplicationModuleInitEnd != null)
            {
                s_OnApplicationModuleInitEnd();
            }
        }

        // 框架模块初始化完成回调
        public static ApplicationVoidCallback s_OnApplicationModuleInitEnd = null;
        public static ApplicationVoidCallback s_OnApplicationQuit = null;
        public static ApplicationBoolCallback s_OnApplicationPause = null;
        public static ApplicationBoolCallback s_OnApplicationFocus = null;
        public static ApplicationVoidCallback s_OnApplicationUpdate = null;
        public static ApplicationVoidCallback s_OnApplicationFixedUpdate = null;
        public static ApplicationVoidCallback s_OnApplicationOnGUI = null;
        public static ApplicationVoidCallback s_OnApplicationOnDrawGizmos = null;
        public static ApplicationVoidCallback s_OnApplicationLateUpdate = null;

        void OnApplicationQuit()
        {
            if (s_OnApplicationQuit != null)
            {
                try
                {
                    s_OnApplicationQuit();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        /*
         * 强制暂停时，先 OnApplicationPause，后 OnApplicationFocus
         * 重新“启动”游戏时，先OnApplicationFocus，后 OnApplicationPause
         */
        void OnApplicationPause(bool pauseStatus)
        {
            if (s_OnApplicationPause != null)
            {
                try
                {
                    s_OnApplicationPause(pauseStatus);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        void OnApplicationFocus(bool focusStatus)
        {
            if (s_OnApplicationFocus != null)
            {
                try
                {
                    s_OnApplicationFocus(focusStatus);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        void Update()
        {
            if (s_OnApplicationUpdate != null)
                s_OnApplicationUpdate();
        }

        private void LateUpdate()
        {
            if (s_OnApplicationLateUpdate != null)
            {
                s_OnApplicationLateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (s_OnApplicationFixedUpdate != null)
                s_OnApplicationFixedUpdate();
        }

        void OnGUI()
        {
            if (s_OnApplicationOnGUI != null)
                s_OnApplicationOnGUI();
        }

        private void OnDrawGizmos()
        {
            if (s_OnApplicationOnDrawGizmos != null)
                s_OnApplicationOnDrawGizmos();
        }
        private void OnDestroy()
        {

        }

        // 设置资源加载方式
        void SetResourceLoadType(bool useCache)
        {
            if (UseAssetsBundle)
            {
                HotUpdateManager.CheckLocalVersion();
                ResourceManager.Initialize(AssetsLoadType.AssetBundle, useCache);
            }
            else
            {
                ResourceManager.Initialize(AssetsLoadType.Resources, useCache);
            }
        }

        // 初始化全局逻辑
        void InitGlobalLogic()
        {
            for (int i = 0; i < m_globalLogic.Count; i++)
            {
                GlobalLogicManager.InitLogic(m_globalLogic[i]);
            }
        }
    }
}