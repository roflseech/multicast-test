namespace Game.Games.CombineWordsGame.EntitiesBase
{
    public enum EntityOwnerType { Base, HorizontalScroll }
    public interface IEntityOwner
    {
        void Detach(IEntity entity);
        void Attach(IEntity entity);
        bool CanAttach(IEntity entity);
        void Reattach(IEntity entity);
        EntityOwnerType OwnerType { get; }
    }
}