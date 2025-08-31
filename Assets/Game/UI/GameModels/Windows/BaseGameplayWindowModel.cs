using System;
using Game.UI.GameLayers;
using Game.UI.GameModels.Widgets;
using Game.UI.Models.Window;
using UniRx;

namespace Game.UI.GameModels.Windows
{
    public interface IBaseGameplayWindowModel : IWindowModel
    {
        IObservable<bool> ShowLoadingBar { get; }
        IButtonWidgetModel BackButton { get; }
    }
    
    public class BaseGameplayWindowModel : IBaseGameplayWindowModel
    {
        private readonly IUiAggregate _uiAggregate;
        
        private IButtonWidgetModel _backButton;
        public IObservable<bool> ShowLoadingBar => Observable.Return(false);

        public IButtonWidgetModel BackButton => _backButton ??= new ButtonWidgetModel(
            () => _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>());

        public BaseGameplayWindowModel(IUiAggregate uiAggregate)
        {
            _uiAggregate = uiAggregate;
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}