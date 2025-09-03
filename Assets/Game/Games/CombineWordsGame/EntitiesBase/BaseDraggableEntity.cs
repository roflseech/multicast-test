using Game.Games.CombineWordsGame.Common;
using Game.Games.CombineWordsGame.Flow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using VContainer;
using UniRx;

namespace Game.Games.CombineWordsGame.EntitiesBase
{
    public class BaseDraggableEntity : BaseEntity, IPointerDownHandler
    {
        [SerializeField] private DraggableUiElement _draggable;
        
        private CombinedWordsGameParams _gameParams;
        
        private bool _isDragging;
        private bool _isPointerDown;
        private bool _dragBlocked;
        private Vector2 _pointerDownPosition;
        
        private float VerticalDragThreshold =>  (Owner != null && Owner.OwnerType == EntityOwnerType.HorizontalScroll) ? _gameParams.VerticalDragThreshold : 0.0f;
        private float HorizontalScrollThreshold => (Owner != null && Owner.OwnerType == EntityOwnerType.HorizontalScroll) ? _gameParams.HorizontalScrollThreshold : 10000.0f;
        
        public virtual bool Locked { get; set; }
        
        [Inject]
        public void Construct(CombinedWordsGameParams gameParams)
        {
            _gameParams = gameParams;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pointerDownPosition = Input.mousePosition;
            }
            
            if (Locked)
            {
                _isDragging = false;
                
                return;
            }
            if (_isDragging && !Input.GetMouseButton(0))
            {
                EndDrag();
                return;
            }
            
            if (_isPointerDown && !_isDragging && !_dragBlocked)
            {
                Vector2 currentPosition = Input.mousePosition;
                float verticalDistance = Mathf.Abs(currentPosition.y - _pointerDownPosition.y);
                float horizontalDistance = Mathf.Abs(currentPosition.x - _pointerDownPosition.x);
                
                if (horizontalDistance > HorizontalScrollThreshold)
                {
                    _dragBlocked = true;
                }
                else if (verticalDistance > VerticalDragThreshold)
                {
                    StartDragging();
                }
            }
            
            if (_isPointerDown && !Input.GetMouseButton(0))
            {
                _isPointerDown = false;
                _dragBlocked = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
            _dragBlocked = false;
        }
        
        private void StartDragging()
        {
            _isDragging = true;
            _draggable.FollowMouse = true;
            
            var scrollRect = GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.enabled = false;
            }
        }
        
        private void EndDrag()
        {
            _isDragging = false;
            _isPointerDown = false;

            ScrollFix();
            
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = ListPool<RaycastResult>.Get();
            bool attached = false;

            EventSystem.current.RaycastAll(pointerEventData, results);
            foreach (var result in results)
            {
                var ownerCandidate = result.gameObject.GetComponent<IEntityOwner>();
                
                if (ownerCandidate == null)
                    continue;

                if (ownerCandidate == Owner)
                {
                    SetOwner(ownerCandidate);
                    attached = true;
                    continue;
                }

                if (ownerCandidate.CanAttach(this))
                {
                    SetOwner(ownerCandidate);
                    attached = true;
                    break;
                }
            }
            
            _draggable.FollowMouse = false;

            if (!attached)
            {
                _draggable.ReturnToOriginalPosition();
                RebuildParentLayout();
            }
            
            ListPool<RaycastResult>.Release(results);

            void ScrollFix()
            {
                var scrollRect = GetComponentInParent<ScrollRect>();
                if (scrollRect != null)
                {
                    scrollRect.enabled = true;
                }
            }
        }
        
        private void RebuildParentLayout()
        {
            if (transform.parent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            }
        }
    }
}