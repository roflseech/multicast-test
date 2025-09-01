using Game.Common.VContainer;
using Game.Gameplay;
using VContainer;
using VContainer.Unity;

namespace Game.Games.CombineWordsGame
{
    public class CombineWordsGameInstaller : LifetimeScope, IGameContextHolder
    {
        public IGameContext Context => Container.Resolve<IGameContext>();

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.BindSingleton<GameContext>();
        }
    }
}