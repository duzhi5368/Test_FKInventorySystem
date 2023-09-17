using System.Collections.Generic;
using System.IO;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 绘制文件树状目录
    public class FolderTreeView : TreeView
    {
        public CallBack<FolderTreeViewItem> selectCallBack;             // 选择Item回调
        public CallBack<FolderTreeViewItem> dblclickItemCallBack;       // 双击Item回调

        private static Texture2D folderIcon = EditorGUIUtility.FindTexture("Folder Icon");
        private static Texture2D fileIcon = EditorGUIUtility.FindTexture("TextAsset Icon");
        private bool userSearch = true;
        private int index;
        private List<string> allPath = null;
        private SearchField m_SearchField;

        public FolderTreeView(TreeViewState state)
                : base(state)
        {
            index = 1;
            searchString = "";
            rowHeight = 20;
            showBorder = true;
            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += SetFocusAndEnsureSelectedItem;
        }

        // 是否开启使用搜索栏
        public bool UserSearch
        {
            get
            {
                return userSearch;
            }
            set
            {
                userSearch = value;
            }
        }

        // 返回root节点
        public FolderTreeViewItem RootItem
        {
            get { return (FolderTreeViewItem)rootItem; }
        }

        // 设置数据
        public void SetData(List<string> datas)
        {
            allPath = datas;
            Reload();
        }

        private List<FolderTreeViewItem> GetFolderRows()
        {
            List<FolderTreeViewItem> list = new List<FolderTreeViewItem>();
            var rows = GetRows();
            if (rows != null)
            {
                foreach (var item in GetRows())
                {
                    list.Add((FolderTreeViewItem)item);
                }
            }
            return list;
        }

        protected override TreeViewItem BuildRoot()
        {
            FolderTreeViewItem root = new FolderTreeViewItem { id = 0, depth = -1, displayName = "Root" };
            root.children = new List<TreeViewItem>();
            var rows = GetFolderRows();
            if (allPath == null)
                return root;
            foreach (var p in allPath)
            {
                string[] items = p.Split('/');
                string fullPath = "";
                for (int i = 0; i < items.Length; i++)
                {
                    string displayName = items[i];
                    if (string.IsNullOrEmpty(fullPath))
                        fullPath = displayName;
                    else
                        fullPath += "/" + displayName;

                    bool isDirectory = !(i == (items.Length - 1));
                    bool isHaveItem = false;
                    foreach (var item in rows)
                    {
                        if (item.fullPath == fullPath && item.isDirectory == isDirectory)
                        {
                            isHaveItem = true;
                            break;
                        }
                    }
                    if (!isHaveItem)
                    {
                        FolderTreeViewItem temp = new FolderTreeViewItem(index, i, displayName, fullPath, isDirectory);
                        string dir = Path.GetDirectoryName(fullPath);
                        FolderTreeViewItem parent = null;
                        foreach (var item in rows)
                        {
                            if (item.fullPath == dir && item.isDirectory)
                            {
                                parent = item;
                                break;
                            }
                        }
                        if (parent != null)
                            parent.AddChild(temp);
                        else
                            root.AddChild(temp);
                        rows.Add(temp);
                        index++;
                    }
                }
            }
            List<TreeViewItem> list = new List<TreeViewItem>();
            foreach (var item in rows)
            {
                list.Add(item);
            }
            //SetupParentsAndChildrenFromDepths(root, list);
            foreach (var item in rows)
            {
                if (item.hasChildren)
                    item.icon = folderIcon;
                else
                    item.icon = fileIcon;
            }
            return root;
        }


        public override void OnGUI(Rect rect)
        {
            if (!userSearch)
                base.OnGUI(rect);
            else
            {
                Rect searchRect = new Rect(rect.x, rect.y, rect.width, EditorStyles.toolbar.fixedHeight);
                GUI.Box(searchRect, "", EditorStyles.toolbar);
                searchRect = new Rect(rect.x + rect.width / 2, rect.y + 3, rect.width / 2, EditorStyles.toolbar.fixedHeight);
                searchString = m_SearchField.OnToolbarGUI(searchRect, searchString);
                rect = new Rect(rect.x, rect.y + searchRect.height, rect.width, rect.height - searchRect.height);
                base.OnGUI(rect);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && args.rowRect.Contains(e.mousePosition) && selectCallBack != null)
            {
                selectCallBack((FolderTreeViewItem)args.item);

            }
            if (e.type == EventType.MouseDown && args.rowRect.Contains(e.mousePosition) && e.clickCount == 2 && dblclickItemCallBack != null)
            {
                dblclickItemCallBack((FolderTreeViewItem)args.item);
            }
            base.RowGUI(args);
        }
    }
}