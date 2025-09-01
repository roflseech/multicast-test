using System;
using System.Collections.Generic;

namespace Game.Games.CombineWordsGame
{
    [Serializable]
    public class CombineWordsLevelData
    {
        public int Rows;
        public int Columns;
        public List<string> TargetWords;
        public List<string> AvailableCLusters;
    }
}