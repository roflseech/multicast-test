namespace Game.Gameplay
{
    public class GameParams
    {
        public string GameName { get; }
        public string Level { get; }

        public GameParams(string gameName, string level)
        {
            GameName = gameName;
            Level = level;
        }
    }
}