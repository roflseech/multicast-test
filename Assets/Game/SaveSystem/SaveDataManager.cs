using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.SaveSystem.Storage;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Game.SaveSystem
{
    public interface ISaveDataManager
    {
        void Initialize();
        UniTask LoadAllDataAsync();
    }
    
    public class SaveDataManager : ISaveDataManager, IDisposable
    {
        private readonly IObjectResolver _objectResolver;
        
        private readonly IPersistantDataStorage _persistantDataStorage;
        private readonly IRuntimeDataStorage _runtimeDataStorage;
        private readonly CompositeDisposable _disposable = new();
        
        private readonly Dictionary<string, KeyData> _keyData = new();

        private bool _loading;
        
        internal SaveDataManager(IRuntimeDataStorage runtimeDataStorage, IPersistantDataStorage persistantDataStorage, IObjectResolver objectResolver)
        {
            _runtimeDataStorage = runtimeDataStorage;
            _persistantDataStorage = persistantDataStorage;
            _objectResolver = objectResolver;
        }

        public void Initialize()
        {
            foreach (var keyData in _keyData)
            {
                keyData.Value.SaveOnUpdate().AddTo(_disposable);
            }
        }
        
        public async UniTask LoadAllDataAsync()
        {
            if (_loading)
            {
                Debug.LogError("Already loading");
                return;
            }
            
            _loading = true;
            
            var list = ListPool<UniTask>.Get();
            
            foreach (var key in _keyData)
            {
                list.Add(LoadFromPersistantDataAsync(key.Key));
            }
            
            await UniTask.WhenAll(list);
            
            ListPool<UniTask>.Release(list);
            _loading = false;
        }

        private async UniTask LoadFromPersistantDataAsync(string key)
        {
            var keyData = _keyData[key];
            
            if (!_persistantDataStorage.HasSaved(key))
            {
                keyData.SetDefaultValue();
                keyData.PublishValue();
                return;
            }

            await _persistantDataStorage.LoadAsync(key, _runtimeDataStorage.GetWriter(key));

            keyData.PublishValue();
        }
        
        public void Register<T>(string key, T defaultValue)
        {
            var keyData = new KeyData();

            keyData.SetDefaultValue = () =>
            {
                _runtimeDataStorage.Set(key, defaultValue);
            };

            keyData.PublishValue = () =>
            {
                GetSaveData().Publish(defaultValue);
            };

            keyData.SaveOnUpdate = () =>
            {
                return GetSaveData().Subscribe(value =>
                {
                    if (_loading) return;
                    
                    _runtimeDataStorage.Set(key, value);
                    RequestSave();
                });
            };
            
            _keyData[key] = keyData;
            
            ISaveData<T> GetSaveData()
            {
                return _objectResolver.Resolve<ISaveData<T>>();
            }
        }

        private void RequestSave()
        {
            
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private class KeyData
        {
            public Action SetDefaultValue;
            public Action PublishValue;
            public Func<IDisposable> SaveOnUpdate;
        }
    }
}