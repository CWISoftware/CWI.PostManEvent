using CWI.PostManEvent.Hubs.AsyncSync;
using System.Threading;

namespace CWI.PostManEvent.Test.Implementations
{
    public class AsyncSubscribe : AsyncSyncSubscribe
    {
        public bool Running = true;


        public override AsyncSyncEventType Type
        {
            get
            {
                return AsyncSyncEventType.Async;
            }
        }

        public override void Published<T>(T postManEvent)
        {
            while(Running)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
