using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace visma.pubsub.tests
{
    [TestClass]
    public class EventBusTests
    {
        private IEventBus _bus;
        //private object _doomedContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _bus = new EventBus();
        }

        [TestMethod]
        public void Publish_InvokeSubscriberByType()
        {
            var count = 0;
            _bus.Subscribe<int>(new object(), d => count = d);

            _bus.Publish(10);

            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void Publish_InvokeAllSubscribersByType()
        {
            var count = 0;
            _bus.Subscribe<int>(new object(), d => count += 2);
            _bus.Subscribe<int>(new object(), d => count += 2);

            _bus.Publish(10);

            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void Publish_InvokeOnlyCorrectSubscriberByType()
        {
            var count = 0;
            var message = string.Empty;
            var context = new object();
            _bus.Subscribe<int>(context, d => count++);
            _bus.Subscribe<string>(context, d => message = d);

            _bus.Publish("hello from the other side");

            Assert.AreEqual(0, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Publish_DataPassedCorrectlyToSubscribers()
        {
            var count = 0;
            var message = string.Empty;
            _bus.Subscribe<int>(new object(), d => count = d);
            _bus.Subscribe<string>(new object(), d => message = d);

            _bus.Publish(20);
            _bus.Publish("hello from the other side");

            Assert.AreEqual(20, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Publish_DisposedContextRemovedFromSubscribersList()
        {
            var count = 0;
            var liveContext = new object();
            SubscribeDisposedInstance<int>(_bus, d => count++);
            _bus.Subscribe<int>(liveContext, d => count++);
            GC.Collect();

            _bus.Publish(10);

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UnPublish_PublisherRemovedSuccessfully()
        {
            var count = 0;
            var message = string.Empty;
            _bus.Subscribe<int>(new object(), d => count = d);
            _bus.Subscribe<string>(new object(), d => message = d);

            _bus.UnPublish<int>();
            _bus.Publish(20);
            _bus.Publish("hello from the other side");

            Assert.AreEqual(0, count);
            Assert.AreEqual("hello from the other side", message);
        }

        [TestMethod]
        public void Unsubscribe_ByContext_RemovedSuccesfully()
        {
            var count = 0;
            var context = new object();
            _bus.Subscribe<int>(context, d => count += 2);

            _bus.Publish(10);
            _bus.Unsubscribe(context);
            _bus.Publish(10);

            Assert.AreEqual(2, count);
        }

        /// <summary>
        /// /// A helper method that will not inline to its caller to ensure JIT-preserved locals don't interfere with weak reference tests.
        /// see <see cref="https://github.com/dotnet/runtime/issues/8561"/>
        /// </summary>
        private void SubscribeDisposedInstance<T>(IEventBus bus, Action<T> func)
        {
            bus.Subscribe(new object(), func);
        }
    }
}