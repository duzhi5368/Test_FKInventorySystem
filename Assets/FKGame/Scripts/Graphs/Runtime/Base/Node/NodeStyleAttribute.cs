﻿using System;
//------------------------------------------------------------------------
// 图 中节点的属性
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeStyleAttribute : Attribute
    {
        public readonly string iconPath;
        public readonly bool displayHeader;
        public readonly string category;
        public NodeStyleAttribute(bool displayHeader) : this(string.Empty, displayHeader, string.Empty){}
        public NodeStyleAttribute(bool displayHeader, string category) : this(string.Empty, displayHeader, category){}
        public NodeStyleAttribute(string iconPath):this(iconPath, true, string.Empty) {}
        public NodeStyleAttribute(string iconPath, bool displayHeader, string category)
        {
            this.iconPath = iconPath;
            this.displayHeader = displayHeader;
            this.category = category;
        }
    }
}