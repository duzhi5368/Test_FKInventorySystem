using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FKGame.QuestSystem
{
	[System.Serializable]
	public class QuestDatabase : ScriptableObject
	{
		public List<Quest> items = new List<Quest>();
		public List<Settings> settings = new List<Settings>();
	}
}