using CWI.PostManEvent.Common.Hubs;
using CWI.PostManEvent.Common.Events;
using System.Threading.Tasks;

namespace CWI.PostManEvent.Hubs.LocalMemory
{
    public class LocalMemoryHub : BaseHubEvent
    {
        protected override void ProcessPublish(BasePostManEvent postManEvent)
        {
            var currentSubscribes = ListSubscribesFor(postManEvent);

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
    }
}
