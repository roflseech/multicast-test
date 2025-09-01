using Game.Gameplay;
using Game.UI.GamePresenters.Widgets;
using Game.UI.Presenters.Widget;
using UniRx;
using UnityEngine;

namespace Game.UI.GameModels.Widgets
{
    public class GameWidget : BaseWidget<IGameWidgetModel>
    {
        [SerializeField] private GamePresenter _gamePresenters;
        
        protected override void SetBindings(IGameWidgetModel model, CompositeDisposable bindings)
        {
            model.CurrentGame.Subscribe(data =>
            {
                //gamePresenters.SetGame()
            }).AddTo(bindings);
            
            
        }
    }
}