using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UIAnimManager : MonoBehaviour
    {
        // ��ʼ���ý��붯��
        public void StartEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
        {
            UISystemEvent.Dispatch(UIbase, UIEvent.OnStartEnterAnim);
            StartCoroutine(UIbase.EnterAnim(EndEnterAnim, callBack, objs));
        }

        // ���붯��������ϻص�
        public void EndEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
        {
            UISystemEvent.Dispatch(UIbase, UIEvent.OnCompleteEnterAnim);
            UIbase.OnCompleteEnterAnim();
            UIbase.windowStatus = WindowStatus.Open;
            try
            {
                if (callBack != null)
                {
                    callBack(UIbase, objs);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        // ��ʼ�����˳�����
        public void StartExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
        {
            UISystemEvent.Dispatch(UIbase, UIEvent.OnStartExitAnim);
            StartCoroutine(UIbase.ExitAnim(EndExitAnim, callBack, objs));
        }

        // �˳�����������ϻص�
        public void EndExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
        {
            UISystemEvent.Dispatch(UIbase, UIEvent.OnCompleteExitAnim);
            UIbase.OnCompleteExitAnim();
            UIbase.windowStatus = WindowStatus.Close;
            try
            {
                if (callBack != null)
                {
                    callBack(UIbase, objs);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}