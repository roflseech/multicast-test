using System;
using System.Collections.Generic;
using System.Linq;
using Game.Games.CombineWordsGame.EntitiesBase;
using Game.Games.CombineWordsGame.Visual;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Text;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Games.CombineWordsGame.Entities
{
    public class LetterRow : MonoBehaviour, IEntityOwner
    {
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private GameObject _slotTemplate;

        private Transform _insideElementsRoot;
        private int _slotCount;
        private List<Entry> _entries = new();
        private List<RectTransform> _slots = new();
        private IReadOnlyList<string> _targetWords;

        private Action _onLock;
        
        private struct Entry
        {
            public int Pos;
            public WordCluster Entity;
        }

        public EntityOwnerType OwnerType => EntityOwnerType.Base;
        
        public void Initialize(int length, IReadOnlyList<string> targetWords, Action onLock, Transform insideElementsRoot)
        {
            _onLock = onLock;
            _targetWords = targetWords;
            _slotCount = length;
            _insideElementsRoot = insideElementsRoot;
            CreateSlots();
        }
        
        private void CreateSlots()
        {
            foreach (var slot in _slots)
            {
                if (slot != null && slot.gameObject != _slotTemplate)
                    Destroy(slot.gameObject);
            }
            _slots.Clear();
            
            _slots.Add(_slotTemplate.GetComponent<RectTransform>());

            var prev = _slotTemplate;
            for (int i = 1; i < _slotCount; i++)
            {
                var slotInstance = Instantiate(_slotTemplate, _slotsContainer);
                slotInstance.transform.SetSiblingIndex(prev.transform.GetSiblingIndex() + 1);
                var slotRect = slotInstance.GetComponent<RectTransform>();

                _slots.Add(slotRect);
                prev = slotInstance;
            }
        }

        public void Detach(IEntity entity)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                object obj = _entries[i].Entity;
                if (obj == entity)
                {
                    _entries[i].Entity.Transform.parent = null;
                    _entries.RemoveAt(i);
                    return;
                }
            }
        }

        public void Attach(IEntity entity)
        {
            if (entity is not WordCluster wordCluster)
            {
                Debug.LogError("Attempt to attach not supported type");
                return;
            }

            var index = FindBestIndexToPlace(wordCluster);
            if (index < 0)
            {
                Debug.LogError("Attempt to attach without check");
                return;
            }
            
            _entries.Add(new Entry() { Entity = wordCluster, Pos = index });
            entity.Transform.parent = _insideElementsRoot;
            
            var delta = wordCluster.transform.position - GetClusterLeftPos(wordCluster);
            entity.Transform.position = _slots[index].transform.position + delta;

            if (CheckWords())
            {
                Lock();
            }
        }
        
        public void Reattach(IEntity entity)
        {
            Detach(entity);
            Attach(entity);
        }
        
        private int FindBestIndexToPlace(WordCluster wordCluster)
        {
            int closestSlot = -1;
            float minDistance = 0.0f;
            
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!CanPlace(i, wordCluster.Letters.Length)) continue;
                
                var dist = Vector2.Distance(_slots[i].transform.position, GetClusterLeftPos(wordCluster));
                if (closestSlot == -1 || dist < minDistance)
                {
                    closestSlot = i;
                    minDistance = dist;
                }
            }

            return closestSlot;
        }
        
        private bool CanPlace(int index, int length)
        {
            if (index + length - 1 >= _slots.Count) return false;
            int startIndex = index;
            int endIndex = index + length - 1;

            foreach (var entry in _entries)
            {
                var entryStartIndex = entry.Pos;
                var entryEndIndex = entry.Pos + entry.Entity.Letters.Length - 1;

                var l = Mathf.Max(entryStartIndex, startIndex);
                var r = Mathf.Min(entryEndIndex, endIndex);
                if (l <= r)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public bool CanAttach(IEntity entity)
        {
            if (entity is not WordCluster wordCluster)
            {
                return false;
            }

            return FindBestIndexToPlace(wordCluster) >= 0;
        }

        private bool CheckWords()
        {
            string currentWord = GetCurrentWord();
            if (string.IsNullOrEmpty(currentWord))
                return false;
                
            return _targetWords.Contains(currentWord);
        }
        
        private string GetCurrentWord()
        {
            if (_entries.Count == 0)
                return "";
            
            _entries.Sort((a, b) => a.Pos.CompareTo(b.Pos));
            
            using var sb = ZString.CreateStringBuilder();
            int expectedPosition = 0;
            
            foreach (var entry in _entries)
            {
                if (entry.Pos > expectedPosition)
                {
                    return "";
                }
                
                sb.Append(entry.Entity.Letters);
                expectedPosition = entry.Pos + entry.Entity.Letters.Length;
            }
            
            return sb.ToString();
        }

        private void Lock()
        {
            _onLock?.Invoke();
            foreach (var entry in _entries)
            {
                entry.Entity.Locked = true;
            }
        }

        private Vector3 GetClusterLeftPos(WordCluster cluster)
        {
            return cluster.LeftAnchorPoint;
        }
    }
}