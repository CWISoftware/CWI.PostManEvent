using CWI.PostManEvent.Common.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CWI.PostManEvent.Common.Hubs
{
    /// <summary>
    /// Hub de controle de eventos, podendo ser configurado de diversas maneiras para melhor atender as necessidades do domínio
    /// </summary>
    public abstract class BaseHubEvent
    {
        protected readonly ConcurrentDictionary<Type, List<IPostManSubscribe>> subscribes = new ConcurrentDictionary<Type, List<IPostManSubscribe>>();
        protected readonly ConcurrentBag<BasePostManEvent> events = new ConcurrentBag<BasePostManEvent>();

        public virtual void Publish(BasePostManEvent postManEvent)
        {
            events.Add(postManEvent);

            if (!HasSubscribe(postManEvent.GetType()))
                return;

            ProcessPublish(postManEvent);
        }

        protected List<IPostManSubscribe> ListSubscribesFor(BasePostManEvent postManEvent)
        {
            return subscribes[postManEvent.GetType()];
        }

        protected abstract void ProcessPublish(BasePostManEvent postManEvent);


        /// <summary>
        /// Quando um evento é publicado, o evento é enviado ao hub para que seja processado caso necessário
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postManEvent"></param>
        public virtual bool HasPublished<T>()
        {
            return events.Any(e => typeof(T) == e.GetType());
        }

        public virtual void Subscribe<E>(IPostManSubscribe subscribe) 
            where E : Events.BasePostManEvent
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

        public virtual void Unsubscribe<T>(IPostManSubscribe subscribe)
           where T : Events.BasePostManEvent
        {
            List<IPostManSubscribe> listSub = subscribes[typeof(T)];

            if (listSub != null)
            {
                listSub.Remove(subscribe);
            }
        }

        public virtual IEnumerable<T> Published<T>()
            where T : Events.BasePostManEvent
        {
            return events.Where(e => typeof(T) == e.GetType()).Select(e => (T)e).ToList();
        }

        protected virtual bool HasSubscribe(Type subscribe)
        {
            return subscribes.Any(s => s.Key == subscribe);
        }
    }
}
