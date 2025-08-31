using System;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IGameContext
    {
        IObservable<Unit> SetGame(GameParams gameParams);
    }
    
    public class GameContext : IGameContext
    {
        private GameParams _lastGame;
        private GameObject _currentIntance;
        
        public IObservable<Unit> SetGame(GameParams gameParams)
        {
            throw new NotImplementedException();
        }

        public GameObject CreateGameInstance()
        {
            throw new NotImplementedException();
        }
    }
}