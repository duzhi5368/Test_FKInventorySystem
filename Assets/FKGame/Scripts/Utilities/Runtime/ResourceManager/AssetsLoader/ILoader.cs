using System;
using System.Collections;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class ILoaderBase
    {
        protected AssetsLoadController loadAssetsController;
        public ILoaderBase(AssetsLoadController loadAssetsController)
        {
            this.loadAssetsController = loadAssetsController;
        }
        public abstract IEnumerator LoadAssetsIEnumerator(string path, Type resType, CallBack<AssetsData> callBack);
        public abstract AssetsData LoadAssets(string path);
        public abstract AssetsData LoadAssets<T>(string path) where T : UnityEngine.Object;
        public virtual string[] GetAllDependenciesName(string name) { return new string[0]; }

        // 判断资源是否含有依赖
        public virtual bool IsHaveDependencies(string name) { return false; }
    }
}