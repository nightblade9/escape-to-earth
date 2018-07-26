using EscapeToEarth.Ecs;
using NUnit.Framework;
using System;

namespace EscapeToEarth.UnitTest.Ecs
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void GetGetsSetValue()
        {
            var expected = "Expected Value!";

            var e = new Entity();
            e.Set<String>(expected);
            var actual = e.Get<String>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SetOverridesPreviousValuePerType()
        {
            var expected = "Hello, world!";

            var e = new Entity();
            e.Set<String>("wrong value");
            e.Set<String>(expected);

            var actual = e.Get<String>();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}