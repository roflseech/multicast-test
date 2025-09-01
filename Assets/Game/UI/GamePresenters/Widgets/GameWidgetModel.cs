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
    }
    
    public class GameWidgetModel : IGameWidgetModel
    {
        private readonly ObservableValue<GameState> _state = new();
        private readonly ObservableValue<GameParams> _currentGame = new(GameParams.Undefined);
        public IObservable<GameState> State => _state;
        public IObservable<GameParams> CurrentGame => _currentGame.Where(x => x.IsValid());

        public void SetGame(GameParams gameParams)
        {
            _currentGame.Value = gameParams;
        }
    }
}