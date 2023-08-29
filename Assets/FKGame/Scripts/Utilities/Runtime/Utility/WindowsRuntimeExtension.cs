using System;
//------------------------------------------------------------------------
namespace FKGame
{
	public static class WindowsRuntimeExtension
	{
		public static Type GetBaseType(this Type type)
		{
			return type.BaseType;
		}
	}
}
