using UnityEditor.IMGUI.Controls;
//------------------------------------------------------------------------
namespace FKGame
{
    public class FolderTreeViewItem : TreeViewItem
    {
        private string m_fullPath = "";
        private bool m_isDirectory;

        public FolderTreeViewItem(int id, int depth, string displayName, string fullPath, bool isDirectory) : base(id, depth, displayName)
        {
            m_fullPath = fullPath;
            m_isDirectory = isDirectory;
        }
        public FolderTreeViewItem(int id, int depth, string displayName) : base(id, depth, displayName)
        {
        }
        public FolderTreeViewItem(int id, int depth) : base(id, depth)
        {
        }
        public FolderTreeViewItem(int id) : base(id)
        {
        }
        public FolderTreeViewItem() : base()
        {
        }

        public string fullPath
        {
            get
            {
                return m_fullPath;
            }

            set
            {
                m_fullPath = value;
            }
        }

        public bool isDirectory
        {
            get
            {
                return m_isDirectory;
            }

            set
            {
                m_isDirectory = value;
            }
        }
    }
}