using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public class QuestPickerAttribute : PropertyAttribute
    {
		public bool utility;
		public QuestPickerAttribute() : this(false) { }

		public QuestPickerAttribute(bool utility)
		{
			this.utility = utility;
		}
	}
}