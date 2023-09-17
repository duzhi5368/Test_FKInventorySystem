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

        [Tooltip("�Ƿ��¼���뵽����")]
        public bool m_recordInput = true;
        [Tooltip("�Ƿ��������")]
        public bool m_quickLunch = true;
        [HideInInspector]
        public string m_Status = "";
        [HideInInspector]
        public List<string> m_globalLogic;
        [HideInInspector]
        public string currentStatus;
        [Tooltip("�Ƿ���ʾ����ֵ")]
        public bool showLanguageValue = false;
        [Tooltip("������Դʱ�Ƿ�ʹ�û��棬Bundle���ز�������(��Ϊʹ��)")]
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

        // ��������
        public void AppLaunch()
        {
            DontDestroyOnLoad(gameObject);
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            SetResourceLoadType(useCacheWhenLoadResource);              //������Դ��������
            AudioManager.Init();
            MemoryManager.Init();                                       //�ڴ�����ʼ��
            TimerManager.Init();                                        //��ʱ������
            InputManager.Init();                                        //�������������
#if !UNITY_WEBGL
            UIManager.Init();                                           //UIManager����
#else
            UIManager.InitAsync();                                      //�첽����UIManager
#endif
            ApplicationStatusManager.Init();                            //��Ϸ����״̬����ʼ��
            GlobalLogicManager.Init();                                  //��ʼ��ȫ���߼�

            if (AppMode != AppMode.Release)
            {
                GUIConsole.Init();                                      //����ʱConsole
                DevelopReplayManager.OnLunchCallBack += () =>
                {
                    SDKManager.Init();                                  //��ʼ��SDKManger
                    InitGlobalLogic();                                  //ȫ���߼�
                    ApplicationStatusManager.EnterTestModel(m_Status);  //���ԴӴ˴������������
                };

                DevelopReplayManager.Init(m_quickLunch);                //�����߸��̹�����
                LanguageManager.Init();
            }
            else
            {
                Log.Init(false);                                        //�ر� Debug
                SDKManager.Init();                                      //��ʼ��SDKManger
                InitGlobalLogic();                                      //ȫ���߼�
                ApplicationStatusManager.EnterStatus(m_Status);         //��Ϸ����״̬������ʼ��һ��״̬
                LanguageManager.Init();
            }

            if (s_OnApplicationModuleInitEnd != null)
            {
                s_OnApplicationModuleInitEnd();
            }
        }

        // ���ģ���ʼ����ɻص�
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
         * ǿ����ͣʱ���� OnApplicationPause���� OnApplicationFocus
         * ���¡���������Ϸʱ����OnApplicationFocus���� OnApplicationPause
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

        // ������Դ���ط�ʽ
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

        // ��ʼ��ȫ���߼�
        void InitGlobalLogic()
        {
            for (int i = 0; i < m_globalLogic.Count; i++)
            {
                GlobalLogicManager.InitLogic(m_globalLogic[i]);
            }
        }
    }
}