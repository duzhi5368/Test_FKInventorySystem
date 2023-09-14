using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class INetMsgProcessPackestPluginBase : INetMsgProcessPluginBase
    {
        protected Queue<MsgPackest> msgQueue = new Queue<MsgPackest>();

        public override void ReceveProcess(MsgPackest packest)
        {
            msgQueue.Enqueue(packest);
            OnMsgReceve();
        }
        protected virtual void OnMsgReceve() { }
    }
}