using System;
//------------------------------------------------------------------------
namespace FKGame.Telepathy
{
    public static class Logger
    {
        public static Action<string> Log = Console.WriteLine;
        public static Action<string> LogWarning = Console.WriteLine;
        public static Action<string> LogError = Console.Error.WriteLine;
    }
}