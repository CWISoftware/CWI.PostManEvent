using CWI.PostManEvent.Common.Events;

namespace CWI.PostManEvent.Common.Hubs
{
    public interface IPostManSubscribe
    {
        void Published<T>(T postManEvent) where T : BasePostManEvent;
    }
}
