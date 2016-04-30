using CWI.PostManEvent.Common.Hubs;
using System.Collections.Generic;
using CWI.PostManEvent.Common.Events;
using System.Threading.Tasks;

namespace CWI.PostManEvent.Hubs.AsyncSync
{
    public class AsyncSyncHub : BaseHubEvent
    {
        protected override void ProcessPublish(BasePostManEvent postManEvent)
        {
            List<Task> taskPool = new List<Task>();

            var currentSubscribes = ListSubscribesFor(postManEvent);

            if (currentSubscribes != null)
            {
                Parallel.ForEach(currentSubscribes, s =>
                {
                    var sync = !(s is AsyncSyncSubscribe) || 
                              (s as AsyncSyncSubscribe).Type == AsyncSyncEventType.Sync;

                    postManEvent.ProcessingFor(s);

                    if (sync)
                    {
                        taskPool.Add(Task.Run(() =>
                        {
                            Process(postManEvent, s);
                        }));
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Process(postManEvent, s);
                        });
                    }

                });

                taskPool.ForEach(t => t.Wait());
            }
        }

        private void Process<T>(T postManEvent, IPostManSubscribe s) where T : BasePostManEvent
        {
            s.Published(postManEvent);
            postManEvent.ProcessedFor(s);
        }
    }
}
