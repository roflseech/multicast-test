using Game.DomainLogic;
using Game.UI.GameLayers;

namespace Game.UI.GameModels.Windows
{
    public interface IWindowFactory
    {
        IWinWindowModel CreateWindow(string levelCompletionData);
    }
    
    public class WindowFactory : IWindowFactory
    {
        private readonly ILevelSelector _levelSelector;
        private readonly IUiAggregate _uiAggregate;

        public WindowFactory(ILevelSelector levelSelector, IUiAggregate uiAggregate)
        {
            _levelSelector = levelSelector;
            _uiAggregate = uiAggregate;
        }

        public IWinWindowModel CreateWindow(string levelCompletionData)
        {
            return new WinWindowModel(levelCompletionData, _levelSelector, _uiAggregate);
        }
    }
}