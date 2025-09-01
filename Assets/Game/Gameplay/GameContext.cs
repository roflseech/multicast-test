using System;
using Game.AssetManagement;
using Game.Common.UniRXExtensions;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IGameContext
    {
        void Setup(GameParams gameParams);
    }
    
    public class GameContext : IGameContext
    {
        public void Setup(GameParams gameParams)
        {
            Debug.LogError($"{gameParams.LevelData}");
        }
    }
}