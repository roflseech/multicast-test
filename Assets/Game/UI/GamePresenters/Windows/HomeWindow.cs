using Game.UI.Common.UiElements;
using Game.UI.GameModels.Windows;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Presenters.Window;
using UniRx;
using UnityEngine;

namespace Game.UI.GamePresenters.Windows
{
    public class HomeWindow : BaseWindow<IHomeWindowModel>
    {
        [SerializeField] private TextWidget _gamesPlayed;
        [SerializeField] private ButtonWithTextWidget _playButton;
        
        protected override void SetBindings(IHomeWindowModel model, CompositeDisposable bindings)
        {
            _gamesPlayed.Bind(model.GamesPlayed);
            _playButton.Bind(model.PlayButton);
        }

        protected override void OnWindowOpen()
        {
            
        }

        protected override void OnWindowClose()
        {
            
        }
    }
}