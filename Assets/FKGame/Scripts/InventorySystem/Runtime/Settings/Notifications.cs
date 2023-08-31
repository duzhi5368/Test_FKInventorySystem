using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public static class NotificationExtension
    {
        public static void Show(this UINotificationOptions options, params string[] replacements)
        {
            if (InventoryManager.UI.notification != null)
            {
                InventoryManager.UI.notification.AddItem(options, replacements);
            }
        }
    }
}

namespace FKGame.InventorySystem.Configuration
{
    [System.Serializable]
    public class Notifications : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.NOTIFICATION;
            }
        }
        [Header("Container:")]
        public UINotificationOptions containerFull = new UINotificationOptions()
        {
            text = "{0} is full!"
        };

        public UINotificationOptions failedRestriction = new UINotificationOptions()
        {
            text = "{0} can't be added to {1}."
        };

        public UINotificationOptions missingItem = new UINotificationOptions()
        {
            text = "This action requires {0}."
        };

        public UINotificationOptions missingCategoryItem = new UINotificationOptions()
        {
            text = "This action requires {0}."
        };

        public UINotificationOptions inCooldown = new UINotificationOptions()
        {
            text = "{0} is in cooldown for {1} seconds!"
        };
        public UINotificationOptions selectItem = new UINotificationOptions()
        {
            text = "You need to select an item first."
        };

        [Header("Crafting:")]
        public UINotificationOptions alreadyCrafting = new UINotificationOptions()
        {
            text = "You are already crafting."
        };
        public UINotificationOptions craftedItem = new UINotificationOptions()
        {
            text = "Successfully crafted {0}"
        };
        public UINotificationOptions missingIngredient = new UINotificationOptions()
        {
            text = "You don't have all ingredients to craft this item!"
        };
        public UINotificationOptions failedToCraft = new UINotificationOptions()
        {
            text = "You failed to craft {0}."
        };

        [Header("Enchanting:")]
        public UINotificationOptions alreadyEnchanting = new UINotificationOptions()
        {
            text = "You are already enchating."
        };
        public UINotificationOptions enchantedItem = new UINotificationOptions()
        {
            text = "Successfully enchanted {0}."
        };
        public UINotificationOptions missingMaterials = new UINotificationOptions()
        {
            text = "You don't have all materials to enchant this item!"
        };
        public UINotificationOptions failedToEnchant = new UINotificationOptions()
        {
            text = "You failed to enchant {0}."
        };


        [Header("Vendor:")]
        public UINotificationOptions soldItem = new UINotificationOptions()
        {
            text = "Sold {0} for {1}."
        };
        public UINotificationOptions boughtItem = new UINotificationOptions()
        {
            text = "Bought {0} for {1}."
        };
        public UINotificationOptions noCurrencyToBuy = new UINotificationOptions()
        {
            text = "You don't have enough coins."
        };

        public UINotificationOptions cantSellItem = new UINotificationOptions()
        {
            text = "You can't sell this {0}!"
        };

        [Header("Trigger:")]
        public UINotificationOptions toFarAway = new UINotificationOptions()
        {
            text = "This is to far away!"
        };
        public UINotificationOptions inUse = new UINotificationOptions()
        {
            text = "My life is already fairly busy."
        };
        public UINotificationOptions empty = new UINotificationOptions()
        {
            text = "There is nothing to be found here."
        };

        [Header("Skills:")]
        public UINotificationOptions skillGain = new UINotificationOptions()
        {
            text = "Your {0} increased by {1}% to {2}%."
        };
    }
}