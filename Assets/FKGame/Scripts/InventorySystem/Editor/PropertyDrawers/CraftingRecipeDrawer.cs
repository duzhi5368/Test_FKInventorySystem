using System.Collections.Generic;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [CustomPropertyDrawer(typeof(CraftingRecipe))]
    public class CraftingRecipeDrawer : PickerDrawer<CraftingRecipe>
    {
        protected override List<CraftingRecipe> GetItems(ItemDatabase database)
        {
            return database.craftingRecipes;
        }
    }
}