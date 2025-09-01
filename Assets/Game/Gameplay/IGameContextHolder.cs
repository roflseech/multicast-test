namespace Game.Gameplay
{
    public interface IGameContextHolder
    {
        IGameContext Context { get; }
    }
}