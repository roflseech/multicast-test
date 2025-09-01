using Game.AssetManagement;
using Game.Gameplay;
using Game.SaveSystem;
using Game.State;

namespace Game.DomainLogic
{
    public interface ILevelSelector
    {
        void SetLastAvailableLevel();
    }
    
    public class LevelSelector : ILevelSelector
    {
        private readonly ISaveData<PlayerStatistics> _playerStatistics;
        private readonly ICurrentGame _currentGame;
        private readonly ILevelLoader _levelLoader;
        
        public LevelSelector(ISaveData<PlayerStatistics> playerStatistics, ICurrentGame currentGame, ILevelLoader levelLoader)
        {
            _playerStatistics = playerStatistics;
            _currentGame = currentGame;
            _levelLoader = levelLoader;
        }

        public void SetLastAvailableLevel()
        {
            //только по первой игре, для данной задачи пока не имеет смысла поддерживать все
            var game = _levelLoader.AvailableGames[0];
            var levelCount = _levelLoader.GetLevelCount(game);
            var completedLevels = _playerStatistics.Value.LevelsCompleted;
            var targetLevel = completedLevels % levelCount + 1;
            _currentGame.SetLevel(game, targetLevel);
        }
    }
}