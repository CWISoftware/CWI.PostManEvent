using CWI.PostManEvent.Common.Events;
using CWI.PostManEvent.Common.Hubs;

namespace CWI.PostManEvent.Hubs.AsyncSync
{
    public abstract class AsyncSyncSubscribe : IPostManSubscribe
    {
        public abstract AsyncSyncEventType Type { get; }

        public abstract void Published<T>(T postManEvent) where T : BasePostManEvent;
    }
}
