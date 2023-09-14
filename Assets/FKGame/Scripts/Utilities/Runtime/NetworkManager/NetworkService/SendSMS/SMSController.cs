using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SMSController
    {
        public static Action<ReplySendSMS2Client> SendResult;
        private static bool isInit = false;
        private static void Init()
        {
            if (isInit)
                return;
            isInit = true;
            GlobalEvent.AddTypeEvent<ReplySendSMS2Client>(OnReplySendSMS);
        }

        private static void OnReplySendSMS(ReplySendSMS2Client e, object[] args)
        {
            if (SendResult != null)
            {
                SendResult(e);
            }
        }

        /// <summary>
        /// ʹ��ģ�淢��
        /// </summary>
        /// <param name="internationalCode">���ʵ绰����,�й���86</param>
        /// <param name="phoneNumber">�绰</param>
        /// <param name="templateID">����ģ��ID</param>
        /// <param name="parameters">����ģ�����</param>
        public static void SendByTemplate(string internationalTelephoneCode, string phoneNumber, string templateID, string[] parameters)
        {
            Init();
            if (LoginGameController.IsLogin)
            {
                SendSMSData2Server msg = new SendSMSData2Server(internationalTelephoneCode, phoneNumber, templateID, parameters);
                JsonMessageProcessingController.SendMessage(msg);
            }
            else
            {
                Debug.LogError("���ȵ�¼");
            }
        }

        /// <summary>
        /// ����ģ�淢��
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="templateID"></param>
        /// <param name="parameters"></param>
        public static void SendByTemplate(string phoneNumber, string templateID, string[] parameters)
        {
            SendByTemplate("86", phoneNumber, templateID, parameters);
        }
    }
}