using Game.UI.GameModels.Windows;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Presenters.Window;
using UniRx;
using UnityEngine;

namespace Game.UI.GamePresenters.Windows
{
    public class WinWindow : BaseWindow<IWinWindowModel>
    {
        [SerializeField] private ButtonWithTextWidget _nextLevelButton;
        [SerializeField] private ButtonWithTextWidget _mainMenuButton;
        [SerializeField] private TextWidget _titleText;
        
        protected override void SetBindings(IWinWindowModel model, CompositeDisposable bindings)
        {
            _titleText.Bind(model.TitleText);
            _nextLevelButton.Bind(model.NextLevelButton);
            _mainMenuButton.Bind(model.MainMenuButton);
        }

        protected override void OnWindowOpen()
        {
            
        }

        protected override void OnWindowClose()
        {
            
        }
    }
}