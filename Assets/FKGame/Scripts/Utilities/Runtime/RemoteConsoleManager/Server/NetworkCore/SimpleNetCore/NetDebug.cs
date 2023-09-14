using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class NetDebug
    {
        public static System.Action<string> Log = Console.WriteLine;
        public static System.Action<string> LogWarning = Console.WriteLine;
        public static System.Action<string> LogError = Console.Error.WriteLine;
    }
}