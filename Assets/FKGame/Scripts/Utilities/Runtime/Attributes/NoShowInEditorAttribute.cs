using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // 不在默认Editor GUI里面显示
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NoShowInEditorAttribute : Attribute { }
}