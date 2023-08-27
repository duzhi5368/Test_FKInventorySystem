//------------------------------------------------------------------------
using FKGame.Macro;
using UnityEngine;

namespace FKGame.InventorySystem.Configuration
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
        [InspectorLabel(LanguagesMacro.IS_AUTO_SAVE)]
        public bool autoSave = true;
        [InspectorLabel(LanguagesMacro.SAVE_KEY)]
        [Tooltip(LanguagesMacro.SAVE_KEY_TIP)]
        public string savingKey = "Player";
        [InspectorLabel(LanguagesMacro.SAVE_RATE)]
        [Tooltip(LanguagesMacro.SAVE_RATE_TIP)]
        public float savingRate = 60f;
        public SavingProvider provider;

        public enum SavingProvider
        {
            PlayerPrefs,
        }
    }
}