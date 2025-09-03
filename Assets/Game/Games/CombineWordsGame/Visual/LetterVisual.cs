using TMPro;
using UnityEngine;

namespace Game.Games.CombineWordsGame.Visual
{
    public class LetterVisual : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void SetLetter(char letter)
        {
            _text.SetText(letter.ToString());
        }
    }
}