using EscapeToEarth.Ecs;
using EscapeToEarth.Generators;
using EscapeToEarth.UnitTest.Helpers;
using GoRogue;
using GoRogue.MapViews;
using GoRogue.Pathing;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EscapeToEarth.UnitTest.Generators
{
    [TestFixture]
    public class MapGeneratorTests
    {
        [Test]
        public void GenerateFloorGeneratesMapWithWalkablePathFromPlayerToStairsAndStairsIsSufficientlyFarAway()
        {
            ////
            // With small map sizes (eg. 15x15), GoRogue often generates solid maps or maps with very few walkable tiles.
            // See: https://github.com/Chris3606/GoRogue/issues/56
            // Generate map big enough to separate player/stairs. Use A* path-finding to quickly walk it.
            ////

            // Get the player and stairs position
            // Path-find (BFS) and make sure we can reach
            // verify the distance is large enough
            var mapData = MapGenerator.GenerateFloor(1, 40, 40);

            var map = mapData.Map;
            var playerPosition = mapData.PlayerPosition;
            var stairsDownPosition = mapData.StairsDownPosition;

            // True = walkable
            Assert.That(map[playerPosition.X, playerPosition.Y], Is.True);
            Assert.That(map[stairsDownPosition.X, stairsDownPosition.Y], Is.True);

            // Path-find! Use A* path-finding because breadth-first search seems to take forever.
            var pathFinder = new AStar(map, Distance.MANHATTAN); // Manhattan = four-way only
            var path = pathFinder.ShortestPath(playerPosition, stairsDownPosition);
            Assert.That(path, Is.Not.Null);
            
            foreach (var step in path.Steps)
            {
                // Make sure it gave us a walkable path. Sorry, I don't understand the GoRogue docs on ShortestPath.
                Assert.That(map[step.X, step.Y], Is.EqualTo(true));
            }

            var playerToStairsDistance = Math.Abs(playerPosition.X - stairsDownPosition.X) + Math.Abs(playerPosition.Y - stairsDownPosition.Y);
            Assert.That(playerToStairsDistance, Is.GreaterThanOrEqualTo(MapGenerator.MinimumPlayerStairsDistance));
        }
    }
}