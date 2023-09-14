using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LoginController : ClientControllerBase
    {
        private bool isLogin;
        private string key;
        private string password;
        public Action<Login2Client> Onlogin;
        public Action<Logout2Client> OnLogout;

        public bool IsLogin
        {
            get
            {
                return isLogin;
            }
        }
        public override void OnInit()
        {
            netManager.MsgManager.RegisterMsgEvent<Login2Client>(OnLoginEvent);
            netManager.MsgManager.RegisterMsgEvent<Logout2Client>(OnLogoutEvent);
            netManager.OnNetConnectStateChange += OnNetConnectStateChange;
        }

        private void OnNetConnectStateChange(NetConnectState state, EDisconnectInfo arg2)
        {
            if (state == NetConnectState.DisConnected)
            {
                RemovePlayer();
            }
        }

        public override void OnStop()
        {
            netManager.MsgManager.UnregisterMsgEvent<Login2Client>(OnLoginEvent);
            netManager.MsgManager.UnregisterMsgEvent<Logout2Client>(OnLogoutEvent);
            netManager.OnNetConnectStateChange -= OnNetConnectStateChange;
        }

        private void RemovePlayer()
        {
            if (netManager == null || netManager.Session == null)
                return;
            Player player = PlayerManager.GetPlayer(netManager.Session);
            if (player != null)
            {
                PlayerManager.RemovePlayer(player);
            }
            isLogin = false;
        }

        private void OnLogoutEvent(NetMessageData messageHandler)
        {
            Debug.Log("�ͻ��˽��յǳ�");
            if (IsLogin)
            {
                Logout2Client msg = messageHandler.GetMessage<Logout2Client>();
                if (OnLogout != null)
                {
                    OnLogout(msg);
                }
                RemovePlayer();
            }
        }

        private void OnLoginEvent(NetMessageData messageHandler)
        {
            Login2Client msg = messageHandler.GetMessage<Login2Client>();
            if (msg.code == 0)
            {
                isLogin = true;
                if (string.IsNullOrEmpty(msg.playerID))
                {
                    msg.playerID = "001";
                }
                Player player = new Player(messageHandler.session);
                player.playerID = msg.playerID;
                player.AddData(msg.appData);
                PlayerManager.AddPlayer(player);

            }
            if (Onlogin != null)
            {
                Onlogin(msg);
            }
            Debug.Log("��¼����:" + msg.code);
        }

        public void LoginByAccount(string key, string password)
        {
            this.key = key;
            this.password = password;
            string mixed = null;
            // ��������
            if (!string.IsNullOrEmpty(password))
            {
                string aesKey = Encoding.UTF8.GetString(AESService.GetRandomKey(32));
                string pwMD5 = MD5Tool.GetObjectMD5(password);
                string md5Ery = AESService.AESEncrypt(pwMD5, aesKey);
                string length = md5Ery.Length.ToString().PadLeft(4, '0');
                mixed = length + md5Ery + aesKey;
            }
            Login2Server msg = new Login2Server();
            msg.loginType = LoginType.Account;
            msg.key = key;
            msg.password = mixed;
            bool res = netManager.Send(msg);
            Debug.Log("Send Res:" + res);
        }

        public void ReLogin()
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("No record key password to login!");
                return;
            }
            LoginByAccount(key, password);
        }
        public void Logout()
        {
            if (IsLogin)
            {
                netManager.Send(new Logout2Server());
            }
        }
    }
}