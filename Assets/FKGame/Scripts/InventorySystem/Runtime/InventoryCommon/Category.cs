using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem{
	[System.Serializable]
	public class Category : ScriptableObject,INameable {
		[AcceptNull]
		[SerializeField]
		protected Category m_Parent;
		public Category Parent
        {
			get { return this.m_Parent; }
			set { this.m_Parent = value; }
        }

		[SerializeField]
		[InspectorLabel(LanguagesMacro.NAME)]
		private new string name="";
		public string Name{
			get{return this.name;}
			set{this.name = value;}
		}

		[SerializeField]
        [InspectorLabel(LanguagesMacro.COLOR)]
        protected Color m_EditorColor = Color.clear;
		public Color EditorColor {
			get { return this.m_EditorColor; }
		}

		[Tooltip("类型公用冷却，例如多个药品，会有公用冷却CD")]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.CATEGORY_COOLDOWN)]
        protected float m_Cooldown = 1f;
        public float Cooldown {
            get { return this.m_Cooldown; }
        }

		public bool IsAssignable(Category other) {
			if (other == null)
				return false;
			if (this.Name == other.Name)
				return true;
			if (other.Parent != null) {
				return IsAssignable(other.Parent);
			}
			return false;
		}
	}
}