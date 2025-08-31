using System;

namespace Game.Gameplay
{
    public interface IGameWidgetModel
    {
        string GameName { get; }
        ILevelDefinition Level { get; }
    }
    
    public class GameWidgetModel : IGameWidgetModel
    {
        public string GameName { get; }
        public ILevelDefinition Level { get; }

        public GameWidgetModel(string gameName, ILevelDefinition level)
        {
            GameName = gameName;
            Level = level;
        }
    }
}