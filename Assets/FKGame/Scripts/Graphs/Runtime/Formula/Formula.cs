﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [CreateAssetMenu(fileName = "Formula", menuName = "FKGame/图/公式图")]
    [System.Serializable]
    public class Formula : ScriptableObject, IGraphProvider
    {
        [SerializeField]
        protected FormulaGraph m_Graph;

        public Graph GetGraph()
        {
            return this.m_Graph;
        }

        public static implicit operator float(Formula formula)
        {
            FormulaOutput output = formula.GetGraph().nodes.Find(x => x.GetType() == typeof(FormulaOutput)) as FormulaOutput;
            return output.GetInputValue<float>("result", output.result);
        }
    }
}