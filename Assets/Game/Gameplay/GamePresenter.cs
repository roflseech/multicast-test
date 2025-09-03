using System;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.Gameplay
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;

        private readonly ObservableValue<bool> _isReady = new();
        private readonly ObservableEvent _onCompleted = new();
        
        private readonly CompositeDisposable _disposable = new();
        
        private IGameDataLoader _gameDataLoader;
        private GameParams _currentGameParams = GameParams.Undefined;
        private GameObject _currentGameInstance;
        private string _levelCompletionData;
        public IReadOnlyObservableValue<bool> IsReady => _isReady;
        public IObservable<Unit> OnCompleted => _onCompleted;

        public string GetLevelCompletionData()
        {
            return _levelCompletionData;
        }
        
        [Inject]
        public void Construct(IGameDataLoader gameDataLoader)
        {
            _gameDataLoader = gameDataLoader;
        }
        
        public void SetGame(GameParams gameParams)
        {
            if (_currentGameParams.GameName == gameParams.GameName)
            {
                Setup(_currentGameInstance);
                return;
            }

            _disposable.Clear();
            _isReady.Value = false;
            
            if (_currentGameInstance != null)
            {
                DestroyCurrentGame();
            }

            _currentGameParams = gameParams;
            
            LoadAndInstantiateGame(gameParams).Forget();
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

            var contextHolder = _currentGameInstance.GetComponent<IGameContextHolder>();
            var context = contextHolder.Context;
            await context.IsReady.Where(x => x).ToUniTask(true);
            
            Setup(_currentGameInstance);
            
            context.OnCompleted.Subscribe(_ =>
            {
                _levelCompletionData = context.GetLevelCompletionData();
                _onCompleted.Invoke();
            }).AddTo(_disposable);
            
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
            _disposable.Dispose();
        }
    }
}