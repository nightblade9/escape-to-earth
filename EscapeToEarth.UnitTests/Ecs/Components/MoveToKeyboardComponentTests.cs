using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SadConsole.Input;
using System.Collections.Generic;

namespace EscapeToEarth.UnitTest.Ecs.Components
{
    [TestFixture]
    public class MoveToKeyboardComponentTests
    {
        [Test]
        public void UpdateMovesPlayerPositionToArrowKeys()
        {
            var e = new Entity();
            var component = new MoveToKeyboardComponent(e);
            e.Position.X = 5;
            e.Position.Y = 7;

            var keys = new List<AsciiKey>() { AsciiKey.Get(Keys.Up) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(5));
            Assert.That(e.Position.Y, Is.EqualTo(6));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.Right) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(6));
            Assert.That(e.Position.Y, Is.EqualTo(6));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.Down) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(6));
            Assert.That(e.Position.Y, Is.EqualTo(7));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.Left) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(5));
            Assert.That(e.Position.Y, Is.EqualTo(7));
        }

        [Test]
        public void UpdateMovesPlayerPositionToWasdKeys()
        {
            var e = new Entity();
            var component = new MoveToKeyboardComponent(e);
            e.Position.X = 3;
            e.Position.Y = 3;

            var keys = new List<AsciiKey>() { AsciiKey.Get(Keys.W) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(3));
            Assert.That(e.Position.Y, Is.EqualTo(2));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.D) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(4));
            Assert.That(e.Position.Y, Is.EqualTo(2));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.S) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(4));
            Assert.That(e.Position.Y, Is.EqualTo(3));

            keys = new List<AsciiKey>() { AsciiKey.Get(Keys.A) };
            component.Update(keys);
            Assert.That(e.Position.X, Is.EqualTo(3));
            Assert.That(e.Position.Y, Is.EqualTo(3));
        }
    }
}