using UnityEngine;
//------------------------------------------------------------------------
// 挂载到GameObj上，这个类就不会在切换场景时进行销毁
//------------------------------------------------------------------------
namespace FKGame
{
	public class ComponentDontDestroyOnLoad : MonoBehaviour {
		private void Awake(){
			DontDestroyOnLoad (gameObject);
		}
	}
}