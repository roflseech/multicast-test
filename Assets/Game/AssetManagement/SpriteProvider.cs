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
        
       /* public async UniTask<Sprite> GetSprite(string path)
        {
            var state = _assetProvider.GetAssetState(path);
            
            if (state == AssetState.Loaded)
            {
                return _assetProvider.GetAsset(path) as Sprite;
            }
            
            if (state == AssetState.NotLoaded)
            {
                await _assetProvider.PreloadAsset(path);
                return _assetProvider.GetAsset(path) as Sprite;
            }
            
            await _assetProvider.AssetLoaded.Where(loadedPath => loadedPath == path).Take(1);
            return _assetProvider.GetAsset(path) as Sprite;
        }*/

        public IObservable<Sprite> GetSpriteAsObservable(string path)
        {
            var state = _assetProvider.GetAssetState(path);

            if (state == AssetState.Loaded)
            {
                return Observable.Return(_assetProvider.GetAsset<Sprite>(path));
            }

            if (state == AssetState.NotLoaded)
            {
                _assetProvider.PreloadAsset<Sprite>(path).Forget();
            }

            return _assetProvider.AssetLoaded
                .Where(loadedPath => loadedPath == path)
                .Take(1)
                .Select(_ => _assetProvider.GetAsset<Sprite>(path));
        }
    }
}