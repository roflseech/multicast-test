using System;
using Game.DomainLogic;
using Game.Gameplay;
using Game.SaveSystem;
using Game.State;
using Game.UI.GameLayers;
using Game.UI.GameModels.Widgets;
using Game.UI.Models.Window;

namespace Game.UI.GameModels.Windows
{
    public interface IHomeWindowModel : IWindowModel
    {
        IButtonWithTextWidgetModel PlayButton { get; }
        ITextWidgetModel GamesPlayed { get; }
    }
    
    public class HomeWindowModel : IHomeWindowModel
    {
        private const string GAMES_PLAYED_TEXT = "games_played";
        private const string START_GAME_TEXT = "start_game";
        
        private readonly IReadOnlySaveData<PlayerStatistics> _playerStatistics;
        private readonly IUiAggregate _uiAggregate;
        private readonly ILevelSelector _levelSelector;
            
        private IButtonWithTextWidgetModel _playButton;
        
        public IButtonWithTextWidgetModel PlayButton => _playButton ??= 
            new ButtonWithTextWidgetModel(
                new TextWidgetModel(START_GAME_TEXT, true),
                () =>
                {
                    _levelSelector.SetLastAvailableLevel();
                    _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IBaseGameplayWindowModel>();
                });
        
        public ITextWidgetModel GamesPlayed =>  new TextWidgetModel(GAMES_PLAYED_TEXT, true, _playerStatistics.Value.LevelsCompleted.ToString());
        
        public HomeWindowModel(IReadOnlySaveData<PlayerStatistics> playerStatistics, IUiAggregate uiAggregate, 
            ILevelSelector levelSelector)
        {
            _playerStatistics = playerStatistics;
            _uiAggregate = uiAggregate;
            _levelSelector = levelSelector;
        }
        
        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}