using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using FKGame.UIWidgets;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
	[System.Serializable]
	public class Item : ScriptableObject, INameable, IJsonSerializable
	{
		[SerializeField]
		[HideInInspector]
		private string m_Id;
		public string Id {
			get{ return this.m_Id; }
            set { this.m_Id = value; }
		}

        [Tooltip("物品唯一名称，可以用来显示到UI上")]
		[SerializeField]
        [InspectorLabel("物品名称")]
		private string m_ItemName = LanguagesMacro.NEW_ITEM;
		public string Name {
			get{ return this.m_ItemName; }
			set{ this.m_ItemName = value; }
		}

        [Tooltip("是否使用物品名称作为UI上显示的名称")]
        [SerializeField]
        [InspectorLabel("物品名称作为显示名称")]
        private bool m_UseItemNameAsDisplayName = true;
        [Tooltip("在UI上显示的物品名称")]
        [SerializeField]
        [InspectorLabel("物品显示名称")]
        private string m_DisplayName = "New Item";
        public string DisplayName
        {
            get 
            {
                string displayName = m_UseItemNameAsDisplayName ? this.m_ItemName : this.m_DisplayName;
                if (Rarity.UseAsNamePrefix)
                    displayName = Rarity.Name + " " + displayName;
                return displayName; 
            }
            set 
            {
                this.m_DisplayName = value; 
            }
        }

        [Tooltip("物品图标")]
        [SerializeField]
        [InspectorLabel("物品图标")]
        private Sprite m_Icon;
		public Sprite Icon {
			get{ return this.m_Icon; }
			set{ this.m_Icon = value; }
		}

        [Tooltip("物品拖拽处容器后显示的模型，也可用于丢弃到地上的模型")]
		[SerializeField]
		private GameObject m_Prefab;
		public GameObject Prefab {
			get{ return m_Prefab; }
			set{ this.m_Prefab = value; }
		}

        [Tooltip("物品描述信息")]
		[SerializeField]
		[Multiline (4)]
        private string m_Description = string.Empty;
		public string Description {
			get{ return this.m_Description; }
		}

        [Tooltip("物品所属类别，在排序和分类时使用")]
		[Header (LanguagesMacro.BEHAVIOUR)]
        [SerializeField]
        private Category m_Category = null;
        public Category Category
        {
            get { return this.m_Category; }
            set { this.m_Category = value; }
        }

        private static Rarity m_DefaultRarity;
        private static Rarity DefaultRarity {
            get {
                if (Item.m_DefaultRarity is null) {
                    Item.m_DefaultRarity = ScriptableObject.CreateInstance<Rarity>();
                    Item.m_DefaultRarity.Name = "None";
                    Item.m_DefaultRarity.Color = Color.grey;
                    Item.m_DefaultRarity.Chance = 100;
                    Item.m_DefaultRarity.Multiplier = 1.0f;
                }
                return Item.m_DefaultRarity;
             }
        }

        private Rarity m_Rarity;
		public Rarity Rarity {
			get{
                if (this.m_Rarity == null ) {
                    this.m_Rarity = DefaultRarity;
                }
                return this.m_Rarity; 
            }
            set { this.m_Rarity = value; }
		}

        [Tooltip(LanguagesMacro.IS_SELLABLE)]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.IS_SELLABLE)]
        private bool m_IsSellable = true;
        public bool IsSellable {
            get { return this.m_IsSellable; }
            set { this.m_IsSellable = true; }
        }

        [Tooltip(LanguagesMacro.BUY_PRICE)]
		[SerializeField]
        [InspectorLabel(LanguagesMacro.BUY_PRICE)]
		private int m_BuyPrice = 0;

		public int BuyPrice {
			get{ return Mathf.RoundToInt(m_BuyPrice*Rarity.PriceMultiplier); }
            set { this.m_BuyPrice = value; }
		}

        [Tooltip(LanguagesMacro.IS_CAN_BUY_BACK)]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.IS_CAN_BUY_BACK)]
        private bool m_CanBuyBack = true;
        public bool CanBuyBack { get { return this.m_CanBuyBack; } }

        [Tooltip(LanguagesMacro.BUY_CURRENCY)]
        [SerializeField]
        private Currency m_BuyCurrency=null;
        public Currency BuyCurrency
        {
            get { return this.m_BuyCurrency; }
            set { this.m_BuyCurrency = value; }
        }

        [Tooltip(LanguagesMacro.SELL_PRICE)]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.SELL_PRICE)]
        private int m_SellPrice=0;
		public int SellPrice {
			get{ return Mathf.RoundToInt(this.m_SellPrice*Rarity.PriceMultiplier); }
		}

        [Tooltip(LanguagesMacro.SELL_CURRENCY)]
        [SerializeField]
        private Currency m_SellCurrency= null;
        public Currency SellCurrency
        {
            get { return this.m_SellCurrency; }
            set { this.m_SellCurrency = value; }
        }

        [Tooltip(LanguagesMacro.STACK)]
        [SerializeField]
        [Range(1, 100)]
        private int m_Stack = 1;
        public virtual int Stack
        {
            get { return this.m_Stack; }
            set
            {
                this.m_Stack = value;
                if (Slot != null){
                    if (m_Stack <= 0 && !typeof(Currency).IsAssignableFrom(GetType())){
                        ItemContainer.RemoveItemCompletely(this);
                    }
                    Slot.Repaint();
                    for (int i = 0; i < ReferencedSlots.Count; i++){
                        ReferencedSlots[i].Repaint();
                    }
                }
            }
        }

        [Tooltip(LanguagesMacro.MAX_STACK)]
        [SerializeField]
		[Range (0, 100)]
        [InspectorLabel(LanguagesMacro.MAX_STACK)]
        private int m_MaxStack = 1;
		public virtual int MaxStack {
			get{
                if (this.m_MaxStack > 0){
                    return this.m_MaxStack;
                }
                return int.MaxValue;
            }
		}

        [Tooltip(LanguagesMacro.IS_CAN_BE_DESTORYED)]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.IS_CAN_BE_DESTORYED)]
        private bool m_CanDestroy = true;
        public bool CanDestroy {
            get { return this.m_CanDestroy; }
        }

        [Tooltip(LanguagesMacro.IS_DROPPABLE)]
		[SerializeField]
        [InspectorLabel(LanguagesMacro.IS_DROPPABLE)]
        private bool m_IsDroppable = true;
		public bool IsDroppable {
			get{ return this.m_IsDroppable; }
		}

        [Tooltip(LanguagesMacro.DROPPED_SOUND)]
		[SerializeField]
		private AudioClip m_DropSound = null;

		public AudioClip DropSound {
			get{ return this.m_DropSound; }
		}

        [Tooltip(LanguagesMacro.OVERRIDE_PERFAB_TIPS)]
        [SerializeField]
		private GameObject m_OverridePrefab=null;
		public GameObject OverridePrefab {
			get{ return this.m_OverridePrefab; }
		}

        [AcceptNull]
        [SerializeField]
        [Tooltip(LanguagesMacro.CRAFTING_RECIPE)]
        private CraftingRecipe m_CraftingRecipe = null;
        public CraftingRecipe CraftingRecipe {
            get { return this.m_CraftingRecipe; }
            set { this.m_CraftingRecipe = value; }
        }

        [AcceptNull]
        [SerializeField]
        [Tooltip(LanguagesMacro.ENCHANTING_RECIPE)]
        private CraftingRecipe m_EnchantingRecipe = null;
        public CraftingRecipe EnchantingRecipe
        {
            get { return this.m_EnchantingRecipe; }
            set { this.m_EnchantingRecipe = value; }
        }

        public ItemContainer Container
        {
            get {
                if (Slot != null) {
                    return Slot.Container;
                }
                return null;
            }
        }

		public Slot Slot {
			get{ 
                if(Slots.Count > 0)
                {
                    return this.Slots[0];
                }
                return null; 
            }
		}

        private List<Slot> m_Slots= new List<Slot>();
        public List<Slot> Slots
        {
            get { return this.m_Slots; }
            set { this.m_Slots = value; }
        }

        private List<Slot> m_ReferencedSlots = new List<Slot> ();

		public List<Slot> ReferencedSlots {
			get{ return this.m_ReferencedSlots; }
			set{ this.m_ReferencedSlots = value; }
		}

        private float m_CooldownTime = 0f;
        public float CooldownTime {
            get { return this.m_CooldownTime; }
        }

        private float m_CooldownDuration= 0f;
        public float CooldownDuration
        {
            get { return this.m_CooldownDuration; }
        }

        public bool IsInCooldown {
            get { return (this.m_CooldownDuration - (Time.time - this.m_CooldownTime)) > 0f; }
        }
        public void SetCooldown(float duration)
        {
            if (duration > 0f){
                this.m_CooldownDuration = duration;
                this.m_CooldownTime = Time.time;
            }
        }

        [SerializeField]
		private List<ObjectProperty> properties = new List<ObjectProperty> ();

        public void AddProperty(string name, object value) {
            ObjectProperty property = new ObjectProperty();
            property.Name = name;
            property.SetValue(value);
            properties.Add(property);
        }

        public void RemoveProperty(string name)
        {
            properties.RemoveAll(x => x.Name == name);
        }

        public ObjectProperty FindProperty (string name)
		{
			return properties.FirstOrDefault (property => property.Name == name);
		}

		public ObjectProperty[] GetProperties ()
		{
			return properties.ToArray ();
		}

		public void SetProperties (ObjectProperty[] properties)
		{
			this.properties = new List<ObjectProperty> (properties);
		}

        protected virtual void OnEnable ()
		{
			if (string.IsNullOrEmpty (this.m_Id)) {
				this.m_Id = System.Guid.NewGuid ().ToString ();
			}
		}

        public List<KeyValuePair<string, string>> GetPropertyInfo()
        {
            List<KeyValuePair<string, string>> propertyInfo = new List<KeyValuePair<string, string>>();
            foreach (ObjectProperty property in properties)
            {
                if (property.show)
                {
                    propertyInfo.Add(new KeyValuePair<string, string>(UnityTools.ColorString(property.Name,property.color),FormatPropertyValue(property)));
                }
            }
            return propertyInfo;
        }

        private string FormatPropertyValue(ObjectProperty property) {
            string propertyValue = string.Empty;

            if (property.SerializedType == typeof(Vector2))
            {
                propertyValue = property.vector2Value.x + "-" + property.vector2Value.y;
            }
            else if (property.SerializedType== typeof(string)) {
                propertyValue = property.stringValue;
            }
            else {
                propertyValue = ((UnityTools.IsNumeric(property.GetValue()) && System.Convert.ToSingle(property.GetValue()) > 0f) ? "+" : "-");
                propertyValue += (UnityTools.IsNumeric(property.GetValue()) ? Mathf.Abs(System.Convert.ToSingle(property.GetValue())) : property.GetValue()).ToString();
            }
            propertyValue = UnityTools.ColorString(propertyValue, property.color);
            return propertyValue;
        }

        public virtual void Use() { }
        public virtual void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", this.Name);
            data.Add("Stack", this.Stack);
            data.Add("RarityIndex", InventoryManager.Database.raritys.IndexOf(Rarity));

            if (Slot != null)
            {
                data.Add("Index", this.Slot.Index);
            }

            List<object> objectProperties = new List<object>();
            foreach (ObjectProperty property in properties)
            {
                Dictionary<string, object> propertyData = new Dictionary<string, object>();
                if (!typeof(UnityEngine.Object).IsAssignableFrom(property.SerializedType))
                {
                    propertyData.Add("Name", property.Name);
                    propertyData.Add("Value", property.GetValue());
                    objectProperties.Add(propertyData);
                }
            }
            data.Add("Properties",objectProperties);

            if (Slots.Count > 0)
            {
                List<object> slots = new List<object>();
                for (int i = 0; i < Slots.Count; i++)
                {
                    slots.Add(Slots[i].Index);
                }
                data.Add("Slots", slots);
            }

            if (ReferencedSlots.Count > 0)
            {
                List<object> references = new List<object>();
                for (int i = 0; i < ReferencedSlots.Count; i++)
                {
                    Dictionary<string, object> referenceData = new Dictionary<string, object>();
                    referenceData.Add("Container", ReferencedSlots[i].Container.Name);
                    referenceData.Add("Slot", ReferencedSlots[i].Index);
                    references.Add(referenceData);
                }
                data.Add("Reference", references);
            }
        }

        public virtual void SetObjectData(Dictionary<string, object> data)
        {
            this.Stack = System.Convert.ToInt32(data["Stack"]);
            if (data.ContainsKey("RarityIndex")) {
                int rarityIndex =System.Convert.ToInt32(data["RarityIndex"]);
                if (rarityIndex > -1 && rarityIndex < InventoryManager.Database.raritys.Count) {
                    this.m_Rarity = InventoryManager.Database.raritys[rarityIndex];
                }
            }

            if (data.ContainsKey("Properties"))
            {
                List<object> objectProperties = data["Properties"] as List<object>;
                for (int i = 0; i < objectProperties.Count; i++)
                {
                    Dictionary<string, object> propertyData = objectProperties[i] as Dictionary<string, object>;
                    string propertyName = (string)propertyData["Name"];
                    object propertyValue = propertyData["Value"];
                    ObjectProperty property = FindProperty(propertyName);
                    if (property == null) {
                        property = new ObjectProperty();
                        property.Name = propertyName;
                        properties.Add(property);
                    }
                    property.SetValue(propertyValue);
                }
            }

            if (data.ContainsKey("Reference"))
            {
                List<object> references = data["Reference"] as List<object>;
                for (int i = 0; i < references.Count; i++)
                {
                    Dictionary<string, object> referenceData = references[i] as Dictionary<string, object>;
                    string container = (string)referenceData["Container"];
                    int slot = System.Convert.ToInt32(referenceData["Slot"]);
                    ItemContainer referenceContainer = WidgetUtility.Find<ItemContainer>(container);
                    if (referenceContainer != null)
                    {
                        referenceContainer.ReplaceItem(slot, this);
                    }
                }
            }
        }
    }
}