using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageSettingConfig
    {
        public SystemLanguage defaultLanguage = SystemLanguage.Unknown;                 // Ĭ������
        public List<SystemLanguage> gameExistLanguages = new List<SystemLanguage>();    // ��Ϸ���ڵ�����
        public bool useSystemLanguage = true;                                           // �Ƿ��Զ�ƥ���ֻ�ϵͳ����
    }
}