using EscapeToEarth.Ecs.Components;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using System.Collections.Generic;

namespace EscapeToEarth.Ecs.Components
{
    /// <summary>
    /// Moves an entity's X/Y coordinates by one (discretely) if the user pressed arrow keys.
    // TODO: WASD support
    /// </summary>
    public class MoveToKeyboardComponent : BaseComponent
    {
        public MoveToKeyboardComponent(Entity parent) : base(parent)
        {
        }

        public void Update(IList<AsciiKey> keysDown)
        {
            var dx = 0;
            var dy = 0;

            if (keysDown.Contains(AsciiKey.Get(Keys.Left)) || keysDown.Contains(AsciiKey.Get(Keys.A)))
            {
                dx = -1;
            }
            else if (keysDown.Contains(AsciiKey.Get(Keys.Right)) || keysDown.Contains(AsciiKey.Get(Keys.D)))
            {
                dx = 1;
            }

            if (keysDown.Contains(AsciiKey.Get(Keys.Up)) || keysDown.Contains(AsciiKey.Get(Keys.W)))
            {
                dy = -1;
            }
            else if (keysDown.Contains(AsciiKey.Get(Keys.Down)) || keysDown.Contains(AsciiKey.Get(Keys.S)))
            {
                dy = 1;
            }

            if (dx != 0 || dy != 0)
            {
                var position = this.Parent.Position;
                position.X += dx;
                position.Y += dy;
            }
        }
    }
}