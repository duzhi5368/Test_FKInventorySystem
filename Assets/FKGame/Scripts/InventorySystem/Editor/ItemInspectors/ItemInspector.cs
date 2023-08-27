using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using UnityEditorInternal;
using UnityEngine.Events;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
	[CustomEditor (typeof(Item), true)]
	public class ItemInspector :  Editor
	{
        protected SerializedProperty m_ItemName;
        protected SerializedProperty m_UseItemNameAsDisplayName;
        protected AnimBool m_ShowItemDisplayNameOptions;
        protected SerializedProperty m_ItemDisplayName;
        protected SerializedProperty m_Icon;
        protected SerializedProperty m_Prefab;
        protected SerializedProperty m_Description;
        protected SerializedProperty m_Category;
        protected SerializedProperty m_BuyPrice;
        protected SerializedProperty m_BuyCurrency;
        protected SerializedProperty m_SellPrice;
        protected SerializedProperty m_SellCurrency;
        protected SerializedProperty m_Stack;
        protected SerializedProperty m_MaxStack;
        protected SerializedProperty m_IsDroppable;
        protected SerializedProperty m_DropSound;
        protected SerializedProperty m_OverridePrefab;
        protected SerializedProperty m_Properties;
        protected SerializedProperty m_IsSellable;
        protected SerializedProperty m_CanBuyBack;
        protected SerializedProperty m_CanDestroy;
        protected SerializedProperty m_CraftingRecipe;
        protected SerializedProperty m_EnchantingRecipe;

        protected AnimBool m_ShowSellOptions;
        protected AnimBool m_ShowDropOptions;
        protected AnimBool m_ShowCraftOptions;

        protected ReorderableList m_PropertyList;
        protected static List<ObjectProperty> copy = new List<ObjectProperty> ();

        protected SerializedProperty m_Script;

        private Dictionary<Type, string[]> m_ClassProperties;
        protected string[] m_PropertiesToExcludeForChildClasses;
        protected List<System.Action> m_DrawInspectors;
        protected string m_NameError;

        protected virtual void OnEnable ()
		{
            if (target == null)
                return;

            this.m_DrawInspectors = new List<System.Action>();
            m_ClassProperties = new Dictionary<Type, string[]>();

            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_ItemName = serializedObject.FindProperty("m_ItemName");
            this.m_ItemDisplayName = serializedObject.FindProperty("m_DisplayName");
            this.m_UseItemNameAsDisplayName = serializedObject.FindProperty("m_UseItemNameAsDisplayName");
            this.m_ShowItemDisplayNameOptions = new AnimBool(!this.m_UseItemNameAsDisplayName.boolValue);
            this.m_ShowItemDisplayNameOptions.valueChanged.AddListener(new UnityAction(Repaint));

            this.m_Icon = serializedObject.FindProperty("m_Icon");
            this.m_Prefab = serializedObject.FindProperty("m_Prefab");
            this.m_Description = serializedObject.FindProperty("m_Description");
            this.m_Category = serializedObject.FindProperty("m_Category");

            this.m_IsSellable = serializedObject.FindProperty("m_IsSellable");
            this.m_ShowSellOptions = new AnimBool(this.m_IsSellable.boolValue);
            this.m_ShowSellOptions.valueChanged.AddListener(new UnityAction(Repaint));
            this.m_CanBuyBack = serializedObject.FindProperty("m_CanBuyBack");
            this.m_BuyPrice = serializedObject.FindProperty("m_BuyPrice");
            this.m_BuyCurrency = serializedObject.FindProperty("m_BuyCurrency");
            this.m_SellPrice = serializedObject.FindProperty("m_SellPrice");
            this.m_SellCurrency = serializedObject.FindProperty("m_SellCurrency");

            this.m_Stack = serializedObject.FindProperty("m_Stack");
            this.m_MaxStack = serializedObject.FindProperty("m_MaxStack");
            this.m_CanDestroy = serializedObject.FindProperty("m_CanDestroy");

            this.m_IsDroppable = serializedObject.FindProperty("m_IsDroppable");
            this.m_DropSound = serializedObject.FindProperty("m_DropSound");
            this.m_OverridePrefab = serializedObject.FindProperty("m_OverridePrefab");
            this.m_IsDroppable = serializedObject.FindProperty("m_IsDroppable");
            this.m_ShowDropOptions = new AnimBool(this.m_IsDroppable.boolValue);
            this.m_ShowDropOptions.valueChanged.AddListener(new UnityAction(Repaint));

            this.m_Properties = serializedObject.FindProperty("properties");

            this.m_PropertyList = new ReorderableList (serializedObject,this.m_Properties, true, true, true, true);
			this.m_PropertyList.elementHeight = (EditorGUIUtility.singleLineHeight + 4f) * 3;
			this.m_PropertyList.drawHeaderCallback = (Rect rect) => {  
				EditorGUI.LabelField (rect, LanguagesMacro.ITEM_PROPERTIES);
				Event ev = Event.current;
				if (ev.type == EventType.MouseDown && ev.button == 1 && rect.Contains (ev.mousePosition)) {
					GenericMenu menu = new GenericMenu ();
					menu.AddItem (new GUIContent (LanguagesMacro.COPY), false, delegate {
						ObjectProperty[] properties = (target as Item).GetProperties ();
						foreach (ObjectProperty property in properties) {
							ObjectProperty clone = new ObjectProperty ();
							FieldInfo[] fields = typeof(ObjectProperty).GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							foreach (FieldInfo field in fields) {
								field.SetValue (clone, field.GetValue (property));
							}
							copy.Add (clone);
						}
					});
					if (copy != null && copy.Count > 0) {
						menu.AddItem (new GUIContent ("Paste"), false, delegate {
							(target as Item).SetProperties (copy.ToArray ());
						});
					} else {
						menu.AddDisabledItem (new GUIContent ("Paste"));
					}
					menu.ShowAsContext ();
				}
			};
			
			this.m_PropertyList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				Rect mRect = new Rect (rect);
				var element = m_PropertyList.serializedProperty.GetArrayElementAtIndex (index);
				rect.y += 2;
				rect.height = EditorGUIUtility.singleLineHeight;
				rect.width -= 17;
				EditorGUI.PropertyField (rect, element.FindPropertyRelative ("name"));
				rect.x += rect.width + 2;
				rect.y -= 2f;
				EditorGUI.PropertyField (rect, element.FindPropertyRelative ("show"), GUIContent.none);
				rect.y += 2f;
				rect.x -= rect.width + 2;
				rect.width += 17;
				rect.y += EditorGUIUtility.singleLineHeight + 2;
				float width = rect.width;
				rect.width = EditorGUIUtility.labelWidth - 2f;//148f;
				SerializedProperty typeIndex = element.FindPropertyRelative ("typeIndex");
				typeIndex.intValue = EditorGUI.Popup (rect, typeIndex.intValue, ObjectProperty.DisplayNames);
				rect.x += rect.width + 2f;
				rect.width = width - EditorGUIUtility.labelWidth;
		
				EditorGUI.PropertyField (rect, element.FindPropertyRelative (ObjectProperty.GetPropertyName (ObjectProperty.SupportedTypes [typeIndex.intValue])), GUIContent.none);
				rect.y += EditorGUIUtility.singleLineHeight + 2;
				rect.x = mRect.x;
				rect.width = mRect.width;
				EditorGUI.PropertyField (rect, element.FindPropertyRelative ("color"));
			};

            this.m_CraftingRecipe = serializedObject.FindProperty("m_CraftingRecipe");
            this.m_EnchantingRecipe = serializedObject.FindProperty("m_EnchantingRecipe");

            List<string> propertiesToExclude = new List<string>() {
                this.m_Script.propertyPath,
                this.m_ItemName.propertyPath,
                this.m_ItemDisplayName.propertyPath,
                this.m_UseItemNameAsDisplayName.propertyPath,
                this.m_Icon.propertyPath,
                this.m_Prefab.propertyPath,
                this.m_Description.propertyPath,
                this.m_Category.propertyPath,
                this.m_BuyPrice.propertyPath,
                this.m_BuyCurrency.propertyPath,
                this.m_SellPrice.propertyPath,
                this.m_SellCurrency.propertyPath,
                this.m_Stack.propertyPath,
                this.m_MaxStack.propertyPath,
                this.m_IsDroppable.propertyPath,
                this.m_DropSound.propertyPath,
                this.m_OverridePrefab.propertyPath,
                this.m_Properties.propertyPath,
                this.m_IsSellable.propertyPath,
                this.m_CanBuyBack.propertyPath,
                this.m_CanDestroy.propertyPath,
                this.m_CraftingRecipe.propertyPath,
                this.m_EnchantingRecipe.propertyPath
            };

            Type[] subInspectors = Utility.BaseTypesAndSelf(GetType()).Where(x => x.IsSubclassOf(typeof(ItemInspector))).ToArray();
            Array.Reverse(subInspectors);
            for (int i = 0; i < subInspectors.Length; i++)
            {
                MethodInfo method = subInspectors[i].GetMethod("DrawInspector", BindingFlags.NonPublic | BindingFlags.Instance);
                Type inspectedType = typeof(CustomEditor).GetField("m_InspectedType", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(subInspectors[i].GetCustomAttribute<CustomEditor>()) as Type;
                FieldInfo[] fields = inspectedType.GetAllSerializedFields().Where(x => !x.HasAttribute(typeof(HideInInspector))).ToArray();
                string[] classProperties = fields.Where(x => x.DeclaringType == inspectedType).Select(x => x.Name).ToArray();
                if (!this.m_ClassProperties.ContainsKey(inspectedType))
                {
                    this.m_ClassProperties.Add(inspectedType, classProperties);
                }
                propertiesToExclude.AddRange(classProperties);
                if (method != null)
                {
                    m_DrawInspectors.Add(delegate { method.Invoke(this, null); });
                }
                else
                {
                    m_DrawInspectors.Add(delegate () {
                        for (int j = 0; j < classProperties.Length; j++)
                        {
                            SerializedProperty property = serializedObject.FindProperty(classProperties[j]);
                            EditorGUILayout.PropertyField(property);
                        }
                    });
                }
            }
            this.m_PropertiesToExcludeForChildClasses = propertiesToExclude.ToArray();
        }

        protected virtual void OnDisable() { }

        public override void OnInspectorGUI()
        {
            ScriptGUI();
            serializedObject.Update();
            DrawBaseInspector();
            for (int i = 0; i < m_DrawInspectors.Count; i++)
            {
               this.m_DrawInspectors[i].Invoke();
            }
            DrawPropertiesExcluding(serializedObject, this.m_PropertiesToExcludeForChildClasses);
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawBaseInspector() {
            EditorGUILayout.PropertyField(this.m_ItemName, new GUIContent("Name"));
            EditorGUILayout.PropertyField(this.m_UseItemNameAsDisplayName, new GUIContent("Use name as display name"));
            this.m_ShowItemDisplayNameOptions.target = !this.m_UseItemNameAsDisplayName.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowItemDisplayNameOptions.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_ItemDisplayName);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(this.m_Icon);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(this.m_Prefab);
            SetupPrefab(this.m_Prefab);
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(this.m_Description);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(LanguagesMacro.ITEM_PROPERTIES, EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(LanguagesMacro.ITEM_PROPERTIES_TIPS, MessageType.Info);
            this.m_PropertyList.elementHeight = this.m_PropertyList.count == 0 ? (EditorGUIUtility.singleLineHeight + 4f) : (EditorGUIUtility.singleLineHeight + 4f) * 3;
            this.m_PropertyList.DoLayoutList();

            EditorGUILayout.PropertyField(this.m_Category);
            EditorGUILayout.PropertyField(this.m_IsSellable);
            this.m_ShowSellOptions.target = this.m_IsSellable.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowSellOptions.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_CanBuyBack);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.m_BuyPrice);
                EditorGUILayout.PropertyField(this.m_BuyCurrency, GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.m_SellPrice);
                EditorGUILayout.PropertyField(this.m_SellCurrency, GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(this.m_Stack);
            if (this.m_MaxStack.intValue == 0)
            {
                EditorGUILayout.HelpBox("Maximum stack of 0 ~ unlimited", MessageType.Info);
            }
            EditorGUILayout.PropertyField(this.m_MaxStack);
            EditorGUILayout.PropertyField(this.m_CanDestroy);

            EditorGUILayout.PropertyField(this.m_IsDroppable);
            this.m_ShowDropOptions.target = this.m_IsDroppable.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowDropOptions.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(this.m_OverridePrefab);
                EditorGUILayout.PropertyField(this.m_DropSound);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(this.m_CraftingRecipe);
            EditorGUILayout.PropertyField(this.m_EnchantingRecipe);
        }

        protected virtual void DrawBuySellGUI() {
            EditorGUILayout.PropertyField(this.m_IsSellable);
            this.m_ShowSellOptions.target = this.m_IsSellable.boolValue;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowSellOptions.faded))
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.m_BuyPrice);
                EditorGUILayout.PropertyField(this.m_BuyCurrency, GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.m_SellPrice);
                EditorGUILayout.PropertyField(this.m_SellCurrency, GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUILayout.EndFadeGroup();
        }

        protected void ScriptGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();
        }

        private void SetupPrefab(SerializedProperty prefabProperty)
        {
            if (prefabProperty.objectReferenceValue != null)
            {
                GameObject mPrefab = prefabProperty.objectReferenceValue as GameObject;
                if (mPrefab.GetComponent<Trigger>() == null ||
                   mPrefab.GetComponent<Collider>() == null ||
                   mPrefab.GetComponent<Rigidbody>() == null)
                {
                    Color color = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Setup", GUILayout.Width(70)))
                    {
                        GameObject prefab = (GameObject)Instantiate(mPrefab);
                        if (prefab.GetComponent<Trigger>() == null)
                        {
                            // Trigger trigger = prefab.AddComponent<Trigger>();
                        }
                        if (prefab.GetComponent<ItemCollection>() == null) {
                            ItemCollection collection=prefab.AddComponent<ItemCollection>();
                            collection.Add((Item)target);
                        }
                        if (prefab.GetComponent<Collider>() == null)
                        {
                            MeshCollider collider = prefab.AddComponent<MeshCollider>();
                            collider.convex = true;
                        }
                        if (prefab.GetComponent<Rigidbody>() == null)
                        {
                            prefab.AddComponent<Rigidbody>();
                        }

                        string mPath = EditorUtility.SaveFilePanelInProject(
                                           "Create Prefab" + prefab.name,
                                           "New " + prefab.name + ".prefab",
                                           "prefab", "");
                        if (!string.IsNullOrEmpty(mPath))
                        {
                            GameObject mGameObject = PrefabUtility.SaveAsPrefabAsset(prefab, mPath);
                            AssetDatabase.SaveAssets();
                            prefabProperty.objectReferenceValue = mGameObject;
                        }
                        DestroyImmediate(prefab);
                    }
                    GUI.backgroundColor = color;
                }
            }
        }
    }
}