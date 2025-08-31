using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.AppFlow
{
    public interface ICoreInitializer
    {
        UniTask InitAsync();
        bool IsInitialized { get; }
    }
    
    public class CoreInitializer : ICoreInitializer
    {
        public bool IsInitialized { get; private set; }
        

        public async UniTask InitAsync()
        {
            if (IsInitialized)
            {
                Debug.LogError("[CoreInitializer] Already initialized");
                return;
            }

            IsInitialized = true;
        }
    }
}