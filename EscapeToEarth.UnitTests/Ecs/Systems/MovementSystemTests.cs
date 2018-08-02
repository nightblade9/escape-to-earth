using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs.Systems;
using EscapeToEarth.Entities;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SadConsole.Input;

namespace EscapeToEarth.UnitTest.Ecs.Systems
{
    [TestFixture]
    public class MovementSystemTests
    {
        [TearDown]
        public void ClearGlobalStateThings()
        {
            new EventBus(); // Reset event bus
            SadConsole.Global.KeyboardState.KeysPressed.Clear();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UpdateChangesPlayerPositionIfNewPositionIsWalkable(bool isDestinationWalkable)
        {
            // Arrange
            var player = new Entity();
            var system = new MovementSystem(player);

            player.Set(new MoveToKeyboardComponent(player));
            player.Position.X = 3;
            system.Add(player);
            player.Position.Y = 3;
            
            var playerMoved = false;

            var map = new ArrayMap<MapTile>(5, 5);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = new MapTile() { IsWalkable = true };
                }
            }

            map[player.Position.X - 1, player.Position.Y].IsWalkable = isDestinationWalkable;
            SadConsole.Global.KeyboardState.KeysPressed.Add(AsciiKey.Get(Keys.Left));

            // Update the map in the movement system
            EventBus.Instance.Broadcast("Map changed", map);
            EventBus.Instance.Register("Player moved", (data) => playerMoved = true);

            var expectedX = isDestinationWalkable ? player.Position.X - 1 : player.Position.X;

            // Act
            system.Update(0);

            // Assert
            Assert.That(player.Position.X, Is.EqualTo(expectedX)); // Position changed
            Assert.That(playerMoved, Is.EqualTo(isDestinationWalkable)); // Event fired (or not)
        }
    }
}