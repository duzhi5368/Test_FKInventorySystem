using FKGame.Macro;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem.Configuration
{
    [System.Serializable]
    public class Input : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.INPUT;
            }
        }

        [Header("Unstacking:")]
        [InspectorLabel("Event")]
        [EnumFlags]
        public UnstackInput unstackEvent = UnstackInput.OnClick | UnstackInput.OnDrag;
        [InspectorLabel(LanguagesMacro.KEY_CODE)]
        public KeyCode unstackKeyCode = KeyCode.LeftShift;

        [Flags]
        public enum UnstackInput {
            OnClick = 1,
            OnDrag = 2
        }
    }
}