using System;
using System.Collections.Generic;
using Cysharp.Text;
using Game.Common.UniRXExtensions;
using Game.Common.UnityExtensions;
using Game.Gameplay;
using Game.Games.CombineWordsGame.Entities;
using Game.Games.Common.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Games.CombineWordsGame.Flow
{
    public interface ICombineWordsGameContext : IGameContext
    {
        
    }
    
    public class CombineWordsGameContext : ICombineWordsGameContext
    {
        private readonly ILevelDataParser _levelDataParser;
        private readonly ICombineWordsFactory _combineWordsFactory;
        private readonly CombineWordsEntities _entities;

        private readonly ObservableValue<int> _wordsRemaining = new();
        private readonly ObservableValue<bool> _isReady = new();

        private string _levelCompletionData;
        
        public IObservable<Unit> OnCompleted => _wordsRemaining.Where(x => x == 0).Select(_ => Unit.Default);
        public IReadOnlyObservableValue<bool> IsReady => _isReady;

        
        public CombineWordsGameContext(ILevelDataParser levelDataParser, ICombineWordsFactory combineWordsFactory,
            CombineWordsEntities entities)
        {
            _levelDataParser = levelDataParser;
            _combineWordsFactory = combineWordsFactory;
            _entities = entities;
        }

        public void Setup(GameParams gameParams)
        {
            if (!_isReady.Value)
            {
                Debug.LogError("Game was not properly initialized");
                return;
            }
            
            _entities.RowContainer.ClearObjectsUnderTransform();
            _entities.BottomRepository.Clear();
            
            var levelData = _levelDataParser.Parse<CombineWordsLevelData>(gameParams.LevelData);

            using var sb = new Utf16ValueStringBuilder(true);

            foreach (var word in levelData.TargetWords)
            {
                sb.Append(word);
                sb.Append("\n");
            }
            
            _levelCompletionData = sb.ToString();
            
            for (int i = 0; i < levelData.Rows; i++)
            {
                var letterRow = _combineWordsFactory.CreateRow(levelData.Columns, levelData.TargetWords, () =>
                {
                    _wordsRemaining.Value -= 1;
                });
                letterRow.transform.SetParent(_entities.RowContainer, true);
            }

            var clusters = ListPool<WordCluster>.Get();
            foreach (var availableCluster in levelData.AvailableCLusters)
            {
                var wordCluster = _combineWordsFactory.Create(availableCluster);
                clusters.Add(wordCluster);
            }
            
            _entities.BottomRepository.Fill(clusters);

            _wordsRemaining.Value = levelData.TargetWords.Count;
            ListPool<WordCluster>.Release(clusters);
        }

        public void SetReady()
        {
            _isReady.Value = true;
        }

        public string GetLevelCompletionData()
        {
            return _levelCompletionData;
        }
    }
}