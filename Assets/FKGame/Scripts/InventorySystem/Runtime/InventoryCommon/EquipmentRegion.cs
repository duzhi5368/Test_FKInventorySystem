using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
	[System.Serializable]
	public class EquipmentRegion : ScriptableObject, INameable
	{
		[SerializeField]
		[InspectorLabel(LanguagesMacro.EQUIP_REGION)]
		private new string name="";
		public string Name{
			get{return this.name;}
			set{this.name = value;}
		}
	}
}