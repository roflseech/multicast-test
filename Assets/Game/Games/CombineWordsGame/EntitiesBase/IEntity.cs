using UnityEngine;

namespace Game.Games.CombineWordsGame.EntitiesBase
{
    public interface IEntity
    {
        IEntityOwner Owner { get; }
        public Transform Transform { get; }
        void SetOwner(IEntityOwner owner);
    }
}