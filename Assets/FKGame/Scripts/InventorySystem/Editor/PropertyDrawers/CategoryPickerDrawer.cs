﻿using UnityEditor;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem{
	[CustomPropertyDrawer(typeof(Category))]
	public class CategoryPickerDrawer : PickerDrawer<Category> 
	{
		protected override List<Category> GetItems(ItemDatabase database) {
			return database.categories;
		}
	}
}