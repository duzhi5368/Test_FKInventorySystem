using System;
using System.Collections;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ResourcesLoader : ILoaderBase
    {
        public ResourcesLoader(AssetsLoadController loadAssetsController) : base(loadAssetsController){}

        public override IEnumerator LoadAssetsIEnumerator(string path, Type resType, CallBack<AssetsData> callBack)
        {
            AssetsData rds = null;
            string s = PathUtils.RemoveExtension(path);
            ResourceRequest ass = null;
            if (resType != null)
            {
                ass = Resources.LoadAsync(s, resType);
            }
            else
            {
                ass = Resources.LoadAsync(s);
            }
            yield return ass;

            if (ass.asset != null)
            {
                rds = new AssetsData(path);
                rds.Assets = new UnityEngine.Object[] { ass.asset };
            }
            else
            {
                Debug.LogError("º”‘ÿ ß∞‹,Path:" + path);
            }

            if (callBack != null)
                callBack(rds);
            yield return new WaitForEndOfFrame();
        }
        public override AssetsData LoadAssets(string path)
        {
            string s = PathUtils.RemoveExtension(path);
            AssetsData rds = null;
            UnityEngine.Object ass = Resources.Load(s);
            if (ass != null)
            {
                rds = new AssetsData(path);
                rds.Assets = new UnityEngine.Object[] { ass };
            }
            else
            {
                Debug.LogError("º”‘ÿ ß∞‹,Path:" + path);
            }
            return rds;
        }

        public override AssetsData LoadAssets<T>(string path)
        {
            string s = PathUtils.RemoveExtension(path);
            AssetsData rds = null;
            T ass = Resources.Load<T>(s);
            if (ass != null)
            {
                rds = new AssetsData(path);
                rds.Assets = new UnityEngine.Object[] { ass };
            }
            else
            {
                Debug.LogError("º”‘ÿ ß∞‹,Path:" + path);
            }
            return rds;
        }
    }
}