using UnityEngine;
using System.Collections;

namespace FKGame.UIWidgets{
	internal interface ITweenValue
	{
		void TweenValue(float floatPercentage);
		bool ignoreTimeScale { get; }
		float duration { get; }
		bool ValidTarget();
		void OnFinish();
	}
}