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
        private readonly IWindowFactory _windowFactory;
        
        private IButtonWidgetModel _backButton;

        private string _levelCompletionData;
        
        public IObservable<bool> ShowLoadingBar => Observable.Return(false);

        public IButtonWidgetModel BackButton => _backButton ??= new ButtonWidgetModel(
            () => _uiAggregate.Get(UiLayer.Main).OpenSingletonWindow<IHomeWindowModel>());

        public BaseGameplayWindowModel(IUiAggregate uiAggregate, ICurrentGame currentGame, IWindowFactory windowFactory)
        {
            _uiAggregate = uiAggregate;
            _currentGame = currentGame;
            _windowFactory = windowFactory;
        }
        
        public IObservable<IGameWidgetModel> GameWidget => Observable.Create<IGameWidgetModel>(o =>
        {
            var disposable = new CompositeDisposable();
            var model = new GameWidgetModel();
            o.OnNext(model);

            _currentGame.ObserveValid().Subscribe(data =>
                {
                    model.SetGame(data);
                })
                .AddTo(disposable);
            
            model.OnCompleted.Subscribe(_ =>
            {
                _levelCompletionData = model.GetLevelCompletionData();
                HandleLevelCompleted();
            }).AddTo(disposable);
            
            return disposable;
        });

        private void HandleLevelCompleted()
        {
            _currentGame.MarkCompleted();
            _uiAggregate.Get(UiLayer.Popup).OpenWindow(_windowFactory.CreateWindow(_levelCompletionData));
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}