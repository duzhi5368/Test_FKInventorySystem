using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LoginService : ServiceBase
    {
        public Action<Player> OnPlayerLogin;
        public Action<Player> OnPlayerLoginAfter;
        public Action<Player> OnPlayerLogout;
        private IPlayerLoginHandlerBase playerLoginHandler;

        public override void OnStart()
        {
            msgManager.RegisterMsgEvent<Login2Server>(OnLoginMsg);
            msgManager.RegisterMsgEvent<Logout2Server>(OnLogoutMsg);
            netManager.OnPeerDisconnected += OnPeerDisconnected;
        }
        
        public void SetPlayerLoginHandler(IPlayerLoginHandlerBase handler)
        {
            playerLoginHandler = handler;
        }

        private void OnPeerDisconnected(Session session, EDisconnectInfo info)
        {
            Player player = PlayerManager.GetPlayer(session);
            LogoutAction(player);
        }

        private void OnLogoutMsg(NetMessageData messageHandler)
        {
            Player player = PlayerManager.GetPlayer(messageHandler.session);
            Debug.Log("����˽��յǳ�:" + player);
            LogoutAction(player);
        }

        private void LogoutAction(Player player)
        {
            if (player == null)
                return;
            if (PlayerManager.IsLogin(player.session))
            {
                PlayerManager.RemovePlayer(player);
                if (OnPlayerLogout != null)
                {
                    OnPlayerLogout(player);
                }
                netManager.Send(player.session, new Logout2Client());
            }
        }

        private void OnLoginMsg(NetMessageData messageHandler)
        {
            Debug.Log("���ܵ���½��Ϣ!");
            Login2Server msg = messageHandler.GetMessage<Login2Server>();
            bool isRightDecryptPW = true;
            // �������
            if (!string.IsNullOrEmpty(msg.password))
            {
                try
                {
                    string temp = msg.password;
                    // �������md5������
                    int length = int.Parse(temp.Substring(0, 4));
                    string md5Ery = temp.Substring(4, length);
                    string aesKey = temp.Substring(4 + length);
                    string pwMD5 = AESService.AESDecrypt(md5Ery, aesKey);
                    msg.password = pwMD5;
                }
                catch (Exception e)
                {
                    Debug.LogError("password Decrypt error:" + msg.password + "\n" + e);
                    isRightDecryptPW = false;
                }
            }

            Login2Client resMsg = new Login2Client();
            resMsg.appData = new AppData();
            resMsg.appData.serverAppName = Application.productName;
            resMsg.appData.serverAppVersion = Application.version;
            resMsg.appData.bundleIdentifier = Application.identifier;
            Player player = null;
            if (isRightDecryptPW)
            {
                if (PlayerManager.IsLogin(messageHandler.session))
                {
                    resMsg.code = 100;          // �ظ���½
                }
                else if (playerLoginHandler != null)
                {
                    resMsg.code = playerLoginHandler.LoginLogic(msg, messageHandler.session, out player);
                    if (resMsg.code == 0)
                    {
                        if (PlayerManager.IsLogin(player.playerID))
                        {
                            resMsg.code = 103;  // ��ǰ�˺��ѵ�¼
                        }
                        else
                        {
                            resMsg.playerID = player.playerID;
                        }
                    }
                }
                else
                {
                    resMsg.code = 101;          // ��������
                }
            }
            else
            {
                resMsg.code = 104;              // �����������
            }
            netManager.Send(messageHandler.session, resMsg);
            PlayerManager.AddPlayer(player);
            if (resMsg.code == 0)
            {
                if (OnPlayerLogin != null)
                    OnPlayerLogin(player);

                if (OnPlayerLoginAfter != null)
                {
                    OnPlayerLoginAfter(player);
                }
            }
        }
    }
}