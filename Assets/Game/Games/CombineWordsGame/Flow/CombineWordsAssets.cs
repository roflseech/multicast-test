using UnityEngine;

namespace Game.Games.CombineWordsGame.Flow
{
    [CreateAssetMenu(menuName = "Game/CombineWordsGame/CombineWordsAssets", fileName = "CombineWordsAssets")]
    public class CombineWordsAssets : ScriptableObject
    {
        [field: SerializeField] public string WordClusterPrefabPath { get; private set; }
        [field: SerializeField] public string LetterRowPrefabPath { get; private set; }
    }
}