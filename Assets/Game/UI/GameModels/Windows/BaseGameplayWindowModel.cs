using System;
using Game.DomainLogic;
using Game.UI.GameLayers;
using Game.UI.GameModels.Widgets;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Models.Window;
using UniRx;

namespace Game.UI.GameModels.Windows
{
    public interface IBaseGameplayWindowModel : IWindowModel
    {
        IObservable<bool> ShowLoadingBar { get; }
        IButtonWidgetModel BackButton { get; }
        IObservable<IGameWidgetModel> GameWidget { get; }
    }
    
    public class BaseGameplayWindowModel : IBaseGameplayWindowModel
    {
        private readonly IUiAggregate _uiAggregate;
        private readonly ICurrentGame _currentGame;
        
        private IButtonWidgetModel _backButton;
        public IObservable<bool> ShowLoadingBar => Observable.Return(false);

        public IButtonWidgetModel BackButton => _backButton ??= new ButtonWidgetModel(
            () => _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>());

        public IObservable<IGameWidgetModel> GameWidget => Observable.Create<IGameWidgetModel>(o =>
        {
            var model = new GameWidgetModel();
            o.OnNext(model);

            return _currentGame.ObserveValid()
                .Subscribe(data =>
                {
                    model.SetGame(data);
                });
        });

        public BaseGameplayWindowModel(IUiAggregate uiAggregate, ICurrentGame currentGame)
        {
            _uiAggregate = uiAggregate;
            _currentGame = currentGame;
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}