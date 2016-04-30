using CWI.PostManEvent.Common.Hubs;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using CWI.PostManEvent.Common.Events;
using System;

namespace CWI.PostManEvent
{
    public class PostManManager
    {
        private const string INSTANCEKEY = "PostManManagerInstance";

        private static PostManManager instance;

        public static PostManManager Instance
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Items[INSTANCEKEY] as PostManManager;
                }

                if (instance == null)
                {
                    instance = new PostManManager();
                }

                return instance;

            }
        }

        public static void Clean()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[INSTANCEKEY] = null;
                return;
            }

            instance = null;
        }

        public static void PerRequest()
        {
            var requestInstance = new PostManManager(instance);
            HttpContext.Current.Items[INSTANCEKEY] = requestInstance;
        }


        private ConcurrentBag<IHubEvent> hubEvents;

        private ConcurrentBag<BasePostManEvent> postManEvent = new ConcurrentBag<BasePostManEvent>();

        private PostManManager()
        {
            this.hubEvents = new ConcurrentBag<IHubEvent>();
        }

        private PostManManager(PostManManager instance)
        {
            this.hubEvents = instance.hubEvents;
        }

        public static void Raise(BasePostManEvent postManEvent)
        {
            Instance.Process(postManEvent);
        }

        public static IEnumerable<T> Events<T>()
            where T : BasePostManEvent
        {
            return Instance.postManEvent.Where(e => e.GetType() == typeof(T)).Select(e => (T)e).ToList();
        }

        public static IEnumerable<BasePostManEvent> Events()
        {
            return Instance.postManEvent.ToList();
        }

        public void SetHub(IHubEvent hubEvent)
        {
            hubEvents.Add(hubEvent);
        }

        private void Process(BasePostManEvent postManEvent)
        {
            this.postManEvent.Add(postManEvent);

            List<Task> taks = new List<Task>();
            Parallel.ForEach(hubEvents, h => 
            {
                taks.Add(Task.Run(() => { h.Publish(postManEvent); }));
            });

            taks.ForEach(t =>
            {
                t.Wait();
            });
        }
    }
}
