using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageSettingConfig
    {
        public SystemLanguage defaultLanguage = SystemLanguage.Unknown;                 // 默认语言
        public List<SystemLanguage> gameExistLanguages = new List<SystemLanguage>();    // 游戏存在的语言
        public bool useSystemLanguage = true;                                           // 是否自动匹配手机系统语言
    }
}