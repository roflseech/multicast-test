using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Game.Games.CombineWordsGame.Common
{
    public class DraggableUiElement : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<MaskableGraphic> _maskables = new();
        
        private bool _followMouse;
        private bool _offsetCalculated;
        private Vector2 _dragOffset;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;

        
        public bool FollowMouse
        {
            get => _followMouse;
            set
            {
                if (_followMouse != value)
                {
                    _followMouse = value;
                    if (_followMouse)
                    {
                        _originalPosition = _rectTransform.position;
                        Vector2 mousePosition = Input.mousePosition;
                        _dragOffset = (Vector2)_rectTransform.position - mousePosition;
                        _offsetCalculated = true;
                        
                        _canvasGroup.blocksRaycasts = false;
                        RefreshMaskables();
                        foreach (var maskable in _maskables)
                        {
                            maskable.maskable = false;
                        }
                    }
                    else
                    {
                        _canvasGroup.blocksRaycasts = true;
                        
                        foreach (var maskable in _maskables)
                        {
                            maskable.maskable = true;
                        }
                    }
                }
            }
        }

        public void ReturnToOriginalPosition()
        {
            _rectTransform.position = _originalPosition;
        }


        public void RefreshMaskables()
        {
            var maskables = ListPool<MaskableGraphic>.Get();
            GetComponentsInChildren<MaskableGraphic>(maskables);
            _maskables.Clear();
            _maskables.AddRange(maskables);
            ListPool<MaskableGraphic>.Release(maskables);
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!_followMouse) return;
            _rectTransform.position = (Vector2)Input.mousePosition + _dragOffset;
        }
    }
}