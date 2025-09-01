namespace Game.Games.Common.Data
{
    public interface ILevelDataParser
    {
        T Parse<T>(string levelData);
    }
    
    public class LevelDataParser
    {
        
    }
}