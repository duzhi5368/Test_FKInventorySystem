using FKGame.Macro;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [System.Serializable]
    public class InventorySystemInspector
    {
        private ItemDatabase m_Database;
        private List<ICollectionEditor> m_ChildEditors;

        [SerializeField]
        private int toolbarIndex;

        private string[] toolbarNames
        {
            get
            {
                string[] items = new string[m_ChildEditors.Count];
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    items[i] = m_ChildEditors[i].ToolbarName;
                }
                return items;
            }
        }

        public void OnEnable()
        {
            this.m_Database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(EditorPrefs.GetString("ItemDatabasePath"));
            if (this.m_Database == null) {
                string[] guids = AssetDatabase.FindAssets("t:" + typeof(ItemDatabase).FullName);
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    this.m_Database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(path);
                }
            }
            toolbarIndex = EditorPrefs.GetInt("InventoryToolbarIndex");
            ResetChildEditors();
        }

        public void OnDisable()
        {
            if (this.m_Database != null) {
                EditorPrefs.SetString("ItemDatabasePath",AssetDatabase.GetAssetPath(this.m_Database));
            }
            EditorPrefs.SetInt("InventoryToolbarIndex",toolbarIndex);

            if (m_ChildEditors != null)
            {
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    m_ChildEditors[i].OnDisable();
                }
            }
        }

        public void OnDestroy()
        {
            if (m_ChildEditors != null)
            {
                for (int i = 0; i < m_ChildEditors.Count; i++)
                {
                    m_ChildEditors[i].OnDestroy();
                }
            }
        }

        public void OnGUI(Rect position)
        {
            DoToolbar();

            if (m_ChildEditors != null)
            {
                this.m_Database.RemoveNullReferences();
                m_ChildEditors[toolbarIndex].OnGUI(new Rect(0f, 20f, position.width, position.height - 20f));
            }
        }

        private void DoToolbar() {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();

            SelectDatabaseButton();
            GUILayout.Space(2f);
            if (this.m_ChildEditors != null)
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarNames,EditorStyles.toolbarButton, GUILayout.MinWidth(200));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void SelectDatabaseButton() {
            GUIStyle buttonStyle = EditorStyles.objectField;
            GUIContent buttonContent = new GUIContent(this.m_Database != null ? this.m_Database.name : "Null");
            Rect buttonRect = GUILayoutUtility.GetRect(180f,18f);
            buttonRect.y += 1f;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                ObjectPickerWindow.ShowWindow(buttonRect, typeof(ItemDatabase), 
                    (UnityEngine.Object obj)=> { 
                        this.m_Database = obj as ItemDatabase;
                        ResetChildEditors();
                    }, 
                    ()=> {
                        ItemDatabase db = EditorTools.CreateAsset<ItemDatabase>(true);
                        if (db != null)
                        {
                            CreateDefaultCategory(db);
                            this.m_Database = db;
                            ResetChildEditors();
                        }
                    });
            }
        }

        private static void CreateDefaultCategory(ItemDatabase database)
        {
            Category category = ScriptableObject.CreateInstance<Category>();
            category.Name = "None";
            category.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(category, database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            database.categories.Add(category);
            EditorUtility.SetDirty(database);
        }

        private void ResetChildEditors() 
        {
            if (this.m_Database == null)
            {
                return;
            }
            this.m_Database.RemoveNullReferences();
            EditorUtility.SetDirty(this.m_Database);

            m_ChildEditors = new List<ICollectionEditor>();
            m_ChildEditors.Add(new ItemCollectionEditor(this.m_Database, this.m_Database.items, this.m_Database.categories.Select(x => x.Name).ToList()));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<Currency>(LanguagesMacro.CURRENCY, this.m_Database, this.m_Database.currencies));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<CraftingRecipe>(LanguagesMacro.CRAFTING_RECIPE, this.m_Database, this.m_Database.craftingRecipes));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<Rarity>(LanguagesMacro.RARITY, this.m_Database, this.m_Database.raritys));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<Category>(LanguagesMacro.CATEGORY, this.m_Database, this.m_Database.categories));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<EquipmentRegion>(LanguagesMacro.EQUIP_REGION, this.m_Database, this.m_Database.equipments));
            m_ChildEditors.Add(new ScriptableObjectCollectionEditor<ItemGroup>(LanguagesMacro.ITEM_GROUP, this.m_Database, this.m_Database.itemGroups));

            m_ChildEditors.Add(new Configuration.ItemSettingsEditor(this.m_Database, this.m_Database.settings));

            for (int i = 0; i < this.m_ChildEditors.Count; i++)
            {
                this.m_ChildEditors[i].OnEnable();
            }
        }
    }
}