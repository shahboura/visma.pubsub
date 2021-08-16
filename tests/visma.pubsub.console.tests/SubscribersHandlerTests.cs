using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace visma.pubsub.console.tests
{
    [TestClass]
    public class SubscribersHandlerTests
    {
        private SubscribersHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            var eventAggregator = new Mock<IEventBus>();
            _handler = new SubscribersHandler(eventAggregator.Object);
        }

        [TestMethod]
        public void AddSubscriber_ShouldHaveCorrectNumberofSubscibers()
        {
            _handler.AddSubscriber<SumSubscriber>();
            _handler.AddSubscriber<PingSubscriber>();
            _handler.AddSubscriber<SumSubscriber>();

            Assert.IsTrue(_handler.Subscribers.Count(s => s.GetType() == typeof(PingSubscriber)) == 1);
            Assert.IsTrue(_handler.Subscribers.Count(s => s.GetType() == typeof(SumSubscriber)) == 2);
        }

        [TestMethod]
        public void Unsubscribe_ShouldUnsubscribeCorrectly()
        {
            var firstSub = _handler.AddSubscriber<SumSubscriber>();
            var secondSub = _handler.AddSubscriber<SumSubscriber>();

            _handler.Unsubscribe(secondSub.Name);

            Assert.IsFalse(_handler.Subscribers
                .First(s => s.Name == secondSub.Name).IsWired);
            Assert.IsTrue(_handler.Subscribers
                .First(s => s.Name == firstSub.Name).IsWired);
        }

        [TestMethod]
        public void DeleteSubscriber()
        {
            var firstSub = _handler.AddSubscriber<SumSubscriber>();
            var secondSub = _handler.AddSubscriber<SumSubscriber>();

            _handler.DeleteSubscriber(secondSub.Name);

            Assert.IsFalse(_handler.Subscribers
                .Any(s => s.Name == secondSub.Name));
        }
    }
}
