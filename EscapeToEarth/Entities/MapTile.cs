namespace EscapeToEarth.Entities
{
    public class MapTile
    {
        public bool IsWalkable { get; set; } = false;
        // Used to implement "fog of war": undiscovered tiles don't show, discovered ones render dark when out of sight
        public bool IsDiscovered { get; set; } = false;
    }
}