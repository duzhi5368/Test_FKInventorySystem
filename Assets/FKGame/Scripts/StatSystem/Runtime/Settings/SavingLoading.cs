using FKGame.Macro;
//------------------------------------------------------------------------
// 属性系统 -> 设置 -> 保存&加载
//------------------------------------------------------------------------
namespace FKGame.StatSystem.Configuration
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
        [InspectorLabel(LanguagesMacro.IS_AUTO_SAVE, LanguagesMacro.IS_AUTO_SAVE_TIP)]
        public bool autoSave = true;
        [InspectorLabel(LanguagesMacro.SAVE_KEY, LanguagesMacro.SAVE_KEY_TIP)]
        public string savingKey = "Player";
        [InspectorLabel(LanguagesMacro.SAVE_RATE, LanguagesMacro.SAVE_RATE_TIP)]
        public float savingRate = 60f;
        [InspectorLabel(LanguagesMacro.PROVIDER, LanguagesMacro.PROVIDER_TIP)]
        public SavingProvider provider;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}