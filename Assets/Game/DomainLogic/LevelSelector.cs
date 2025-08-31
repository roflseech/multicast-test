using Game.Gameplay;

namespace Game.DomainLogic
{
    public interface ILevelSelector
    {
        void SetCurrentLevel();
    }
    
    public class LevelSelector : ILevelSelector
    {
        private readonly IGameContext _gameContext;
        
        public void SetCurrentLevel()
        {
            
        }
    }
}