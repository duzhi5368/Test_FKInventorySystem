using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [System.Serializable]
    public class Default : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.DEFAULT;
            }
        }
        [InspectorLabel(LanguagesMacro.PLAYER_TAG)]
        public string playerTag = "Player";
        [InspectorLabel(LanguagesMacro.IS_SHOW_DEBUG_INFO)]
        public bool debugMessages = true;
    }
}