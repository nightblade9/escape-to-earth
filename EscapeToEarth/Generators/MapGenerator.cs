using EscapeToEarth.Ecs;
using EscapeToEarth.Entities.MapTiles;
using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using System;

namespace EscapeToEarth.Generators
{
    public static class MapGenerator
    {
        // TODO:  make this Internal and use [assembly:InternalsVisibleTo(...)]
        public const int MinimumPlayerStairsDistance = 5;

        public static MapData GenerateFloor(uint floorNumber, int widthInTiles, int heightInTiles)
        {
            // Generate a walkable map, complete with stairs down

            var isWalkableMap = new ArrayMap<bool>(widthInTiles, heightInTiles);
            CellularAutomataGenerator.Generate(isWalkableMap);

            // Randomly positioned on a ground tile! True = walkable
            var playerPosition = isWalkableMap.RandomPosition(true);

            var stairsDownPosition = playerPosition;
            while (Math.Abs(playerPosition.X - stairsDownPosition.X) + Math.Abs(playerPosition.Y - stairsDownPosition.Y) <= MinimumPlayerStairsDistance)
            {
                stairsDownPosition = isWalkableMap.RandomPosition(true);
            }

            System.Console.WriteLine($"Stairs are at {stairsDownPosition}");
            var mapData = new MapData() { Map = isWalkableMap, PlayerPosition = playerPosition, StairsDownPosition = stairsDownPosition };
            
            return mapData;
        }
    }
}