namespace FKGame
{
    public class AccountMergeInfo2Client : CodeMessageBase
    {
        public bool alreadyExistAccount;        // Ҫ���˻��Ƿ��Ѵ���һ�������˻�
        public User mergeAccount;               // ��Ҫ���˻��Ѵ��ڣ�User����

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}