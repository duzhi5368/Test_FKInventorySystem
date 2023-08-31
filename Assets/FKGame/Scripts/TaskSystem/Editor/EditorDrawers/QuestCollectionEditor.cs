using FKGame.Macro;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
	[System.Serializable]
	public class QuestCollectionEditor : ScriptableObjectCollectionEditor<Quest>
	{
		public override string ToolbarName
		{
			get
			{
				return LanguagesMacro.QUESTS;
			}
		}

		public QuestCollectionEditor(UnityEngine.Object target, List<Quest> items, List<string> searchFilters) : base(target, items)
		{
		}
	}
}