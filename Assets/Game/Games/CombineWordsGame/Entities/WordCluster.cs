using System;
using Game.Games.CombineWordsGame.Common;
using Game.Games.CombineWordsGame.EntitiesBase;
using Game.Games.CombineWordsGame.Flow;
using Game.Games.CombineWordsGame.Visual;
using UnityEngine;
using VContainer;
using UniRx;
using UnityEngine.UI;

namespace Game.Games.CombineWordsGame.Entities
{
    public class WordCluster : BaseDraggableEntity
    {
        [SerializeField] private LetterSequence _letterSequence;
        [SerializeField] private Image _border;

        public override bool Locked
        {
            get => base.Locked;
            set
            {
                _border.color = value ? Color.green : Color.black;
                base.Locked = value;
            }
        }
        public string Letters { get; private set; }

        public Vector3 LeftAnchorPoint => _letterSequence.GetLetterPosition(0);
        
        public void SetText(string text)
        {
            Letters = text;
            _letterSequence.SetLetters(text);
        }
    }
}