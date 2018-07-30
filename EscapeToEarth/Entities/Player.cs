using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs;

namespace EscapeToEarth.Entities
{
    class Player : Entity
    {
        internal static Player Instance = new Player();
        // How many tiles we see around us. 5 means five tiles radius.
        internal const int FovRadius = 5;

        private Player()
        {
            this.Set(new MoveToKeyboardComponent(this));
        }
    }
}