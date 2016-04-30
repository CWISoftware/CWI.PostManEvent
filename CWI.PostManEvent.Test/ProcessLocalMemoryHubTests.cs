using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CWI.PostManEvent.Common.Hubs;
using CWI.PostManEvent.Hubs.LocalMemory;
using CWI.PostManEvent.Common.Events;
using CWI.PostManEvent.Test.Implementations;

namespace CWI.PostManEvent.Test
{
    [TestClass]
    public class ProcessLocalMemoryHubTests
    {
        IHubEvent hubEvent;

        [TestInitialize]
        public void Init()
        {
            hubEvent = new LocalMemoryHub();

            PostManManager.Instance.SetHub(hubEvent);
        }

        [TestCleanup]
        public void Terminate()
        {
            PostManManager.Clean();
        }


        [TestMethod]
        public void ExecutaEventoComSucessoSemSubscricao()
        {
            PostManManager.Raise(new EventAEx());

            var events = PostManManager.Events<EventAEx>().ToList();

            Assert.AreEqual(0, events.Single().Results.Count);
            Assert.AreEqual(1, PostManManager.Events().Count());
        }

        [TestMethod]
        public void VerificaSeExisteSubscribeSimples()
        {
            PostManManager.Raise(new EventAEx());

            Assert.IsTrue(hubEvent.HasPublished<EventAEx>());
            Assert.IsFalse(hubEvent.HasPublished<EventBEx>());
        }

        [TestMethod]
        public void ListaEventosPublicados()
        {
            PostManManager.Raise(new EventAEx());
            PostManManager.Raise(new EventBEx());
            PostManManager.Raise(new EventAEx());


            Assert.AreEqual(2, hubEvent.Published<EventAEx>().Count());
            Assert.AreEqual(1, hubEvent.Published<EventBEx>().Count());
            Assert.AreEqual(0, hubEvent.Published<EventCEx>().Count());
        }

        [TestMethod]
        public void ExecutarComSubscribeSimples()
        {
            var sub = new SubscribeA();
            hubEvent.Subscribe<EventAEx, SubscribeA>(sub);

            var eventEx = new EventAEx();
            PostManManager.Raise(eventEx);

            var events = PostManManager.Events<EventAEx>().ToList();

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(1, events.Single().Results.Count);

            var result = events.Single().Results.Single();
            Assert.IsTrue(result.State == ResultEventState.Completed);
            Assert.AreEqual(sub, result.Subscribe);
            Assert.AreEqual(eventEx, result.Event);
            Assert.AreEqual(0, result.Exceptions.Count);
        }

        [TestMethod]
        public void PublicacaoParaMesmoEvento()
        {
            var suba = new SubscribeA();
            hubEvent.Subscribe<EventAEx, SubscribeA>(suba);

            var subb = new SubscribeB();
            hubEvent.Subscribe<EventAEx, SubscribeB>(subb);

            var eventEx = new EventAEx();
            PostManManager.Raise(eventEx);

            var events = PostManManager.Events<EventAEx>().ToList();

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(2, events.Single().Results.Count);
        }

        [TestMethod]
        public void RemoverSubscricao()
        {
            var suba = new SubscribeA();
            hubEvent.Subscribe<EventAEx, SubscribeA>(suba);

            var subb = new SubscribeB();
            hubEvent.Subscribe<EventAEx, SubscribeB>(subb);
            hubEvent.Subscribe<EventBEx, SubscribeB>(subb);

            hubEvent.Unsubscribe<EventAEx, SubscribeB>(subb);

            var eventAEx = new EventAEx();
            PostManManager.Raise(eventAEx);

            var eventBEx = new EventBEx();
            PostManManager.Raise(eventBEx);

            var eventsA = PostManManager.Events<EventAEx>().ToList();
            var eventsB = PostManManager.Events<EventBEx>().ToList();

            Assert.AreEqual(1, eventsA.Single().Results.Count);
            Assert.AreEqual(1, eventsB.Single().Results.Count);

        }

    }
}
