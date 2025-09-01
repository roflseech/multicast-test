using System.Threading;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.UI.GameLayers;
using Game.UI.GameModels.Windows;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.AppFlow.EntryPoints
{
    public class BootstrapEntryPoint : LifetimeScope
    {
        private async void Start()
        {
            var root = VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();

            if (root is RootInstaller rootInstaller)
            {
                await rootInstaller.InitializeAsync();
            }
            
            Build();
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<EntryPoint>();
        }

        private class EntryPoint : IAsyncStartable
        {
            private readonly ICoreInitializer _coreInitializer;
            private readonly IUiAggregate _uiAggregate;
            private readonly ILevelLoader _levelLoader;
            
            public EntryPoint(ICoreInitializer coreInitializer, IUiAggregate uiAggregate, ILevelLoader levelLoader)
            {
                _coreInitializer = coreInitializer;
                _uiAggregate = uiAggregate;
                _levelLoader = levelLoader;
            }
        
            public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
            {
                await _coreInitializer.InitAsync();
                await _levelLoader.UpdateDatabaseAsync();
                await _uiAggregate.Get(UiLayer.Main).PreloadAllWindows();
                _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>();
            }
        }
    }
}