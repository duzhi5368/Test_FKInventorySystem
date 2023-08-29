using UnityEngine;
//------------------------------------------------------------------------
// 使用方法：GameObject.AddComponent<DontDestroyOnLoad>(); 这个类就不会在切换场景时进行销毁
//------------------------------------------------------------------------
namespace FKGame
{
	public class DontDestroyOnLoad : MonoBehaviour {
		private void Awake(){
			DontDestroyOnLoad (gameObject);
		}
	}
}