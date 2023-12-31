﻿using UnityEditor;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem{
	[CustomPropertyDrawer(typeof(EquipmentRegion))]
	public class EquipmentPickerDrawer : PickerDrawer<EquipmentRegion> 
	{
		protected override List<EquipmentRegion> GetItems(ItemDatabase database) {
			return database.equipments;
		}
	}
}