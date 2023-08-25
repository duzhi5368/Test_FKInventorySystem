using UnityEngine;
using System.Collections.Generic;
//------------------------------------------------------------------------;
// 任务信息数据库
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
	[System.Serializable]
	public class QuestDatabase : ScriptableObject
	{
		public List<Quest> items = new List<Quest>();
		public List<Settings> settings = new List<Settings>();
	}
}