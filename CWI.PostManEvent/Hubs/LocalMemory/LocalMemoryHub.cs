using CWI.PostManEvent.Common.Hubs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using CWI.PostManEvent.Common.Events;
using System.Threading.Tasks;

namespace CWI.PostManEvent.Hubs.LocalMemory
{
    public class LocalMemoryHub : IHubEvent
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

            if (!HasSubscribe(postManEvent.GetType()))
                return;

            var currentSubscribes = subscribes[postManEvent.GetType()];

            if (currentSubscribes != null)
            {
                Parallel.ForEach(currentSubscribes, s =>
                {
                    postManEvent.ProcessingFor(s);

                    s.Published(postManEvent);
                    postManEvent.ProcessedFor(s);

                });
            }
        }

        public IEnumerable<T> Published<T>() 
            where T : BasePostManEvent
        {
            return events.Where(e => typeof(T) == e.GetType()).Select(e => (T)e).ToList();
        }

        public void Subscribe<E, S>(S subscribe)
            where E : BasePostManEvent
            where S : IPostManSubscribe
        {
            List<IPostManSubscribe> listSub;

            if (!HasSubscribe(typeof(E)))
            {
                listSub = new List<IPostManSubscribe>();
                subscribes[typeof(E)] = listSub;
            }
            else
            {
                listSub = subscribes[typeof(E)];
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

        private bool HasSubscribe(Type subscribe)
        {
            return subscribes.Any(s => s.Key == subscribe);
        }
    }
}
