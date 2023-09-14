using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class User
    {
        public String userID = "";
        public String nickName;             // 昵称
        public String portrait;             // 头像
        public LoginPlatform loginType;     // 登录平台类型
        public String typeKey;              // 登录账号（平台不同而不同）
        public long playTime = 0;           // 游戏时长(分钟)
        public int totalLoginDays = 0;      // 累计登录天数
    }
}