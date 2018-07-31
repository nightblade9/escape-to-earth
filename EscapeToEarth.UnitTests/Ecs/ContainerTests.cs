using EscapeToEarth.Ecs;
using EscapeToEarth.UnitTest.Helpers;
using NUnit.Framework;
using System.Linq;

namespace EscapeToEarth.UnitTest.Ecs
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void GetSystemGetsAddedSystem()
        {
            var c = new Container();
            Assert.That(c.GetSystem<InspectableSystem>(), Is.Null);

            var expected = new InspectableSystem();
            c.AddSystem(expected);

            Assert.That(c.GetSystem<InspectableSystem>(), Is.EqualTo(expected));
        }

        [Test]
        public void AddEntityAddsEntityToAllSystems()
        {
            // Arrange
            var container = new Container();
            var expected = new Entity();

            var s1 = new InspectableSystem();
            var s2 = new InspectableSystem();

            container.AddSystem(s1);
            container.AddSystem(s2);

            // Act
            container.AddEntity(expected);

            // Assert
            Assert.That(s1.Entities.Single(), Is.EqualTo(expected));
            Assert.That(s2.Entities.Single(), Is.EqualTo(expected));
        }

        [Test]
        public void UpdateUpdatesAllSystems()
        {
            // Arrange
            var container = new Container();

            var s1 = new InspectableSystem();
            var s2 = new InspectableSystem();

            container.AddSystem(s1);
            container.AddSystem(s2);

            // Act
            container.Update(1);
            container.Update(2);
            container.Update(3);

            // Assert
            Assert.That(s1.TotalUpdates, Is.EqualTo(6));
            Assert.That(s2.TotalUpdates, Is.EqualTo(6));
        }
    }
}