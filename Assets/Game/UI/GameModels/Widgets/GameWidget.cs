using Game.Gameplay;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Presenters.Widget;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.UI.GameModels.Widgets
{
    public class GameWidget : BaseWidget<IGameWidgetModel>
    {
        [SerializeField] private GamePresenter _gamePresenters;

        private IObjectResolver _resolver;
        
        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            
            _resolver.Inject(_gamePresenters);
        }
        
        protected override void SetBindings(IGameWidgetModel model, CompositeDisposable bindings)
        {
            model.CurrentGame.Subscribe(data =>
            {
                _gamePresenters.SetGame(data);
            }).AddTo(bindings);
        }
    }
}