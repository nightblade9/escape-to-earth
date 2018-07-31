using EscapeToEarth.Ecs;
using NUnit.Framework;

namespace EscapeToEarth.UnitTest.Ecs
{
    [TestFixture]
    public class EventBusTests
    {
        [Test]
        public void BroadcastBroadcastsEventWithDataToRegisteredCallbacks()
        {
            // Arrange
            var bus = new EventBus();
            var expectedData = new object[] { "hi", 37, -7 };
            var callbacksInvoked = 0;

            // Case #1: Multiple handlers for the same event
            bus.Register("E1", (data) => {
                if (data == expectedData[0])
                {
                    callbacksInvoked++;
                }
            });

            bus.Register("E1", (data) => {
                if (data == expectedData[1])
                {
                    callbacksInvoked++;
                }
            });

            // Case #2: a single handler
            bus.Register("E2", (data) => {
                Assert.That(data, Is.EqualTo(expectedData[2]));
                callbacksInvoked++;
            });

            // Act/Assert
            bus.Broadcast("E1", expectedData[0]);
            Assert.That(callbacksInvoked, Is.EqualTo(1));

            bus.Broadcast("E1", expectedData[1]);
            Assert.That(callbacksInvoked, Is.EqualTo(2));

            bus.Broadcast("E2", expectedData[2]);
            Assert.That(callbacksInvoked, Is.EqualTo(3));

            // Case #3: event nobody cares about
            bus.Broadcast("garbage", expectedData[0]);
            bus.Broadcast("garbage", expectedData[1]);
            bus.Broadcast("garbage", expectedData[2]);

            // Number of caught events didn't change
            Assert.That(callbacksInvoked, Is.EqualTo(3));

        }
    }
}