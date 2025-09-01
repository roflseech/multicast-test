using Game.Common.UniRXExtensions;
using UnityEngine;

namespace Game.Gameplay
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;

        private IReadOnlyObservableValue<bool> IsReady { get; }

        public void SetGame(GameParams gameParams)
        {
            
        }
    }
}