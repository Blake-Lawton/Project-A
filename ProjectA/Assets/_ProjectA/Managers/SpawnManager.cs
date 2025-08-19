using System;
using _BazookaBrawl.Data;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Networking;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private NetworkManagerA _networkManager;
        [SerializeField] private PlayerBrain _playerPrefab;
        
        
        public Action<PlayerBrain> OnPlayerSpawned;
        
        private bool _switchTeams;
        
        private void Awake()
        {
            _networkManager.OnClientConnected += ClientConnected;
        }

       

        private void ClientConnected(NetworkConnectionToClient conn)
        {
            Team team = Team.Team1;
            if (_switchTeams)
            {
                team = Team.Team2;
                _switchTeams = false;
            }
            else
                _switchTeams = true;


            PlayerBrain player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            player.SetUp(team);
            OnPlayerSpawned?.Invoke(player);
        }


        [Button]
        public void ResetAllPlayers()
        {
            int team1Slots = 0;
            int team2Slots = 0;
            foreach (var player in _networkManager.Players)
            {
                player.Reset();
                if (player.Team == Team.Team1)
                {
                    //player.Movement.Teleport(MapInfo.Instance.Team1Spawns[team1Slots]);
                    team1Slots++;
                }
                else
                {
                    //player.Movement.Teleport(MapInfo.Instance.Team2Spawns[team2Slots]);
                    team2Slots++;
                }
                
            }
        }
      
    }
}
