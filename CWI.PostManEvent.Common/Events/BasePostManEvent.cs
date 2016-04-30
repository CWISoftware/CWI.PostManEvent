using CWI.PostManEvent.Common.Hubs;
using System;
using System.Linq;
using System.Collections.Concurrent;

namespace CWI.PostManEvent.Common.Events
{
    public abstract class BasePostManEvent
    {
        public ConcurrentBag<ResultEvent> Results { get; private set; }

        public BasePostManEvent()
        {
            Results = new ConcurrentBag<ResultEvent>();
        }

        public ResultEvent ProcessingFor(IPostManSubscribe subscribe)
        {
            if (AlreadyProcessed(subscribe.GetType()))
                throw new ArgumentException($"Evento {this.GetType()}  já está sendo ou foi processado pelo {subscribe.GetType()}");

            var result = new ResultEvent
            {
                Event = this,
                State = ResultEventState.Running,
                Subscribe = subscribe
            };

            Results.Add(result);

            return result;
        }

        public void ProcessedFor(IPostManSubscribe subscribe)
        {
            if (!AlreadyProcessed(subscribe.GetType()))
                throw new InvalidOperationException($"Não foi possivel sinalizar que o subscribe {subscribe.GetType()} foi processado");

            Results.Single(r => r.Subscribe.GetType() == subscribe.GetType()).State = ResultEventState.Completed;
        }

        protected virtual bool AlreadyProcessed(Type subscribe)
        {
            return Results.Any(r => r.Subscribe.GetType() == subscribe);
        }
    }
}
