using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Game.AssetManagement
{
    public class SpriteProvider : ISpriteProvider
    {
        private readonly IAssetProvider _assetProvider;
        
        public SpriteProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public IObservable<Sprite> GetSpriteAsObservable(string path)
        {
            return _assetProvider.LoadAssetAsObservable<Sprite>(path);
        }
    }
}