using EscapeToEarth.Ecs;
using EscapeToEarth.Generators;
using EscapeToEarth.UnitTest.Helpers;
using GoRogue;
using GoRogue.MapViews;
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
        // This test occassionally takes >= 10 seconds. That's okay. It's not flaky this way.
        public void GenerateFloorGeneratesMapWithWalkablePathFromPlayerToStairsAndStairsIsSufficientlyFarAway()
        {
            // Generate map
            // Get the player and stairs position
            // Path-find (BFS) and make sure we can reach
            // verify the distance is large enough

            // CRITICAL: make sure the map is big enough to position the player far from stairs.
            var mapData = MapGenerator.GenerateFloor(1, 20, 20);

            var map = mapData.Map;
            var playerPosition = mapData.PlayerPosition;
            var stairsDownPosition = mapData.StairsDownPosition;

            // True = walkable
            Assert.That(map[playerPosition.X, playerPosition.Y], Is.True);
            Assert.That(map[stairsDownPosition.X, stairsDownPosition.Y], Is.True);

            // Path-find! This could take a while ...
            var positionsToExplore = new List<Coord>();
            positionsToExplore.Add(playerPosition);

            var exploredPositions = new List<Coord>();

            Coord currentPosition = null;
            DateTime start = DateTime.Now;

            while (positionsToExplore.Any() && currentPosition != stairsDownPosition)
            {
                currentPosition = positionsToExplore.ElementAt(0);
                positionsToExplore.Remove(currentPosition);
                exploredPositions.Add(currentPosition);

                this.AddIfWalkableAndNeverChecked(map, currentPosition.X - 1, currentPosition.Y, exploredPositions, positionsToExplore);
                this.AddIfWalkableAndNeverChecked(map, currentPosition.X + 1, currentPosition.Y, exploredPositions, positionsToExplore);
                this.AddIfWalkableAndNeverChecked(map, currentPosition.X, currentPosition.Y - 1, exploredPositions, positionsToExplore);
                this.AddIfWalkableAndNeverChecked(map, currentPosition.X, currentPosition.Y + 1, exploredPositions, positionsToExplore);
            }

            Assert.That(currentPosition, Is.EqualTo(stairsDownPosition), "Couldn't find a way from current position to stairs");

            var playerToStairsDistance = Math.Abs(playerPosition.X - stairsDownPosition.X) + Math.Abs(playerPosition.Y - stairsDownPosition.Y);
            Assert.That(playerToStairsDistance, Is.GreaterThanOrEqualTo(MapGenerator.MinimumPlayerStairsDistance));
        }

        public void AddIfWalkableAndNeverChecked(ArrayMap<bool> map, int x, int y,  List<Coord> exploredPositions,  List<Coord> positionsToExplore)
        {
            if (x >= 0 && x < map.Width && y >= 0 && y < map.Height && map[x, y] == true && !exploredPositions.Any(c => c.X == x && c.Y == y))
            {
                positionsToExplore.Add(Coord.Get(x, y));
            }
        }
    }
}