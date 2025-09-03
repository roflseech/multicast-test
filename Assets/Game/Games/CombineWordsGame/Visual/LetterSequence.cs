using System.Collections.Generic;
using UnityEngine;

namespace Game.Games.CombineWordsGame.Visual
{
    public class LetterSequence : MonoBehaviour
    {
        [SerializeField] private LetterVisual _letter;

        private List<LetterVisual> _letterVisuals;

        public Vector3 GetLetterPosition(int letterIndex)
        {
            return _letterVisuals[letterIndex].transform.position;
        }
        
        public void SetLetters(string letters)
        {
            if (_letterVisuals == null)
            {
                _letterVisuals = new();
                _letterVisuals.Add(_letter);
            }
            
            int requiredCount = letters.Length;

            var prevLetter = _letter;
            while (_letterVisuals.Count < requiredCount)
            {
                var newLetter = Instantiate(_letter, _letter.transform.parent);
                newLetter.transform.localScale = Vector3.one;
                _letterVisuals.Add(newLetter);
                newLetter.transform.SetSiblingIndex(prevLetter.transform.GetSiblingIndex() + 1);
                
                prevLetter = newLetter;
            }
            
            while (_letterVisuals.Count > requiredCount)
            {
                var lastLetter = _letterVisuals[^1];
                _letterVisuals.RemoveAt(_letterVisuals.Count - 1);
                Destroy(lastLetter.gameObject);
            }
            
            for (int i = 0; i < letters.Length; i++)
            {
                _letterVisuals[i].SetLetter(letters[i]);
            }
        }
    }
}