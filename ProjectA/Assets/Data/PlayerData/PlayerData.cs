using System;
using Data.Types;
using UnityEngine;

namespace _BazookaBrawl.Data.PlayerData
{
    [Serializable]
    public class PlayerData
    {
        private string _playerName;
        private string _playerId;
        private Team _team;
        

        public string PlayerName => _playerName;
        public PlayerData(string playerDataJson, Team team, string name)
        {
            _team = team;
            _playerName = name;
        }

       
        
    }
}
