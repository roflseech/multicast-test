using System;
using Cysharp.Threading.Tasks;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IGameContext
    {
        IObservable<Unit> OnCompleted { get; }
        IReadOnlyObservableValue<bool> IsReady { get; }
        void Setup(GameParams gameParams);
        void SetReady();
        string GetLevelCompletionData();
    }
}