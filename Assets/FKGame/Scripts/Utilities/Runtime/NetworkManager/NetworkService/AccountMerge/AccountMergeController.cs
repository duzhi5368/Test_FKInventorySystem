using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ����ƽ̨�˺Ű�
    public class AccountMergeController
    {
        // ��Ҫ�󶨵�ƽ̨�Ѵ��ڻص�
        public static CallBack<AccountMergeInfo2Client> OnMergeAccountExist;
        // ���հ󶨵Ľ���ص�
        public static CallBack<ConfirmMergeExistAccount2Client> OnConfirmMergeExistAccountCallback;
        // ���ص�ǰ�˻��Ѿ��󶨵�ƽ̨��������ǰ��¼ƽ̨��
        public static CallBack<List<LoginPlatform>> OnRequsetAreadyBindPlatformCallBack;
        private static List<LoginPlatform> alreadyBindPlatform = new List<LoginPlatform>();
        public static bool isWaiting = false;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            GlobalEvent.AddTypeEvent<AccountMergeInfo2Client>(OnAccountMergeInfo);
            GlobalEvent.AddTypeEvent<ConfirmMergeExistAccount2Client>(OnConfirmMergeExistAccount);
            GlobalEvent.AddTypeEvent<RequsetAreadyBindPlatform2Client>(OnRequsetAreadyBindPlatform);
            LoginGameController.OnUserLogin += OnUserLogin;
        }

        private static void OnUserLogin(UserLogin2Client t)
        {
            RequsetAreadyBindPlatform();
        }

        private static void OnRequsetAreadyBindPlatform(RequsetAreadyBindPlatform2Client e, object[] args)
        {
            alreadyBindPlatform = e.areadyBindPlatforms;
            if (OnRequsetAreadyBindPlatformCallBack != null)
            {
                OnRequsetAreadyBindPlatformCallBack(e.areadyBindPlatforms);
            }
        }

        private static void OnConfirmMergeExistAccount(ConfirmMergeExistAccount2Client e, object[] args)
        {
            if (e.code == 0)
            {
                if (alreadyBindPlatform.Contains(e.loginType))
                    Debug.LogError("�Ѱ�����ƽ̨��" + e.loginType);
                else
                    alreadyBindPlatform.Add(e.loginType);
            }
            else
            {
                Debug.LogError("�󶨳���");
            }
            if (OnConfirmMergeExistAccountCallback != null)
            {
                OnConfirmMergeExistAccountCallback(e);
            }
        }

        private static void OnAccountMergeInfo(AccountMergeInfo2Client e, object[] args)
        {
            if (e.code == 0)
                Debug.Log("Ҫ�󶨵��˻��Ѵ��ڣ�" + e.mergeAccount.userID);
            if (OnMergeAccountExist != null)
            {
                OnMergeAccountExist(e);
            }
        }


        // ������Щ���Ѱ󶨵�ƽ̨��Ϣ
        private static void RequsetAreadyBindPlatform()
        {
            RequsetAreadyBindPlatform2Server msg = new RequsetAreadyBindPlatform2Server();
            JsonMessageProcessingController.SendMessage(msg);
        }

        // ���ص�ǰ�˻��Ѿ��󶨵�ƽ̨��������ǰ��¼ƽ̨��
        public static List<LoginPlatform> GetAreadyBindPlatform()
        {
            return alreadyBindPlatform;
        }

        // �ж��Ƿ���ʹ���̵꣨�����ǵ�ǰ���ο͵�¼������δ��������¼��ʽ��
        public static bool CheckCanUseStore(LoginPlatform loginType)
        {
            if (alreadyBindPlatform.Contains(loginType) && alreadyBindPlatform.Count == 1 && loginType == LoginPlatform.Tourist)
                return false;
            return true;
        }

        // ��Ҫ�󶨵��˻��Ѵ��ڣ�ȷ�Ϻϲ�
        public static void ConfirmMerge(bool useCurrentAccount)
        {
            ConfirmMergeExistAccount2Server msg = new ConfirmMergeExistAccount2Server();
            msg.useCurrentAccount = useCurrentAccount;
            JsonMessageProcessingController.SendMessage(msg);
        }

        // ������˻�
        public static void MergeLoginPlatform(LoginPlatform loginPlatform, string accountID = "", string pw = "")
        {
            if (isWaiting)
            {
                Debug.LogError("AccountMergeController => �ȴ�sdk���ص�¼��Ϣ");
                return;
            }
            isWaiting = true;

            SDKManager.LoginCallBack += SDKLoginCallBack;
            string tag = "";
            if (loginPlatform == LoginPlatform.AccountLogin)
            {
                accountID = accountID.Trim();
                pw = pw.Trim();
                string pwMd5 = MD5Utils.GetObjectMD5(pw);
                tag = accountID + "|" + pwMd5;
            }
            SDKManager.LoginByPlatform(loginPlatform, tag);
        }

        private static void SDKLoginCallBack(OnLoginInfo info)
        {
            isWaiting = false;
            SDKManager.LoginCallBack -= SDKLoginCallBack;

            if (info.isSuccess)
            {
                AccountMergeInfo2Server msg = AccountMergeInfo2Server.GetMessage(info.loginPlatform, info.accountId, info.password);
                JsonMessageProcessingController.SendMessage(msg);
            }
            else
            {
                //sdk��¼ʧ��
                if (OnConfirmMergeExistAccountCallback != null)
                {
                    ConfirmMergeExistAccount2Client msg = new ConfirmMergeExistAccount2Client();
                    msg.code = -1;
                    msg.loginType = info.loginPlatform;
                    OnConfirmMergeExistAccountCallback(msg);
                }
            }
        }
    }
}