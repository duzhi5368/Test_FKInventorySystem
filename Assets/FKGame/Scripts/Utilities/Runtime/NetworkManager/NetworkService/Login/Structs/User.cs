using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class User
    {
        public String userID = "";
        public String nickName;             // �ǳ�
        public String portrait;             // ͷ��
        public LoginPlatform loginType;     // ��¼ƽ̨����
        public String typeKey;              // ��¼�˺ţ�ƽ̨��ͬ����ͬ��
        public long playTime = 0;           // ��Ϸʱ��(����)
        public int totalLoginDays = 0;      // �ۼƵ�¼����
    }
}