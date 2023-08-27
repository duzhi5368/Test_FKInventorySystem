using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem.Configuration
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

        [InspectorLabel(LanguagesMacro.MAX_DROP_DISTANCE)]
        public float maxDropDistance = 3f;

        [Header(LanguagesMacro.PHYSICS)]
        [InspectorLabel(LanguagesMacro.QUERIES_HIT_TRIGGERS)]
        public bool queriesHitTriggers = false;

        [Header(LanguagesMacro.DEBUG)]
        [InspectorLabel(LanguagesMacro.IS_SHOW_DEBUG_INFO)]
        public bool debugMessages = true;
        [InspectorLabel(LanguagesMacro.IS_SHOW_ALL_COMPONENTS)]
        public bool showAllComponents = false;
    }
}