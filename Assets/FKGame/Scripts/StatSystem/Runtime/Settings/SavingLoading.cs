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
        [InspectorLabel("自动保存", "是否自动保存本数据库")]
        public bool autoSave = true;
        [InspectorLabel("主键名", "本数据库保存的键名")]
        public string savingKey = "Player";
        [InspectorLabel("时间间隔", "单位：秒")]
        public float savingRate = 60f;
        [InspectorLabel("存储方案", "默认使用 PlayerPrefs，可自行拓展类")]
        public SavingProvider provider;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}