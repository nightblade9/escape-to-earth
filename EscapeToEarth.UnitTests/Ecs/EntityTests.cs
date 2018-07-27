using EscapeToEarth.Ecs;
using EscapeToEarth.UnitTest.Helpers;
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
            var e = new Entity();
            var expected = new StringComponent(e) { Value = "Abc" };

            e.Set(expected);
            var actual = e.Get<StringComponent>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SetOverridesPreviousValuePerType()
        {
            var e = new Entity();
            var expected = new StringComponent(e) { Value = "correct" };

            e.Set(new StringComponent(e) { Value = "FAIL!!! " });
            e.Set(expected);

            var actual = e.Get<StringComponent>();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}