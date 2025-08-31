using System;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.VContainer;
using Game.Configs.UI;
using Game.DomainLogic;
using Game.Localization;
using Game.UI.GameLayers;
using Game.UI.Provider;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Game.AppFlow
{
    public class RootInstaller : LifetimeScope
    {
        private const string CONFIG_TAG = "Configs";
        
        [SerializeField] private UiInstallerParameters _uiInstallerParameters = new();

        private AddressablesAssetProvider _configProvider;
        
        public async UniTask InitializeAsync()
        {
            _configProvider = new AddressablesAssetProvider();
            await _configProvider.PreloadByTag(CONFIG_TAG);
            Build();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.BindSingleton<CoreInitializer>();
            
            InstallUI(builder);
            InstallResourceManagement(builder);
            InstallConfigs(builder);
            InstallState(builder);
            InstallDomainLogic(builder);
            
            builder.BindSingleton<PlaceholderLocalizationProvider>();
        }

        private void InstallUI(IContainerBuilder builder)
        {
            UiInstaller.Install(builder, _uiInstallerParameters);
        }
        
        private void InstallResourceManagement(IContainerBuilder builder)
        {
            builder.BindSingleton<AddressablesAssetProvider>();
            builder.BindSingleton<SpriteProvider>();
        }
        
        private void InstallConfigs(IContainerBuilder builder)
        {
            RegisterConfig<UiWindowsPaths>("Configs/UiWindowsConfig.asset");

            void RegisterConfig<T>(string path) where T : Object 
            {
                var config = _configProvider.GetAsset<T>(path);
                builder.RegisterInstance(config).As<T>();
            }
        }

        private void InstallState(IContainerBuilder builder)
        {
            StateInstaller.Install(builder);
        }

        private void InstallDomainLogic(IContainerBuilder builder)
        {
            builder.BindSingleton<LevelSelector>();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _configProvider?.Dispose();
        }
    }
}