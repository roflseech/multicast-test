using System;
using Game.Common.UniRXExtensions;
using Game.Gameplay;
using UniRx;

namespace Game.UI.GamePresenters.Widgets
{
    public interface IGameWidgetModel
    {
        IObservable<GameState> State { get; }
        IObservable<GameParams> CurrentGame { get; }
        IObservable<Unit> OnCompleted { get; }
        void NotifyCompleted(string levelCompletionData);
        string GetLevelCompletionData();
    }
    
    public class GameWidgetModel : IGameWidgetModel
    {
        private readonly ObservableValue<GameState> _state = new();
        private readonly ObservableValue<GameParams> _currentGame = new(GameParams.Undefined);
        private readonly ObservableEvent _onCompleted = new();

        private string _levelCompletionData;
        
        public IObservable<GameState> State => _state;
        public IObservable<GameParams> CurrentGame => _currentGame.Where(x => x.IsValid());
        public IObservable<Unit> OnCompleted => _onCompleted;
        
        public void NotifyCompleted(string levelCompletionData)
        {
            _levelCompletionData = levelCompletionData;
            _onCompleted.Invoke();
        }

        public void SetGame(GameParams gameParams)
        {
            _currentGame.Value = gameParams;
        }

        public string GetLevelCompletionData()
        {
            return _levelCompletionData;
        }
    }
}