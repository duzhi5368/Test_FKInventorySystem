using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame.InventorySystem
{
    public interface IGenerator
    {
        bool enabled{ get; set; }
    }
}