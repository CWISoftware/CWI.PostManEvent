using System;

namespace CWI.PostManEvent.Common.Hubs
{
    public interface IPostManResolver
    {
        object GetSubscribe(Type type);

    }
}
