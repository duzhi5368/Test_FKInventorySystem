namespace FKGame
{
    public class AccountMergeInfo2Client : CodeMessageBase
    {
        public bool alreadyExistAccount;        // 要绑定账户是否已存在一个单独账户
        public User mergeAccount;               // 当要绑定账户已存在，User数据

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}