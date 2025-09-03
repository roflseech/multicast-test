using System;
using System.Collections.Generic;
using Game.AssetManagement;
using Game.Games.CombineWordsGame.Entities;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Game.Games.CombineWordsGame.Flow
{
    public interface ICombineWordsFactory
    {
        WordCluster Create(string clusterText);
        LetterRow CreateRow(int length, IReadOnlyList<string> targetWords, Action onLock);
    }

    public class CombineWordsFactory : ICombineWordsFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IAssetProvider _assetProvider;
        private readonly CombineWordsAssets _combineWordsAssets;
        private readonly CombineWordsEntities _combineWordsEntities;
        
        public CombineWordsFactory(IAssetProvider assetProvider, CombineWordsAssets combineWordsAssets, 
            IObjectResolver resolver, CombineWordsEntities combineWordsEntities)
        {
            _assetProvider = assetProvider;
            _combineWordsAssets = combineWordsAssets;
            _resolver = resolver;
            _combineWordsEntities = combineWordsEntities;
        }

        public WordCluster Create(string clusterText)
        {
            var prefab = _assetProvider.GetAsset<GameObject>(_combineWordsAssets.WordClusterPrefabPath);
            var instance = Object.Instantiate(prefab);
            _resolver.InjectGameObject(instance);
            var wordCluster = instance.GetComponent<WordCluster>();
            
            wordCluster.SetText(clusterText);
            
            return wordCluster;
        }

        public LetterRow CreateRow(int length, IReadOnlyList<string> targetWords, Action onLock)
        {
            var prefab = _assetProvider.GetAsset<GameObject>(_combineWordsAssets.LetterRowPrefabPath);
            var instance = Object.Instantiate(prefab);
            _resolver.InjectGameObject(instance);
            var letterRow = instance.GetComponent<LetterRow>();
            
            letterRow.Initialize(length, targetWords, onLock, _combineWordsEntities.ElementsContainer);
            
            return letterRow;
        }
    }
}