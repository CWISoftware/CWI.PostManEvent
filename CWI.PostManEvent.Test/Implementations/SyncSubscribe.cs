using CWI.PostManEvent.Hubs.AsyncSync;

namespace CWI.PostManEvent.Test.Implementations
{
    public class SyncSubscribe : AsyncSyncSubscribe
    {
        public override AsyncSyncEventType Type
        {
            get
            {
                return AsyncSyncEventType.Sync;
            }
        }

        public override void Published<T>(T postManEvent)
        {
        }
    }
}
