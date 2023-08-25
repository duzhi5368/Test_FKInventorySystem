﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System;
using FKGame.Macro;
//------------------------------------------------------------------------
// Tools -> FKGame -> 属性系统 -> 编辑器 面板上方的"属性"Tab
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
	[System.Serializable]
	public class StatCollectionEditor : ScriptableObjectCollectionEditor<Stat>
	{
		[SerializeField]
		protected List<string> searchFilters;
		[SerializeField]
		protected string searchFilter = "All";

		public override string ToolbarName
		{
			get
			{
				return LanguagesMacro.STAT;
			}
		}

		public StatCollectionEditor(UnityEngine.Object target, List<Stat> items, List<string> searchFilters) : base(target, items)
		{
			this.target = target;
			this.items = items;

			this.searchFilters = searchFilters;
			this.searchFilters.Insert(0, "All");
            this.m_SearchString = "All";
		}

		// 点击【创建】时，遍历stat system中的全部继承Stat的类
		protected override void Create()
		{
			Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Stat).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();
			if (types.Length > 1)
			{
				GenericMenu menu = new GenericMenu();
				foreach (Type type in types)
				{
					Type mType = type;
					menu.AddItem(new GUIContent(mType.Name), false, delegate () {
						CreateItem(mType);
					});
				}
				menu.ShowAsContext();
			}
			else
			{
				CreateItem(types[0]);
			}
		}

		// 进行搜索
		protected override void DoSearchGUI()
		{
			string[] searchResult = EditorTools.SearchField(m_SearchString, searchFilter, searchFilters);
			searchFilter = searchResult[0];
			m_SearchString = string.IsNullOrEmpty(searchResult[1]) ? searchFilter : searchResult[1];
		}

		protected override bool MatchesSearch(Stat item, string search)
		{
			return (item.Name.ToLower().Contains(search.ToLower()) || m_SearchString == searchFilter || search.ToLower() == item.GetType().Name.ToLower());
		}

		protected override string HasConfigurationErrors(Stat item)
		{
			if (string.IsNullOrEmpty(item.Name))
				return LanguagesMacro.EMPTY_NAME;
            if (Items.Any(x => !x.Equals(item) && x.Name == item.Name))
				return LanguagesMacro.DUPLICATE_NAME;
			return string.Empty;
		}

		protected override void Duplicate(Stat item)
		{
			Stat duplicate = (Stat)ScriptableObject.Instantiate(item);
			//duplicate.Id = System.Guid.NewGuid().ToString();
			duplicate.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.AddObjectToAsset(duplicate, target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Items.Add(duplicate);
			EditorUtility.SetDirty(target);
			Select(duplicate);
		}

		protected override void CreateItem(Type type)
		{
			Stat item = (Stat)ScriptableObject.CreateInstance(type);
			item.hideFlags = HideFlags.HideInHierarchy;
			StatDatabase database = target as StatDatabase;
			AssetDatabase.AddObjectToAsset(item, target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Items.Add(item);
			Select(item);

			EditorUtility.SetDirty(target);
		}

		protected override void DrawItemLabel(int index, Stat item)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(item.Name, Styles.selectButtonText);
			GUILayout.EndHorizontal();
		}
	}
}