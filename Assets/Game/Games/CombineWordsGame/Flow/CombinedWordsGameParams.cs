using UnityEngine;

namespace Game.Games.CombineWordsGame.Flow
{
    [CreateAssetMenu(menuName = "Game/CombineWordsGame/CombinedWordsGameParams", fileName = "CombinedWordsGameParams")]
    public class CombinedWordsGameParams : ScriptableObject
    {
        [field: SerializeField] public float VerticalDragThreshold { get; private set; } = 20f;
        [field: SerializeField] public float HorizontalScrollThreshold { get; private set; } = 15f;
    }
}