namespace FKGame
{
    public enum UIType
    {
        GameUI = 0,
        Fixed = 1,
        Normal = 2,
        TopBar = 3,
        Upper = 4,
        PopUp = 5,
    }

    public enum WindowStatus
    {
        Create,
        Open,
        Close,
        OpenAnim,
        CloseAnim,
        Hide,
    }

    public enum AlignType
    {
        Right,
        Left,
        Center,
    }

    public enum GradientMode
    {
        Global,
        Local
    }

    public enum GradientDir
    {
        Vertical,
        Horizontal,
        DiagonalLeftToRight,
        DiagonalRightToLeft
        //Free
    }

    public enum UIEvent
    {
        OnOpen,
        OnOpened,
        OnClose,
        OnClosed,
        OnHide,
        OnShow,
        OnInit,
        OnDestroy,
        OnRefresh,
        OnStartEnterAnim,
        OnCompleteEnterAnim,
        OnStartExitAnim,
        OnCompleteExitAnim,
    }

    public class ReusingData
    {
        public int index;
        public ReusingStatus status;
    }

    public enum ReusingStatus
    {
        Show,
        Hide
    }
}