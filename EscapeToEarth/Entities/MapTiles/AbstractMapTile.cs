namespace EscapeToEarth.Entities.MapTiles
{
    public abstract class AbstractMapTile
    {
        // Used to implement "fog of war": undiscovered tiles don't show, discovered ones render dark when out of sight
        public bool IsDiscovered { get; set; } = false;

        public virtual bool IsWalkable { get; set; } = false;

        public virtual char FloorCharacter { get; set; } = '.';
    }
}