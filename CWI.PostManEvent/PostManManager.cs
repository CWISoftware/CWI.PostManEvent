using CWI.PostManEvent.Common.Hubs;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using CWI.PostManEvent.Common.Events;

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
                PostManManager request = null;

                if (HttpContext.Current != null)
                {
                    request = HttpContext.Current.Items[INSTANCEKEY] as PostManManager;
                }

                if (instance == null && request == null)
                {
                    instance = new PostManManager();
                    request = instance;
                }
                else
                {
                    request = instance;
                }

                return request;

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


        private readonly ConcurrentBag<BaseHubEvent> hubEvents;

        private readonly ConcurrentBag<BasePostManEvent> postManEvent = new ConcurrentBag<BasePostManEvent>();

        private PostManManager()
        {
            this.hubEvents = new ConcurrentBag<BaseHubEvent>();
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

        public void SetHub(BaseHubEvent hubEvent)
        {
            hubEvents.Add(hubEvent);
        }

        private void Process(BasePostManEvent postManEvent)
        {
            this.postManEvent.Add(postManEvent);

            foreach(var h in hubEvents)
            {
                h.Publish(postManEvent);
            }
        }
    }
}
