using UnityEngine;

namespace Tetris
{
    public static class PlayerPrefsScore
    {
        public static long CurrentHighscore
        {
            get
            {
                long highScore;
                if (long.TryParse(PlayerPrefs.GetString("CurrentHighscore"), out highScore))
                    return highScore;
                return 0;
            }
            set
            {
                PlayerPrefs.SetString("CurrentHighscore", value.ToString());
                PlayerPrefs.Save();
            }
        }
    }
}
