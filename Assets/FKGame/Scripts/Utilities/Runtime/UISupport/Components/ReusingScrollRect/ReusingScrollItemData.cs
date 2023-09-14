using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ReusingScrollItemData : Dictionary<string, object>
    {
        public float m_size;
        public Bounds m_bounds;
        public Bounds GetBounds()
        {
            return new Bounds();
        }
    }
}