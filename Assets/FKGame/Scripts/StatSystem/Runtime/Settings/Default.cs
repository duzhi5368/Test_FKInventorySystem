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
        [InspectorLabel("显示DEBUG信息", "是否显示DEBUG信息")]
        public bool debugMessages = true;
    }
}