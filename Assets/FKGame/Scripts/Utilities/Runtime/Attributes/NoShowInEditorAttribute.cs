using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // ����Ĭ��Editor GUI������ʾ
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NoShowInEditorAttribute : Attribute { }
}