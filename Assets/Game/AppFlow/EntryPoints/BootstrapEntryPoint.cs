using System.Threading;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.SaveSystem;
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

            if (root != null && root is RootInstaller rootInstaller)
            {
                await rootInstaller.InitializeAsync();
            }
            else
            {
                Debug.LogError("Root is not installed");
                return;
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
            private readonly ISaveDataManager _saveDataManager;
            
            public EntryPoint(IUiAggregate uiAggregate, ILevelLoader levelLoader, ISaveDataManager saveDataManager)
            {
                _uiAggregate = uiAggregate;
                _levelLoader = levelLoader;
                _saveDataManager = saveDataManager;
            }
        
            public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
            {
                Application.targetFrameRate = 60;
                _saveDataManager.Initialize();
                await _saveDataManager.LoadAllDataAsync();
                await _levelLoader.UpdateDatabaseAsync();
                await _uiAggregate.Get(UiLayer.Main).PreloadAllWindows();
                await _uiAggregate.Get(UiLayer.Popup).PreloadAllWindows();
                _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>();
            }
        }
    }
}