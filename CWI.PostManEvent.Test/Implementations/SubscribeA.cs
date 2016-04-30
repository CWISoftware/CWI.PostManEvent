using CWI.PostManEvent.Common.Events;
using CWI.PostManEvent.Common.Hubs;

namespace CWI.PostManEvent.Test.Implementations
{
    public class SubscribeA : IPostManSubscribe
    {
        public void Published<T>(T postManEvent) where T : BasePostManEvent
        {

        }
    }
}
