﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FKGame.InventorySystem{
	[CustomPropertyDrawer(typeof(Rarity))]
	public class RarityPickerDrawer : PickerDrawer<Rarity> {

		protected override List<Rarity> GetItems(ItemDatabase database) {
			return database.raritys;
		}
	}
}