using GoRogue;
using GoRogue.MapViews;

namespace EscapeToEarth.Generators
{
    public class MapData
    {
        public ArrayMap<bool> Map { get; set; }

        // Player position AND stairs-down position
        public Coord PlayerPosition { get; set; }
        public Coord StairsDownPosition { get; set; }
    }
}