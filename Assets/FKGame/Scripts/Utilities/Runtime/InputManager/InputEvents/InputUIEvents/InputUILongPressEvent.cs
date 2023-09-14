namespace FKGame
{
    public class InputUILongPressEvent : InputUIEventBase
    {
        public static string GetEventKey(string UIName, string ComponentName, string pram = null)
        {
            return UIName + "." + ComponentName + "." + pram + "." + InputUIEventType.LongPress.ToString();
        }
    }
}