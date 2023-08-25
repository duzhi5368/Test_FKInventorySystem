using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [System.Serializable]
    public class SavingLoading : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.SAVING_AND_LOADING;
            }
        }
        [InspectorLabel(LanguagesMacro.SAVE_KEY, LanguagesMacro.SAVE_KEY_TIP)]
        public string savingKey = "Player";
        [InspectorLabel(LanguagesMacro.IS_AUTO_SAVE, LanguagesMacro.IS_AUTO_SAVE_TIP)]
        public bool autoSave = true;
        [InspectorLabel(LanguagesMacro.PROVIDER, LanguagesMacro.PROVIDER_TIP)]
        public SavingProvider provider = SavingProvider.PlayerPrefs;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}