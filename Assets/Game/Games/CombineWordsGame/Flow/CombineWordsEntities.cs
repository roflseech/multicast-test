using System;
using System.Collections.Generic;
using Game.Games.CombineWordsGame.Entities;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Games.CombineWordsGame.Flow
{
    [Serializable]
    public class CombineWordsEntities
    {
        [field: SerializeField] public EntityRepository BottomRepository { get; private set; }
        [field: SerializeField] public Transform RowContainer { get; private set; }
        [field: SerializeField] public Transform ElementsContainer { get; private set; }
    }
}