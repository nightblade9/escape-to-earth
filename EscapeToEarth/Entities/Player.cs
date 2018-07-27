using EscapeToEarth.Ecs;

namespace EscapeToEarth.Entities
{
    class Player : Entity
    {
        // How many tiles we see around us. 5 means five tiles radius.
        internal const int FovRadius = 5;
        // TODO: move into base entitiy class
    }
}