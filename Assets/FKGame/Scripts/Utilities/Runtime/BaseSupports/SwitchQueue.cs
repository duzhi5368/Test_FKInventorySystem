using System.Collections;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SwitchQueue<T> where T : class
    {
        private Queue mConsumeQueue;
        private Queue mProduceQueue;

        public SwitchQueue()
        {
            mConsumeQueue = new Queue(16);
            mProduceQueue = new Queue(16);
        }

        public SwitchQueue(int capcity)
        {
            mConsumeQueue = new Queue(capcity);
            mProduceQueue = new Queue(capcity);
        }

        public void Push(T obj)
        {
            lock (mProduceQueue)
            {
                mProduceQueue.Enqueue(obj);
            }
        }

        public T Pop()
        {

            return (T)mConsumeQueue.Dequeue();
        }

        public bool Empty()
        {
            return 0 == mConsumeQueue.Count;
        }

        public void Switch()
        {
            lock (mProduceQueue)
            {
                UnityTools.Swap(ref mConsumeQueue, ref mProduceQueue);
            }
        }

        public void Clear()
        {
            lock (mProduceQueue)
            {
                mConsumeQueue.Clear();
                mProduceQueue.Clear();
            }
        }
    }
}