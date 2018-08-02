using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs;

namespace EscapeToEarth.Entities
{
    public class Player : Entity
    {
        /// <summary>
        /// How many tiles we see around us. 5 means five tiles radius.
        /// </summary>
        internal const int FovRadius = 5;

        public Player()
        {
            this.Set(new MoveToKeyboardComponent(this));
        }
    }
}