using CWI.PostManEvent.Common.Hubs;
using System;
using System.Collections.Generic;

namespace CWI.PostManEvent.Common.Events
{
    public class ResultEvent
    {
        internal ResultEvent()
        {
            Exceptions = new List<Exception>();
        }

        public BasePostManEvent Event { get; set; }

        public IPostManSubscribe Subscribe { get; set; }

        public ResultEventState State { get; set; }

        public List<Exception> Exceptions { get; set; }

        public void AddException(Exception ex)
        {
            Exceptions.Add(ex);
        }

    }
}
