using System.Collections.Generic;

namespace CWI.PostManEvent.Common.Hubs
{
    /// <summary>
    /// Hub de controle de eventos, podendo ser configurado de diversas maneiras para melhor atender as necessidades do domínio
    /// </summary>
    public interface IHubEvent
    {
        /// <summary>
        /// Quando um evento é publicado, o evento é enviado ao hub para que seja processado caso necessário
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postManEvent"></param>
        void Publish<T>(T postManEvent) where T : Events.BasePostManEvent;

        void Subscribe<T, E>(E subscribe) 
            where E : IPostManSubscribe
            where T : Events.BasePostManEvent;

        void Unsubscribe<T, S>(S subscribe)
           where S : IPostManSubscribe
           where T : Events.BasePostManEvent;

        IEnumerable<T> Published<T>()
            where T : Events.BasePostManEvent;

        bool HasPublished<T>();
    }
}
