﻿using UnityEngine;
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
                return "Default";
            }
        }
        [Header("Debug")]
        public bool debugMessages = true;
    }
}