using Microsoft.Xna.Framework;

namespace EscapeToEarth.Ecs.Components
{
    public class DisplayComponent : BaseComponent
    {
        public char Character { get; set; }
        public Color Colour { get; set; } = Color.White;

        public DisplayComponent(Entity parent, char character, Color colour) : base(parent)
        {
            this.Character = character;
            this.Colour= colour;
        }        
    }
}