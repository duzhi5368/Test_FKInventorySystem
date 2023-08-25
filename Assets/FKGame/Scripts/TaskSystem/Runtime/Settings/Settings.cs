using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [System.Serializable]
    public abstract class Settings : ScriptableObject, INameable
    {
        public virtual string Name
        {
            get { return LanguagesMacro.SETTING; }
            set { }
        }
    }
}