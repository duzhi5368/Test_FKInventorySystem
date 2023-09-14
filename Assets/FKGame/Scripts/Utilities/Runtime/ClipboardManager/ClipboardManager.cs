using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ClipboardManager : MonoBehaviour
    {
#if UNITY_IPHONE
                /* Interface to native implementation */
                [DllImport ("__Internal")]
                private static extern void _copyTextToClipboard(string text);
#endif

        // ���Ƶ�������
        public static void ToClipboard(string input)
        {
            Debug.LogWarning("===ToClipboard====" + input);

#if UNITY_EDITOR
            GUIUtility.systemCopyBuffer = input;

#elif UNITY_ANDROID
        AndroidJavaObject androidObject = new AndroidJavaObject("clipboard.houling.com.clipboardlib.ClipboardTool");
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        if (activity == null)
            return;
        // ���Ƶ�������
        androidObject.Call("copyTextToClipboard", activity, input);

#elif UNITY_IPHONE
        _copyTextToClipboard(input);
#endif
        }

        // ��ü������ϵ�����
        public static string GetClipboard()
        {
#if UNITY_EDITOR
            return GUIUtility.systemCopyBuffer;
#elif UNITY_ANDROID
        AndroidJavaObject androidObject = new AndroidJavaObject("clipboard.houling.com.clipboardlib.ClipboardTool");
        // �Ӽ������л�ȡ�ı�
        String text = androidObject.Call<String>("getTextFromClipboard");
        return text;
#else
        return "";
#endif
        }
    }
}