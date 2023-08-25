using FKGame.Macro;
//------------------------------------------------------------------------
// 属性系统 -> 设置 -> 默认
//------------------------------------------------------------------------
namespace FKGame.StatSystem.Configuration
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
        [InspectorLabel(LanguagesMacro.IS_SHOW_DEBUG_INFO)]
        public bool debugMessages = true;
    }
}