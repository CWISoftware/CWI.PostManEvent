using CWI.PostManEvent.Hubs.AsyncSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
