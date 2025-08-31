using Cysharp.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;

namespace Game.UI.Common.UiElements
{
    public class TextUiElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetString(string text)
        {
            _text.text = text;
        }
        
        public void SetString(in Utf16ValueStringBuilder sb)
        {
            _text.SetText(sb);
        }
    }
}