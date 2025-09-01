using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.AssetManagement
{
    public interface ILevelLoader
    {
        UniTask UpdateDatabaseAsync();
        int GetLevelCount(string gameName);
        IReadOnlyList<string> AvailableGames { get; }
        UniTask LoadLevelAsync(string gameName, int level);
        void UnloadLevel(string gameName, int level);
        string GetLevel(string gameName, int level);
    }
}