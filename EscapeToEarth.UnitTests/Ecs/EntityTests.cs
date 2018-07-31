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

        [Test]
        public void EntityPositionDefaultsToZeroCoordinates()
        {
            var e = new Entity();
            Assert.That(e.Position.X, Is.EqualTo(0));
            Assert.That(e.Position.Y, Is.EqualTo(0));
        }

        [Test]
        public void HasReturnsTrueIffEntityHasThatComponentType()
        {
            var e = new Entity();
            Assert.That(e.Has<StringComponent>(), Is.False);

            e.Set(new StringComponent(e) { Value = "hello, world!" });
            Assert.That(e.Has<StringComponent>(), Is.True);
        }
    }
}