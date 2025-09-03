using System;
using System.Collections.Generic;
using Game.Common.UnityExtensions;
using Game.Games.CombineWordsGame.EntitiesBase;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Games.CombineWordsGame.Entities
{
    public class EntityRepository : MonoBehaviour, IEntityOwner
    {
        [SerializeField] private Transform _container;
        
        public EntityOwnerType OwnerType => EntityOwnerType.HorizontalScroll;
        
        public void Detach(IEntity entity)
        {
            entity.Transform.SetParent(null);
        }

        public bool CanAttach(IEntity entity)
        {
            return true;
        }

        public void Reattach(IEntity entity)
        {
            
        }

        public void Attach(IEntity entity)
        {
            entity.Transform.SetParent(_container, false);
            entity.Transform.localScale = Vector3.one;
            UpdateLayout();
        }

        public void Fill(IReadOnlyList<IEntity> elements)
        {
            foreach (var element in elements)
            {
                element.SetOwner(this);
            }

            UpdateLayout();
        }

        private void UpdateLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
        }

        public void Clear()
        {
            _container.ClearObjectsUnderTransform();
        }
    }
}