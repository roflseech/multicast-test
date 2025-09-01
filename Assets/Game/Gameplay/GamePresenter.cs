using System;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using UnityEngine;
using VContainer;

namespace Game.Gameplay
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;

        private readonly ObservableValue<bool> _isReady = new();
        public IReadOnlyObservableValue<bool> IsReady => _isReady;

        private IGameDataLoader _gameDataLoader;
        private GameParams _currentGameParams = GameParams.Undefined;
        private GameObject _currentGameInstance;
        
        [Inject]
        public void Construct(IGameDataLoader gameDataLoader)
        {
            _gameDataLoader = gameDataLoader;
        }
        
        public async void SetGame(GameParams gameParams)
        {
            if (_currentGameParams.GameName == gameParams.GameName)
            {
                Setup(_currentGameInstance);
                return;
            }

            _isReady.Value = false;
            
            if (_currentGameInstance != null)
            {
                DestroyCurrentGame();
            }

            _currentGameParams = gameParams;
            
            await LoadAndInstantiateGame(gameParams);
        }

        private async UniTask LoadAndInstantiateGame(GameParams gameParams)
        {
            await _gameDataLoader.LoadGame(gameParams.GameName);
            
            var gamePrefab = _gameDataLoader.GetGame(gameParams.GameName);
            if (gamePrefab == null)
            {
                Debug.LogError($"Game prefab not found for: {gameParams.GameName}");
                return;
            }

            _currentGameInstance = Instantiate(gamePrefab, _container);
            Setup(_currentGameInstance);
            
            _isReady.Value = true;
        }

        private void Setup(GameObject gameInstance)
        {
            gameInstance.GetComponent<IGameContextHolder>().Context.Setup(_currentGameParams);
        }

        private void DestroyCurrentGame()
        {
            if (_currentGameInstance != null)
            {
                Destroy(_currentGameInstance);
                _currentGameInstance = null;
            }
            
            if (!string.IsNullOrEmpty(_currentGameParams.GameName))
            {
                _gameDataLoader.UnloadGame(_currentGameParams.GameName);
            }
        }

        private void OnDestroy()
        {
            DestroyCurrentGame();
            _isReady.Value = false;
        }
    }
}