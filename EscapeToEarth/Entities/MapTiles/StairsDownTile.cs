namespace EscapeToEarth.Entities.MapTiles
{
    public class StairsDownTile : AbstractMapTile
    {
        override public char FloorCharacter { get; set; } = '>';
        override public bool IsWalkable { get; set; } = true;
    }
}