using System;
using Game.Gameplay;

namespace Game.UI.GamePresenters.Widgets
{
    public interface IGameWidgetModel
    {
        IObservable<GameState> State { get; }
        IObservable<GameParams> CurrentGame { get; }
    }
    
    public class GameWidgetModel : IGameWidgetModel
    {
        public IObservable<GameState> State { get; }
        public IObservable<GameParams> CurrentGame { get; }

        void SetGame(GameParams gameParams)
        {
            
        }
    }
}