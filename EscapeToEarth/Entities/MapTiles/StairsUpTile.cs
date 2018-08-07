namespace EscapeToEarth.Entities.MapTiles
{
    public class StairsUpTile : AbstractMapTile
    {
        override public char FloorCharacter { get; set; } = '<';
        override public bool IsWalkable { get; set; } = true;
    }
}