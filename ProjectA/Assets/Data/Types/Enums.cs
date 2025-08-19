using System;

namespace _BazookaBrawl.Data
{
    public enum Team
    {
        Team1 = 1,
        Team2 = 2,
        Team3 = 3 
    }

    public enum HitBoxType
    {
        Head,
        Body,
        Weapon
    }

    public enum GameState
    {
        Loading,
        RoundStarting,
        Playing,
        RoundEnd,
        EndGame
    }
}
