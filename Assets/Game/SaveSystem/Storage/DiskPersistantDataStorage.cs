using System;
using System.Buffers;
using System.IO;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;

namespace Game.SaveSystem.Storage
{
    internal class DiskPersistantDataStorage : IPersistantDataStorage
    {
        public async UniTask SaveAsync(string key, ArrayBufferWriter<byte> bufferWriter)
        {
            var filePath = GetFilePath(key);
            
            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.WriteAsync(bufferWriter.WrittenMemory);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save {key}: {ex.Message}");
            }
        }

        public async UniTask LoadAsync(string key, ArrayBufferWriter<byte> bufferWriter)
        {
            var filePath = GetFilePath(key);
            
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Can't find data for key: {key}");
                return;
            }

            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                
                var buffer = bufferWriter.GetMemory();
                int bytesRead = 0;

                do
                {
                    bytesRead = await fileStream.ReadAsync(buffer);
                    bufferWriter.Advance(bytesRead);
                } while (bytesRead > 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load {key}: {ex.Message}");
            }
        }

        public bool HasSaved(string key)
        {
            var filePath = GetFilePath(key);
            return File.Exists(filePath);
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}