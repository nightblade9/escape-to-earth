using GoRogue;

namespace EscapeToEarth.Entities
{
    class Player
    {
        // How many tiles we see around us. 5 means five tiles radius.
        internal const int FovRadius = 5;
        // TODO: move into base entitiy class
        internal GoRogue.Coord Position { get; set; }

        public Player()
        {
            this.Position = Coord.Get(0, 0);
        }
    }
}