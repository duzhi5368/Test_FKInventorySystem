using UnityEngine;
using System.Collections;

namespace FKGame{
	public class DontDestroyOnLoad : MonoBehaviour {
		private void Awake(){
			DontDestroyOnLoad (gameObject);
		}
	}
}