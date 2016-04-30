using CWI.PostManEvent.Common.Hubs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using CWI.PostManEvent.Common.Events;
using System.Threading.Tasks;

namespace CWI.PostManEvent.Hubs.AsyncSync
{
    public class AsyncSyncHub : IHubEvent
    {
        private readonly ConcurrentDictionary<Type, List<IPostManSubscribe>> subscribes = new ConcurrentDictionary<Type, List<IPostManSubscribe>>();
        private readonly ConcurrentBag<BasePostManEvent> events = new ConcurrentBag<BasePostManEvent>();

        public bool HasPublished<T>()
        {
            return events.Any(e => typeof(T) == e.GetType());
        }

        public void Publish<T>(T postManEvent) where T : BasePostManEvent
        {
            events.Add(postManEvent);

            List<Task> taskPool = new List<Task>();

            var currentSubscribes = subscribes[typeof(T)];

            if (currentSubscribes != null)
            {
                Parallel.ForEach(currentSubscribes, s =>
                {
                    var sync = s.GetType().GetInterfaces().Contains(typeof(IPostManSubscribe)) || 
                              (s as AsyncSyncSubscribe)?.Type == AsyncSyncEventType.Sync;

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

        public IEnumerable<T> Published<T>() 
            where T : BasePostManEvent
        {
            return events.Where(e => typeof(T) == e.GetType()).Select(e => (T)e).ToList();
        }

        public void Subscribe<T, E>(E subscribe)
            where T : BasePostManEvent
            where E : IPostManSubscribe
        {
            List<IPostManSubscribe> listSub = subscribes[typeof(T)];

            if (listSub == null)
            {
                listSub = new List<IPostManSubscribe>();
            }

            listSub.Add(subscribe);
        }

        public void Unsubscribe<T, S>(S subscribe)
            where T : BasePostManEvent
            where S : IPostManSubscribe
        {
            List<IPostManSubscribe> listSub = subscribes[typeof(T)];

            if (listSub != null)
            {
                listSub.Remove(subscribe);
            }
        }
    }
}
