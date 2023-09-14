using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NotJsonSerializedAttribute : Attribute
    {

    }
}