using UnityEngine;

namespace Game.Games.CombineWordsGame.EntitiesBase
{
    public class BaseEntity : MonoBehaviour, IEntity
    {
        public IEntityOwner Owner { get; protected set; }
        public Transform Transform => transform;
        
        public void SetOwner(IEntityOwner owner)
        {
            if (Owner == owner)
            {
                owner.Reattach(this);
                return;
            }
            
            if (owner != null && !owner.CanAttach(this))
            {
                return;
            }
            
            Owner?.Detach(this);
            Owner = owner;
            Owner?.Attach(this);
        }
    }
}