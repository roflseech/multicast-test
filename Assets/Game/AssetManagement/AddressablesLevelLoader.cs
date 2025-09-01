using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.AssetManagement
{
    public class AddressablesLevelLoader : ILevelLoader
    {
        private const string LEVEL_TAG = "Level";
        
        private class LevelEntry
        {
            public string Content { get; set; }
            public AsyncOperationHandle<TextAsset> Handle { get; set; }
        }

        private readonly Dictionary<string, List<string>> _levels = new();
        private readonly List<string> _availableGames = new();
        private readonly Dictionary<string, Dictionary<int, LevelEntry>> _loadedLevels = new();
        
        public IReadOnlyList<string> AvailableGames => _availableGames.AsReadOnly();

        public async UniTask UpdateDatabaseAsync()
        {
            UnloadAllLevels();
            _levels.Clear();
            _availableGames.Clear();
            _loadedLevels.Clear();

            var locationsHandle = Addressables.LoadResourceLocationsAsync(LEVEL_TAG);
            var locations = await locationsHandle.ToUniTask();

            var gameGroups = new Dictionary<string, List<(int level, string location)>>();

            foreach (var location in locations)
            {
                var gameName = ExtractGameName(location.PrimaryKey);
                if (string.IsNullOrEmpty(gameName))
                {
                    Debug.LogError($"Could not extract game name from location '{location.PrimaryKey}'");
                    continue;
                }

                var levelNumber = ExtractLevelNumber(location.PrimaryKey);
                if (levelNumber == -1)
                {
                    Debug.LogError($"Could not extract level number from location '{location.PrimaryKey}'");
                    continue;
                }

                if (!gameGroups.ContainsKey(gameName))
                {
                    gameGroups[gameName] = new List<(int level, string location)>();
                }

                gameGroups[gameName].Add((levelNumber, location.PrimaryKey));
            }

            foreach (var gameGroup in gameGroups)
            {
                var gameName = gameGroup.Key;
                var levelData = gameGroup.Value.OrderBy(x => x.level).ToList();

                var levels = new List<string>();
                var expectedLevel = 1;

                foreach (var (level, location) in levelData)
                {
                    while (expectedLevel < level)
                    {
                        Debug.LogError($"Missing level {expectedLevel} in game '{gameName}'");
                        expectedLevel++;
                    }

                    if (level == expectedLevel)
                    {
                        levels.Add(location);
                        expectedLevel++;
                    }
                    else if (level > expectedLevel)
                    {
                        Debug.LogError($"Unexpected level {level} in game '{gameName}', expected {expectedLevel}");
                    }
                }

                if (levels.Count > 0)
                {
                    _levels[gameName] = levels;
                    _availableGames.Add(gameName);
                }
            }

            Addressables.Release(locationsHandle);
        }

        private string ExtractGameName(string locationKey)
        {
            var lastSlashIndex = locationKey.LastIndexOf('/');
            var pathAfterSlash = lastSlashIndex >= 0 ? locationKey.Substring(lastSlashIndex + 1) : locationKey;
            var match = Regex.Match(pathAfterSlash, @"[a-zA-Z]+");
            return match.Success ? match.Value : null;
        }

        private int ExtractLevelNumber(string locationKey)
        {
            var match = Regex.Match(locationKey, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int levelNumber))
            {
                return levelNumber;
            }
            return -1;
        }

        public int GetLevelCount(string gameName)
        {
            return _levels.TryGetValue(gameName, out var level) ? level.Count : 0;
        }

        public async UniTask LoadLevelAsync(string gameName, int level)
        {
            if (IsLevelLoaded(gameName, level))
            {
                return;
            }

            var levelLocation = GetLevelLocation(gameName, level);
            if (string.IsNullOrEmpty(levelLocation))
            {
                Debug.LogError($"Level {level} not found for game '{gameName}'");
                return;
            }

            var handle = Addressables.LoadAssetAsync<TextAsset>(levelLocation);
            var textAsset = await handle.ToUniTask();
            
            if (textAsset == null)
            {
                Debug.LogError($"Failed to load TextAsset at location '{levelLocation}'");
                Addressables.Release(handle);
                return;
            }

            if (!_loadedLevels.ContainsKey(gameName))
            {
                _loadedLevels[gameName] = new Dictionary<int, LevelEntry>();
            }

            _loadedLevels[gameName][level] = new LevelEntry
            {
                Content = textAsset.text,
                Handle = handle
            };
        }

        public void UnloadLevel(string gameName, int level)
        {
            if (!_loadedLevels.ContainsKey(gameName) || !_loadedLevels[gameName].ContainsKey(level))
            {
                return;
            }

            var levelEntry = _loadedLevels[gameName][level];
            Addressables.Release(levelEntry.Handle);
            
            _loadedLevels[gameName].Remove(level);
            
            if (_loadedLevels[gameName].Count == 0)
            {
                _loadedLevels.Remove(gameName);
            }
        }

        public string GetLevel(string gameName, int level)
        {
            if (_loadedLevels.ContainsKey(gameName) && _loadedLevels[gameName].ContainsKey(level))
            {
                return _loadedLevels[gameName][level].Content;
            }

            return null;
        }

        private string GetLevelLocation(string gameName, int level)
        {
            if (!_levels.ContainsKey(gameName))
            {
                return null;
            }

            var levelIndex = level - 1;
            if (levelIndex < 0 || levelIndex >= _levels[gameName].Count)
            {
                return null;
            }

            return _levels[gameName][levelIndex];
        }

        private bool IsLevelLoaded(string gameName, int level)
        {
            return _loadedLevels.ContainsKey(gameName) && _loadedLevels[gameName].ContainsKey(level);
        }

        private void UnloadAllLevels()
        {
            foreach (var gameEntry in _loadedLevels)
            {
                foreach (var levelEntry in gameEntry.Value)
                {
                    Addressables.Release(levelEntry.Value.Handle);
                }
            }
        }
    }
}