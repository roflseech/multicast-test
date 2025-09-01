namespace Game.Gameplay
{
    public readonly struct GameParams
    {
        public static readonly GameParams Undefined = new GameParams(string.Empty, 0, string.Empty);
        
        public readonly string GameName;
        public readonly int Level;
        public readonly string LevelData;
        
        public GameParams(string gameName, int level, string levelData)
        {
            GameName = gameName;
            Level = level;
            LevelData = levelData;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(GameName) && Level > 0 && !string.IsNullOrEmpty(LevelData);
        }
    }
}