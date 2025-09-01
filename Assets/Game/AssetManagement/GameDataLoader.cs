using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.AssetManagement
{
    public interface IGameDataLoader
    {
        UniTask LoadGame(string gameName);
        void UnloadGame(string gameName);
        GameObject GetGame(string gameName);
    }

    public class GameDataLoader : IGameDataLoader
    {
        private readonly IAssetProvider _assetProvider;
        
        public GameDataLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask LoadGame(string gameName)
        {
            await _assetProvider.PreloadAsset<GameObject>(PathForGame(gameName));
        }

        public void UnloadGame(string gameName)
        {
            _assetProvider.UnloadAsset(PathForGame(gameName));
        }

        public GameObject GetGame(string gameName)
        {
            return _assetProvider.GetAsset<GameObject>(PathForGame(gameName));
        }

        private string PathForGame(string gameName)
        {
            return "Games/" + gameName + ".prefab";
        }
    }
}