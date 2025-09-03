using System.Threading;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.VContainer;
using Game.Gameplay;
using Game.Games.Common.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Games.CombineWordsGame.Flow
{
    public class CombineWordsGameInstaller : LifetimeScope, IGameContextHolder
    {
        [SerializeField] private CombineWordsEntities _entities = new();
        [SerializeField] private CombineWordsAssets _combineWordsAssets;
        [SerializeField] private CombinedWordsGameParams _gameParams;
        
        public IGameContext Context => Container.Resolve<IGameContext>();

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(_entities);
            
            builder.BindSingleton<JsonLevelDataParser>();
            builder.BindSingleton<CombineWordsFactory>();
            builder.BindSingleton<CombineWordsGameContext>();
            builder.RegisterComponent(_combineWordsAssets);
            builder.RegisterComponent(_gameParams);
            builder.BindSingleton<AddressablesAssetProvider>();

            builder.RegisterEntryPoint<EntryPoint>();
        }

        private class EntryPoint : IAsyncStartable
        {
            private readonly IGameContext _gameContext;
            private readonly IAssetProvider _assetProvider;
            private readonly CombineWordsAssets _combineWordsAssets;

            public EntryPoint(IGameContext gameContext, IAssetProvider assetProvider, CombineWordsAssets combineWordsAssets)
            {
                _gameContext = gameContext;
                _assetProvider = assetProvider;
                _combineWordsAssets = combineWordsAssets;
            }

            public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
            {
                await _assetProvider.PreloadAsset<GameObject>(_combineWordsAssets.WordClusterPrefabPath);
                await _assetProvider.PreloadAsset<GameObject>(_combineWordsAssets.LetterRowPrefabPath);
                _gameContext.SetReady();
            }
        }
    }
}