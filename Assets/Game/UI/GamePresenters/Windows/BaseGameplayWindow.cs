using Game.UI.GameModels.Widgets;
using Game.UI.GameModels.Windows;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Presenters.Window;
using UniRx;
using UnityEngine;

namespace Game.UI.GamePresenters.Windows
{
    public class BaseGameplayWindow : BaseWindow<IBaseGameplayWindowModel>
    {
        [SerializeField] private ButtonWidget _backButton;
        [SerializeField] private GameWidget _gameWidget;
        
        protected override void SetBindings(IBaseGameplayWindowModel model, CompositeDisposable bindings)
        {
            _backButton.Bind(model.BackButton);
            model.GameWidget.Subscribe(_gameWidget.Bind).AddTo(bindings);
        }

        protected override void OnWindowOpen()
        {
            
        }

        protected override void OnWindowClose()
        {
            
        }
    }
}