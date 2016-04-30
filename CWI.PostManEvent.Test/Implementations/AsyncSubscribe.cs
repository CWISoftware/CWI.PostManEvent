using CWI.PostManEvent.Hubs.AsyncSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
