using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame
{
    public interface ISelectable
    {
        bool enabled { get; }
        Vector3 position { get; }
        void OnSelect();
        void OnDeselect();
    }
}