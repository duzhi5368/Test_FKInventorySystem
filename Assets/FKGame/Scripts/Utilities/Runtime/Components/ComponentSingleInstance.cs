using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FKGame.Macro;
//------------------------------------------------------------------------
// 单例对象，直接拖到对象上作为组件即可
//------------------------------------------------------------------------
namespace FKGame
{
    public class ComponentSingleInstance : MonoBehaviour
    {
        private static Dictionary<string, GameObject> m_Instances = new Dictionary<string, GameObject>();

        void Awake()
        {
            GameObject instance = null;
            ComponentSingleInstance.m_Instances.TryGetValue(this.name, out instance);
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                ComponentSingleInstance.m_Instances[this.name] = gameObject;
            }
            else
            {
                Debug.Log(LanguagesMacro.MULTIPLE_INSTANCE + gameObject.name);
                DestroyImmediate(gameObject);
            }
        }

        public static List<GameObject> GetInstanceObjects() {
            return m_Instances.Values.Where(x=>x != null).ToList();
        }
    }
}