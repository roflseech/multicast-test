using Game.UI.GameModels.Widgets;
using Game.UI.Models.Window;

namespace Game.UI.GameModels.Windows
{
    public interface IWinWindowModel : IWindowModel
    {
        IButtonWithTextWidgetModel NextLevelButton { get; }
        IButtonWithTextWidgetModel MainMenuButton { get; }
        ITextWidgetModel TitleText { get; }
    }
    
    public class WinWindowModel : IWinWindowModel
    {
        private const string WIN_WINDOW_TITLE = "win_window_title";
        private const string NEXT_LEVEL_TEXT = "next_level";
        private const string MAIN_MENU_TEXT = "main_menu";
        
        private IButtonWithTextWidgetModel _nextLevelButton;
        private IButtonWithTextWidgetModel _mainMenuButton;
        private ITextWidgetModel _completedText;
        
        public IButtonWithTextWidgetModel NextLevelButton => _nextLevelButton ??= new ButtonWithTextWidgetModel(new TextWidgetModel(NEXT_LEVEL_TEXT, true),
            () =>
            {
                
            });
        public IButtonWithTextWidgetModel MainMenuButton => _mainMenuButton ??= new ButtonWithTextWidgetModel(new TextWidgetModel(MAIN_MENU_TEXT, true),
            () =>
            {
                
            });

        public ITextWidgetModel TitleText => _completedText ??= new TextWidgetModel(WIN_WINDOW_TITLE, true);

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}