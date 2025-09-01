using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using Game.Gameplay;
using UniRx;
using UnityEngine;

namespace Game.DomainLogic
{
    public interface ICurrentGame
    {
        IReadOnlyObservableValue<GameParams> Current { get; }
        void SetLevel(string gameName, int level);
    }

    public static class CurrentGameExtensions
    {
        public static IObservable<GameParams> ObserveValid(this ICurrentGame currentGame)
        {
            return currentGame.Current.Where(x => x.IsValid());
        }
    }
    
    public class CurrentGame : ICurrentGame
    {
        private readonly ILevelLoader _levelLoader;
        private readonly ObservableValue<GameParams> _current = new();

        private bool _loading;
        
        public IReadOnlyObservableValue<GameParams> Current => _current;
        
        public CurrentGame(ILevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        public void SetLevel(string gameName, int level)
        {
            if (_loading)
            {
                Debug.LogError("Attempt to load level while already loading");
                return;
            }
            _loading = true;
            _current.Value = GameParams.Undefined;
            SetLevelInternal(gameName, level).Forget();
        }
        
        private async UniTask SetLevelInternal(string gameName, int level)
        {
            var current = _current.Value;
            
            if (current.IsValid())
            {
                _levelLoader.UnloadLevel(current.GameName, current.Level);
            }


            await _levelLoader.LoadLevelAsync(gameName, level);
            var levelData = _levelLoader.GetLevel(gameName, level);
            
            _loading = false;
            _current.Value = new GameParams(gameName, level, levelData);
        }
    }
}