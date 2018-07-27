using EscapeToEarth.Ecs.Components;
using EscapeToEarth.Ecs;

namespace EscapeToEarth.UnitTest.Helpers
{
    class StringComponent : BaseComponent
    {
        public string Value { get; set; }
        public StringComponent(Entity parent) : base(parent)
        {
        }
    }
}