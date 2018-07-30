using EscapeToEarth.Ecs;

namespace EscapeToEarth.Ecs.Systems
{
    public interface ISystem
    {
        void Add(Entity e);
        void Update(double elapsedSeconds);
    }
}