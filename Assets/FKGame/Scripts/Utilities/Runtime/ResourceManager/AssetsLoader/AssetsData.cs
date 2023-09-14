using System.IO;
using UnityEngine.Profiling;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ������Դ��Ϣ������
    public class AssetsData
    {
        public AssetsData(string path)
        {
            assetPath = path;
            assetName = Path.GetFileNameWithoutExtension(assetPath);
            objectsSize = -1;
            bundleSize = -1;
        }

        public int refCount = 0;            // ��Դ���ô���
        public string assetName = "";
        public string assetPath;            // ��Դ·��
        private Object[] assets;
        private AssetBundle assetBundle;
        private long objectsSize = -1;
        private long bundleSize = -1;

        public Object[] Assets
        {
            get { return assets; }
            set
            {
                assets = value;
                objectsSize = 0;
                if (assets != null)
                {

                    foreach (var item in assets)
                    {
                        objectsSize += Profiler.GetRuntimeMemorySizeLong(item);
                    }
                }
            }
        }
        public T GetAssets<T>() where T : Object
        {
            foreach (var item in assets)
            {
                if (item.GetType() == typeof(T))
                    return (T)item;
            }
            return default(T);
        }

        public AssetBundle AssetBundle
        {
            get
            {
                return assetBundle;
            }
            set
            {
                assetBundle = value;
                bundleSize = 0;
                if (assetBundle)
                {
                    bundleSize = Profiler.GetRuntimeMemorySizeLong(assetBundle);
                }
            }
        }

        // ��ȡ��Դ��ռ���ڴ�Ĵ�С
        public long GetObjectsMemorySize()
        {
            return objectsSize;
        }
        public long GetBundleMemorySize()
        {
            return bundleSize;
        }
        public long GetTotalMemorySize()
        {
            return GetObjectsMemorySize() + GetBundleMemorySize();
        }
    }
}