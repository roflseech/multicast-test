using UnityEngine;

namespace Game.Games.Common.Data
{
    public interface ILevelDataParser
    {
        T Parse<T>(string levelData);
    }
    
    public class JsonLevelDataParser : ILevelDataParser
    {
        public T Parse<T>(string levelData)
        {
            return JsonUtility.FromJson<T>(levelData);
        }
    }
}