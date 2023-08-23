//------------------------------------------------------------------------
// 继承该类可实现值的渐变
//------------------------------------------------------------------------
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