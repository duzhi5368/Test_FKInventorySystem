using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem{
	[System.Serializable]
	public class Rarity : ScriptableObject, INameable {
		[SerializeField]
		[InspectorLabel(LanguagesMacro.NAME)]
		private new string name="";
		public string Name{
			get{return this.name;}
			set{this.name = value;}
		}

		[SerializeField]
        [InspectorLabel(LanguagesMacro.IS_USE_NAME_PREFIX)]
        private bool m_UseAsNamePrefix = false;
		public bool UseAsNamePrefix {
			get { return this.m_UseAsNamePrefix; }
		}

		[SerializeField]
        [InspectorLabel(LanguagesMacro.COLOR)]
        private Color color=Color.white;
		public Color Color{
			get{return this.color;}
			set{this.color = value;}
		}

		[SerializeField]
        [InspectorLabel(LanguagesMacro.CHANCE)]
        private int chance = 100;
		public int Chance
		{
			get { return this.chance; }
			set { this.chance = value; }
		}

		[InspectorLabel(LanguagesMacro.PROPERTY_MULTIPLIER)]
		[SerializeField]
		private float multiplier = 1.0f;
		public float Multiplier
		{
			get { return this.multiplier; }
			set { this.multiplier = value; }
		}

		[InspectorLabel(LanguagesMacro.PRICE_MULTIPLIER)]
		[SerializeField]
		private float m_PriceMultiplier = 1.0f;
		public float PriceMultiplier
		{
			get { return this.m_PriceMultiplier; }
			set { this.m_PriceMultiplier = value; }
		}
	}
}