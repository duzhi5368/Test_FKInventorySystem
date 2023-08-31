﻿using System.Collections.Generic;
using System;
using System.Linq;
using FKGame.Macro;
//------------------------------------------------------------------------
// Tools -> FKGame -> 属性系统 -> 编辑器 面板上方的"设置"Tab
//------------------------------------------------------------------------
namespace FKGame.StatSystem.Configuration
{
    [System.Serializable]
    public class StatSettingsEditor : ScriptableObjectCollectionEditor<Settings>
    {
        public override string ToolbarName
        {
            get
            {
                return LanguagesMacro.SETTING;
            }
        }

        protected override bool CanAdd => false;
        protected override bool CanRemove => false;
        protected override bool CanDuplicate => false;

        public StatSettingsEditor(UnityEngine.Object target, List<Settings> items) : base(target, items)
        {
            this.target = target;
            this.items = items;
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Settings).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                if (Items.Where(x => x.GetType() == type).FirstOrDefault() == null)
                {
                    CreateItem(type);
                }
            }
        }

        protected override bool MatchesSearch(Settings item, string search)
        {
            return (item.Name.ToLower().Contains(search.ToLower()) || search.ToLower() == item.GetType().Name.ToLower());
        }

        protected override string ButtonLabel(int index, Settings item)
        {
            return "  " + GetSidebarLabel(item);
        }

    }
}