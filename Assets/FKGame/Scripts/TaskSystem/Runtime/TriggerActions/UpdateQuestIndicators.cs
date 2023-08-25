using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu(LanguagesMacro.QUEST_SYSTEM + "/" + LanguagesMacro.UPDATE_QUEST_INDICATORS)]
    [System.Serializable]
    public class UpdateQuestIndicators : Action
    {
        public override ActionStatus OnUpdate()
        {
            QuestIndicator[] indicators = GameObject.FindObjectsOfType<QuestIndicator>();
            for (int i = 0; i < indicators.Length; i++) {
                indicators[i].UpdateQuestIndicator();
            }
            return ActionStatus.Success;
        }
    }
}