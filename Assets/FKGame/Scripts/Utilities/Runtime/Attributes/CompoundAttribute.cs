using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame
{
    public class CompoundAttribute : PropertyAttribute
    {
        public readonly string propertyPath;

        public CompoundAttribute(string propertyPath) {
            this.propertyPath = propertyPath;
        }
    }
}