using CWI.PostManEvent.Common.Hubs;
using System;

namespace CWI.PostManEvent.Common.Hubs
{
    public class PostManResolver : IPostManResolver
    {
        public object GetSubscribe(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
