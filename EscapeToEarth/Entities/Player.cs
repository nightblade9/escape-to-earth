using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs;
using Microsoft.Xna.Framework;

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
            this.Set(new DisplayComponent(this, '@', Color.White));

            // Do not check in
            EventBus.Instance.Register<Player>("Player moved", (player) => System.Console.WriteLine($"Player is at {player.Position.X}, {player.Position.Y}"));
        }
    }
}