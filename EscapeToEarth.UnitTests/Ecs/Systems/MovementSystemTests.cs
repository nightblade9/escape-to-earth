using EscapeToEarth.Ecs;
using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs.Systems;
using EscapeToEarth.Entities;
using EscapeToEarth.Entities.MapTiles;
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

            var map = new ArrayMap<AbstractMapTile>(5, 5);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = new FloorTile();
                }
            }

            map[player.Position.X - 1, player.Position.Y].IsWalkable = isDestinationWalkable;
            SadConsole.Global.KeyboardState.KeysPressed.Add(AsciiKey.Get(Keys.Left));

            // Update the map in the movement system
            EventBus.Instance.Broadcast("Map changed", map);
            EventBus.Instance.Register<Player>("Player moved", (data) => playerMoved = true);

            var expectedX = isDestinationWalkable ? player.Position.X - 1 : player.Position.X;

            // Act
            system.Update(0);

            // Assert
            Assert.That(player.Position.X, Is.EqualTo(expectedX)); // Position changed
            Assert.That(playerMoved, Is.EqualTo(isDestinationWalkable)); // Event fired (or not)
        }

        [TestCase(Keys.LeftShift)]
        [TestCase(Keys.RightShift)]
        public void UpdateMovesPlayerToNextFloorIfPlayerStandsAboveStairsDownAndShiftDotKeysAreDown(Keys shiftKey)
        {
            // Arrange
            const int StairsX = 3;
            const int StairsY = 3;

            var player = new Entity();
            player.Set(new MoveToKeyboardComponent(player));
            var system = new MovementSystem(player);

            var map = new ArrayMap<AbstractMapTile>(5, 5);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = new FloorTile();
                }
            }

            map[StairsX, StairsY] = new StairsDownTile();
            EventBus.Instance.Broadcast("Map changed", map);

            player.Position.X = StairsX;
            player.Position.Y = StairsY;

            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(shiftKey));
            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(Keys.OemPeriod));
            
            bool usedStairs = false;
            EventBus.Instance.Register<string>("Player used stairs", (floorNumber) => usedStairs = true);
            
            // Act
            system.Update(0);

            // Assert
            Assert.That(usedStairs, Is.True);
        }

        [TestCase(Keys.LeftShift, 1, 0)]
        [TestCase(Keys.RightShift, 0, -1)]
        public void UpdateDoesNotMovePlayerToNextFloorIfPositionIsWrong(Keys shiftKey, int xOffBy, int yOffBy)
        {
            // Arrange
            const int StairsX = 3;
            const int StairsY = 3;

            var player = new Entity();
            player.Set(new MoveToKeyboardComponent(player));
            var system = new MovementSystem(player);

            var map = new ArrayMap<AbstractMapTile>(5, 5);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = new FloorTile();
                }
            }

            map[StairsX, StairsY] = new StairsDownTile();
            EventBus.Instance.Broadcast("Map changed", map);

            player.Position.X = StairsX + xOffBy;
            player.Position.Y = StairsY + yOffBy;

            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(shiftKey));
            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(Keys.OemPeriod));
            
            bool usedStairs = false;
            EventBus.Instance.Register<string>("Player used stairs", (floorNumber) => usedStairs = true);
            
            // Act
            system.Update(0);

            // Assert
            Assert.That(usedStairs, Is.False);
        }

        [TestCase(Keys.LeftShift,  Keys.Delete)]
        [TestCase(Keys.RightShift,  Keys.K)]
        public void UpdateDoesNotMovePlayerToNextFloorIfKeyCombinationDownIsWrong(Keys shiftKey, Keys otherKey)
        {
            // Arrange
            const int StairsX = 3;
            const int StairsY = 3;

            var player = new Entity();
            player.Set(new MoveToKeyboardComponent(player));
            var system = new MovementSystem(player);

            var map = new ArrayMap<AbstractMapTile>(5, 5);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = new FloorTile();
                }
            }

            map[StairsX, StairsY] = new StairsDownTile();
            EventBus.Instance.Broadcast("Map changed", map);

            player.Position.X = StairsX;
            player.Position.Y = StairsY;

            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(shiftKey));
            SadConsole.Global.KeyboardState.KeysDown.Add(AsciiKey.Get(otherKey));
            
            bool usedStairs = false;
            EventBus.Instance.Register<string>("Player used stairs", (floorNumber) => usedStairs = true);
            
            // Act
            system.Update(0);

            // Assert
            Assert.That(usedStairs, Is.False);
        }

        
    }
}