using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.AssetManagement
{
    public interface ISpriteProvider
    {
        //UniTask<Sprite> GetSprite(string path);
        IObservable<Sprite> GetSpriteAsObservable(string path);
    }
}