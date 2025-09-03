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
            private readonly IUiAggregate _uiAggregate;
            private readonly ILevelLoader _levelLoader;
            
            public EntryPoint(IUiAggregate uiAggregate, ILevelLoader levelLoader)
            {
                _uiAggregate = uiAggregate;
                _levelLoader = levelLoader;
            }
        
            public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
            {
                await _levelLoader.UpdateDatabaseAsync();
                await _uiAggregate.Get(UiLayer.Main).PreloadAllWindows();
                await _uiAggregate.Get(UiLayer.Popup).PreloadAllWindows();
                _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>();
            }
        }
    }
}