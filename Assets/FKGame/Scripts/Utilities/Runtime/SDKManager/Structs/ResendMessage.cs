namespace FKGame
{
    public class ResendMessage
    {
        public string removeMT;
        public string mt;
        public string content;
        public bool noSend = false;     // ������Ϣ��Ҳ���ط�����ֻ��������
        public ResendMessage() { }
        public ResendMessage(string removeMT, string mt, string content, bool noSend)
        {
            this.removeMT = removeMT;
            this.mt = mt;
            this.content = content;
            this.noSend = noSend;
        }
    }
}