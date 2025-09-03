using Game.DomainLogic;
using Game.UI.GameLayers;
using Game.UI.GameModels.Widgets;
using Game.UI.Models.Window;

namespace Game.UI.GameModels.Windows
{
    public interface IWinWindowModel : IWindowModel
    {
        IButtonWithTextWidgetModel NextLevelButton { get; }
        IButtonWithTextWidgetModel MainMenuButton { get; }
        ITextWidgetModel TitleText { get; }
        ITextWidgetModel CompletionInfoText { get; }
    }
    
    public class WinWindowModel : IWinWindowModel
    {
        private const string WIN_WINDOW_TITLE = "win_window_title";
        private const string NEXT_LEVEL_TEXT = "next_level";
        private const string MAIN_MENU_TEXT = "main_menu";

        private readonly ILevelSelector _levelSelector;
        private readonly IUiAggregate _uiAggregate;
        
        private readonly string _levelCompletionData;
        
        public WinWindowModel(string levelCompletionData, ILevelSelector levelSelector, IUiAggregate uiAggregate)
        {
            _levelCompletionData = levelCompletionData;
            _levelSelector = levelSelector;
            _uiAggregate = uiAggregate;
        }

        public IButtonWithTextWidgetModel NextLevelButton =>  new ButtonWithTextWidgetModel(new TextWidgetModel(NEXT_LEVEL_TEXT, true),
            () =>
            {
                _levelSelector.SetLastAvailableLevel();
                _uiAggregate.Get(UiLayer.Popup).CloseWindow();
            });
        
        public IButtonWithTextWidgetModel MainMenuButton => new ButtonWithTextWidgetModel(new TextWidgetModel(MAIN_MENU_TEXT, true),
            () =>
            {
                _uiAggregate.Get(UiLayer.Popup).CloseWindow();
                _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>();
            });

        public ITextWidgetModel TitleText => new TextWidgetModel(WIN_WINDOW_TITLE, true);
        public ITextWidgetModel CompletionInfoText => new TextWidgetModel(_levelCompletionData, false);

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}