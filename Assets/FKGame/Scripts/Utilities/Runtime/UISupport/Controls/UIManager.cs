using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
namespace FKGame
{
    // UI�ص�
    public delegate void UICallBack(UIWindowBase UI, params object[] objs);
    public delegate void UIAnimCallBack(UIWindowBase UIbase, UICallBack callBack, params object[] objs);

    [RequireComponent(typeof(UIStackManager))]
    [RequireComponent(typeof(UILayerManager))]
    [RequireComponent(typeof(UIAnimManager))]
    public class UIManager : MonoBehaviour
    {
        private static GameObject s_UIManagerGo;
        private static UILayerManager s_UILayerManager;     // UI�㼶������
        private static UIAnimManager s_UIAnimManager;       // UI����������
        private static UIStackManager s_UIStackManager;     // UIջ������
        private static EventSystem s_EventSystem;
        public static Dictionary<string, List<UIWindowBase>> s_UIs = new Dictionary<string, List<UIWindowBase>>();      //�򿪵�UI
        public static Dictionary<string, List<UIWindowBase>> s_hideUIs = new Dictionary<string, List<UIWindowBase>>();  //���ص�UI
        private static bool isInit;
        private static Regex uiKey = new Regex(@"(\S+)\d+");

        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;
                GameObject instance = GameObject.Find("UIManager");
                if (instance == null)
                {
                    instance = GameObjectManager.CreateGameObjectByPool("UIManager");
                }
                UIManagerGo = instance;
                s_UILayerManager = instance.GetComponent<UILayerManager>();
                s_UIAnimManager = instance.GetComponent<UIAnimManager>();
                s_UIStackManager = instance.GetComponent<UIStackManager>();
                s_EventSystem = instance.GetComponentInChildren<EventSystem>();
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(instance);
                }
            }
        }

        private static void SetUIManager(GameObject instance)
        {
            UIManagerGo = instance;
            UILayerManager = instance.GetComponent<UILayerManager>();
            UIAnimManager = instance.GetComponent<UIAnimManager>();
            DontDestroyOnLoad(instance);
        }

        public static UILayerManager UILayerManager
        {
            get
            {
                if (s_UILayerManager == null)
                {
                    Init();
                }
                return s_UILayerManager;
            }
            set
            {
                s_UILayerManager = value;
            }
        }

        public static UIAnimManager UIAnimManager
        {
            get
            {
                if (s_UILayerManager == null)
                {
                    Init();
                }
                return s_UIAnimManager;
            }
            set
            {
                s_UIAnimManager = value;
            }
        }

        public static UIStackManager UIStackManager
        {
            get
            {
                if (s_UIStackManager == null)
                {
                    Init();
                }
                return s_UIStackManager;
            }
            set
            {
                s_UIStackManager = value;
            }
        }

        public static EventSystem EventSystem
        {
            get
            {
                if (s_EventSystem == null)
                {
                    Init();
                }
                return s_EventSystem;
            }
            set
            {
                s_EventSystem = value;
            }
        }

        public static GameObject UIManagerGo
        {
            get
            {
                if (s_UIManagerGo == null)
                {
                    Init();
                }
                return s_UIManagerGo;
            }
            set
            {
                s_UIManagerGo = value;
            }
        }

        public static void SetEventSystemEnable(bool enable)
        {
            if (EventSystem != null)
            {
                EventSystem.enabled = enable;
            }
            else
            {
                Debug.LogError("EventSystem.current is null !");
            }
        }

        public static string[] GetCameraNames()
        {
            string[] list = new string[UILayerManager.UICameraList.Count];
            for (int i = 0; i < UILayerManager.UICameraList.Count; i++)
            {
                list[i] = UILayerManager.UICameraList[i].m_key;
            }
            return list;
        }

        public static Camera GetCamera(string CameraKey = null)
        {
            var data = UILayerManager.GetUICameraDataByKey(CameraKey);
            return data.m_camera;
        }

        // ��һ��UI�ƶ�����һ��UICamera��
        public static void ChangeUICamera(UIWindowBase ui, string cameraKey)
        {
            UILayerManager.SetLayer(ui, cameraKey);
        }

        // ��һ��UI���·Ż���ԭ����UICamera��
        public static void ResetUICamera(UIWindowBase ui)
        {
            UILayerManager.SetLayer(ui, ui.cameraKey);
        }

        // ����UI,�������������Hide�б���
        public static T CreateUIWindow<T>() where T : UIWindowBase
        {
            return (T)CreateUIWindow(typeof(T).Name);
        }

        public static UIWindowBase CreateUIWindow(string UIName)
        {
            Debug.Log("CreateUIWindow " + UIName);
            GameObject UItmp = GameObjectManager.CreateGameObjectByPool(UIName, UIManagerGo);
            UIWindowBase UIWIndowBase = UItmp.GetComponent<UIWindowBase>();
            UISystemEvent.Dispatch(UIWIndowBase, UIEvent.OnInit);  // �ɷ�OnInit�¼�
            UIWIndowBase.windowStatus = WindowStatus.Create;
            try
            {
                UIWIndowBase.InitWindow(GetUIID(UIName));
            }
            catch (Exception e)
            {
                Debug.LogError(UIName + "OnInit Exception: " + e.ToString());
            }
            AddHideUI(UIWIndowBase);
            UILayerManager.SetLayer(UIWIndowBase);                  // ���ò㼶
            return UIWIndowBase;
        }

        /// <summary>
        /// ��UI
        /// </summary>
        /// <param name="UIName">UI��</param>
        /// <param name="callback">����������ϻص�</param>
        /// <param name="objs">�ص�����</param>`
        /// <returns>���ش򿪵�UI</returns>
        public static UIWindowBase OpenUIWindow(string UIName, UICallBack callback = null, params object[] objs)
        {
            UIWindowBase UIbase = GetHideUI(UIName);
            if (UIbase == null)
            {
                UIbase = CreateUIWindow(UIName);
            }
            RemoveHideUI(UIbase);
            AddUI(UIbase);
            UIStackManager.OnUIOpen(UIbase);
            UILayerManager.SetLayer(UIbase);                    // ���ò㼶
            UIbase.windowStatus = WindowStatus.OpenAnim;
            UISystemEvent.Dispatch(UIbase, UIEvent.OnOpen);     // �ɷ�OnOpen�¼�
            try
            {
                UIbase.OnOpen();
            }
            catch (Exception e)
            {
                Debug.LogError(UIName + " OnOpen Exception: " + e.ToString());
            }
            UISystemEvent.Dispatch(UIbase, UIEvent.OnOpened);       // �ɷ�OnOpened�¼�
            UIAnimManager.StartEnterAnim(UIbase, callback, objs);   // ���Ŷ���
            return UIbase;
        }

        public static T OpenUIWindow<T>() where T : UIWindowBase
        {
            return (T)OpenUIWindow(typeof(T).Name);
        }

        /// <summary>
        /// �ر�UI
        /// </summary>
        /// <param name="UI">Ŀ��UI</param>
        /// <param name="isPlayAnim">�Ƿ񲥷Źرն���</param>
        /// <param name="callback">����������ϻص�</param>
        /// <param name="objs">�ص�����</param>
        public static void CloseUIWindow(UIWindowBase UI, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
        {
            RemoveUI(UI);        // �Ƴ�UI����
            UI.RemoveAllListener();
            if (isPlayAnim)
            {
                // �����������ɾ��UI
                if (callback != null)
                {
                    callback += CloseUIWindowCallBack;
                }
                else
                {
                    callback = CloseUIWindowCallBack;
                }
                UI.windowStatus = WindowStatus.CloseAnim;
                UIAnimManager.StartExitAnim(UI, callback, objs);
            }
            else
            {
                CloseUIWindowCallBack(UI, objs);
            }
        }

        static void CloseUIWindowCallBack(UIWindowBase UI, params object[] objs)
        {
            UI.windowStatus = WindowStatus.Close;
            UISystemEvent.Dispatch(UI, UIEvent.OnClose);  // �ɷ�OnClose�¼�
            try
            {
                UI.OnClose();
            }
            catch (Exception e)
            {
                Debug.LogError(UI.UIName + " OnClose Exception: " + e.ToString());
            }
            UIStackManager.OnUIClose(UI);
            AddHideUI(UI);
            UISystemEvent.Dispatch(UI, UIEvent.OnClosed);  //�ɷ�OnClosed�¼�
        }

        public static void CloseUIWindow(string UIname, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
        {
            UIWindowBase ui = GetUI(UIname);
            if (ui == null)
            {
                Debug.LogError("CloseUIWindow Error UI ->" + UIname + "<-  not Exist!");
            }
            else
            {
                CloseUIWindow(GetUI(UIname), isPlayAnim, callback, objs);
            }
        }

        public static void CloseUIWindow<T>(bool isPlayAnim = true, UICallBack callback = null, params object[] objs) where T : UIWindowBase
        {
            CloseUIWindow(typeof(T).Name, isPlayAnim, callback, objs);
        }

        public static UIWindowBase ShowUI(string UIname)
        {
            UIWindowBase ui = GetUI(UIname);
            return ShowUI(ui);
        }

        public static UIWindowBase ShowUI(UIWindowBase ui)
        {
            ui.windowStatus = WindowStatus.Open;
            UISystemEvent.Dispatch(ui, UIEvent.OnShow);  // �ɷ�OnShow�¼�
            try
            {
                ui.Show();
                ui.OnShow();
            }
            catch (Exception e)
            {
                Debug.LogError(ui.UIName + " OnShow Exception: " + e.ToString());
            }
            return ui;
        }

        public static UIWindowBase HideUI(string UIname)
        {
            UIWindowBase ui = GetUI(UIname);
            return HideUI(ui);
        }

        public static UIWindowBase HideUI(UIWindowBase ui)
        {
            ui.windowStatus = WindowStatus.Hide;
            UISystemEvent.Dispatch(ui, UIEvent.OnHide);  // �ɷ�OnHide�¼�
            try
            {
                ui.Hide();
                ui.OnHide();
            }
            catch (Exception e)
            {
                Debug.LogError(ui.UIName + " OnShow Exception: " + e.ToString());
            }
            return ui;
        }

        public static void HideOtherUI(string UIName)
        {
            List<string> keys = new List<string>(s_UIs.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = s_UIs[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != UIName)
                    {
                        HideUI(list[j]);
                    }
                }
            }
        }

        public static void ShowOtherUI(string UIName)
        {
            List<string> keys = new List<string>(s_UIs.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = s_UIs[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].UIName != UIName)
                    {
                        ShowUI(list[j]);
                    }
                }
            }
        }

        // �Ƴ�ȫ��UI
        public static void CloseAllUI(bool isPlayerAnim = false)
        {
            List<string> keys = new List<string>(s_UIs.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                List<UIWindowBase> list = s_UIs[keys[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    CloseUIWindow(list[i], isPlayerAnim);
                }
            }
        }

        public static void CloseLastUI(UIType uiType = UIType.Normal)
        {
            UIStackManager.CloseLastUIWindow(uiType);
        }

        public static void OpenUIAsync<T>(UICallBack callback, params object[] objs) where T : UIWindowBase
        {
            string UIName = typeof(T).Name;
            OpenUIAsync(UIName, callback, objs);
        }

        public static void OpenUIAsync(string UIName, UICallBack callback, params object[] objs)
        {
            ResourceManager.LoadAsync(UIName, (resObject) =>
            {
                OpenUIWindow(UIName, callback, objs);
            });
        }

        public static void DestroyUI(UIWindowBase UI)
        {
            Debug.Log("UIManager DestroyUI " + UI.name);
            if (GetIsExitsHide(UI))
            {
                RemoveHideUI(UI);
            }
            else if (GetIsExits(UI))
            {
                RemoveUI(UI);
            }
            UISystemEvent.Dispatch(UI, UIEvent.OnDestroy);  // �ɷ�OnDestroy�¼�
            try
            {
                UI.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError("OnDestroy :" + e.ToString());
            }
            GameObjectManager.DestroyGameObjectByPool(UI.gameObject);
        }

        public static void DestroyAllUI()
        {
            DestroyAllActiveUI();
            DestroyAllHideUI();
        }

        // ɾ�����д򿪵�UI
        public static void DestroyAllActiveUI()
        {
            foreach (List<UIWindowBase> uis in s_UIs.Values)
            {
                for (int i = 0; i < uis.Count; i++)
                {
                    UISystemEvent.Dispatch(uis[i], UIEvent.OnDestroy);  // �ɷ�OnDestroy�¼�
                    try
                    {
                        uis[i].Dispose();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("OnDestroy :" + e.ToString());
                    }
                    GameObjectManager.DestroyGameObjectByPool(uis[i].gameObject);
                }
            }
            s_UIs.Clear();
        }

        public static T GetUI<T>() where T : UIWindowBase
        {
            return (T)GetUI(typeof(T).Name);
        }

        public static UIWindowBase GetUI(string UIname)
        {
            if (!s_UIs.ContainsKey(UIname))
            {
                return null;
            }
            else
            {
                if (s_UIs[UIname].Count == 0)
                {
                    return null;
                }
                else
                {
                    // Ĭ�Ϸ�����󴴽�����һ��
                    return s_UIs[UIname][s_UIs[UIname].Count - 1];
                }
            }
        }

        public static UIBase GetUIBaseByEventKey(string eventKey)
        {
            string UIkey = eventKey.Split('.')[0];
            string[] keyArray = UIkey.Split('_');
            string uiEventKey = "";
            UIBase uiTmp = null;
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (i == 0)
                {
                    uiEventKey = keyArray[0];
                    uiTmp = GetUIWindowByEventKey(uiEventKey);
                }
                else
                {
                    uiEventKey += "_" + keyArray[i];
                    uiTmp = uiTmp.GetItemByKey(uiEventKey);
                }
                Debug.Log("uiEventKey " + uiEventKey);
            }
            return uiTmp;
        }

        static UIWindowBase GetUIWindowByEventKey(string eventKey)
        {
            string UIname = uiKey.Match(eventKey).Groups[1].Value;
            if (!s_UIs.ContainsKey(UIname))
            {
                throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " + UIname);
            }
            List<UIWindowBase> list = s_UIs[UIname];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UIEventKey == eventKey)
                {
                    return list[i];
                }
            }
            throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " + UIname);
        }

        static bool GetIsExits(UIWindowBase UI)
        {
            if (!s_UIs.ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return s_UIs[UI.name].Contains(UI);
            }
        }

        static void AddUI(UIWindowBase UI)
        {
            if (!s_UIs.ContainsKey(UI.name))
            {
                s_UIs.Add(UI.name, new List<UIWindowBase>());
            }
            s_UIs[UI.name].Add(UI);
            UI.Show();
        }

        static void RemoveUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error UI is null: !");
            }
            if (!s_UIs.ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI name: ->" + UI.name + "<-  " + UI);
            }
            if (!s_UIs[UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI: ->" + UI.name + "<-  " + UI);
            }
            else
            {
                s_UIs[UI.name].Remove(UI);
            }
        }

        static int GetUIID(string UIname)
        {
            if (!s_UIs.ContainsKey(UIname))
            {
                return 0;
            }
            else
            {
                int id = s_UIs[UIname].Count;
                for (int i = 0; i < s_UIs[UIname].Count; i++)
                {
                    if (s_UIs[UIname][i].UIID == id)
                    {
                        id++;
                        i = 0;
                    }
                }
                return id;
            }
        }

        public static int GetNormalUICount()
        {
            return UIStackManager.m_normalStack.Count;
        }

        // ɾ���������ص�UI
        public static void DestroyAllHideUI()
        {
            foreach (List<UIWindowBase> uis in s_hideUIs.Values)
            {
                for (int i = 0; i < uis.Count; i++)
                {
                    UISystemEvent.Dispatch(uis[i], UIEvent.OnDestroy);  // �ɷ�OnDestroy�¼�
                    try
                    {
                        uis[i].Dispose();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("OnDestroy :" + e.ToString());
                    }
                    GameObjectManager.DestroyGameObjectByPool(uis[i].gameObject);
                }
            }
            s_hideUIs.Clear();
        }

        public static T GetHideUI<T>() where T : UIWindowBase
        {
            string UIname = typeof(T).Name;
            return (T)GetHideUI(UIname);
        }

        // ��ȡһ�����ص�UI,����ж��ͬ��UI���򷵻���󴴽�����һ��
        public static UIWindowBase GetHideUI(string UIname)
        {
            if (!s_hideUIs.ContainsKey(UIname))
            {
                return null;
            }
            else
            {
                if (s_hideUIs[UIname].Count == 0)
                {
                    return null;
                }
                else
                {
                    UIWindowBase ui = s_hideUIs[UIname][s_hideUIs[UIname].Count - 1];
                    // Ĭ�Ϸ�����󴴽�����һ��
                    return ui;
                }
            }
        }

        static bool GetIsExitsHide(UIWindowBase UI)
        {
            if (!s_hideUIs.ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return s_hideUIs[UI.name].Contains(UI);
            }
        }

        static void AddHideUI(UIWindowBase UI)
        {
            if (!s_hideUIs.ContainsKey(UI.name))
            {
                s_hideUIs.Add(UI.name, new List<UIWindowBase>());
            }
            s_hideUIs[UI.name].Add(UI);
            UI.Hide();
        }


        static void RemoveHideUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }
            if (!s_hideUIs.ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }
            if (!s_hideUIs[UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }
            else
            {
                s_hideUIs[UI.name].Remove(UI);
            }
        }
    }
}