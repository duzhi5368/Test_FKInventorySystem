using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FKGame.InventorySystem{
	[CustomPropertyDrawer(typeof(Currency))]
	public class CurrencyPickerDrawer : PickerDrawer<Currency> {

		protected override List<Currency> GetItems(ItemDatabase database) {
			return database.currencies;
		}
	}
}