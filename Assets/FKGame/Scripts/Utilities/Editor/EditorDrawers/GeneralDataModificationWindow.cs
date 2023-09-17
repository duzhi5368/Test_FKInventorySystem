using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ͨ�������޸Ĵ���
    public class GeneralDataModificationWindow : EditorWindow
    {
        private static GeneralDataModificationWindow win;
        public static object otherParameter;               // ���ڴ�������Ҫʹ�õĲ���

        private string m_Title;
        private object modifi_Value;
        private EditorWindow otherWindow;
        CallBackR<object, object> customDrawGUI;
        CallBack<object> modificationCompleteCallBack;
        CallBackR<bool, object> checkCanOkButtonCallBack;

        public static GeneralDataModificationWindow GetInstance()
        {
            return win;
        }
        
        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="title">����</param>
        /// <param name="value">ϣ���޸ĵ�����</param>
        /// <param name="customDrawGUI">�Զ���GUI����</param>
        /// <param name="checkCanOkButtonCallBack">����Ƿ���ʹ��"OK"��ť</param>
        /// <param name="modificationCompleteCallBack">����޸Ļص�</param>
        public static void OpenWindow(EditorWindow otherWindow, string title, object value, CallBackR<object, object> customDrawGUI, CallBackR<bool, object> checkCanOkButtonCallBack, CallBack<object> modificationCompleteCallBack)
        {
            win = GetWindow<GeneralDataModificationWindow>();
            win.wantsMouseMove = true;
            win.autoRepaintOnSceneChange = true;
            win.otherWindow = otherWindow;
            FocusWindowIfItsOpen<GeneralDataModificationWindow>();

            win.m_Title = title;
            win.modifi_Value = value;
            win.customDrawGUI = customDrawGUI;
            win.modificationCompleteCallBack = modificationCompleteCallBack;
            win.checkCanOkButtonCallBack = checkCanOkButtonCallBack;
        }

        private void OnGUI()
        {
            if (modifi_Value == null)
                return;
            EditorDrawGUIUtil.DrawHorizontalCenter(() =>
            {
                GUILayout.Label(m_Title);
            });

            EditorDrawGUIUtil.DrawScrollView(modifi_Value, () =>
            {
                if (customDrawGUI != null)
                    modifi_Value = customDrawGUI(modifi_Value);
                else
                {
                    modifi_Value = EditorDrawGUIUtil.DrawBaseValue("Value", modifi_Value);
                }
                GUILayout.FlexibleSpace();
            });

            GUILayout.FlexibleSpace();
            bool isClose = false;
            EditorDrawGUIUtil.DrawHorizontalCenter(() =>
            {
                if (GUILayout.Button("OK", GUILayout.Width(position.width / 4)))
                {
                    if (checkCanOkButtonCallBack != null)
                    {
                        if (!checkCanOkButtonCallBack(modifi_Value))
                            return;
                    }
                    if (modificationCompleteCallBack != null)
                    {
                        modificationCompleteCallBack(modifi_Value);
                    }
                    if (otherWindow)
                        otherWindow.Repaint();
                    isClose = true;
                }
                if (GUILayout.Button("Cancel", GUILayout.Width(position.width / 4)))
                {
                    isClose = true;
                }
            });
            GUILayout.Space(6);

            if (isClose)
                Close();
        }
    }
}