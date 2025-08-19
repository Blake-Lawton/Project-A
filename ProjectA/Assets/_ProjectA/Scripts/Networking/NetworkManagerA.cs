using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Controllers;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Scripts.Networking
{
    public class NetworkManagerA : NetworkManager
    {
        [Title(" --------------Player! --------------")]
        public static NetworkManagerA Instance { get; private set;}
        
        [SerializeField] private List<PlayerBrain> _players;
        public  List<PlayerBrain> Players => _players;
        public PlayerBrain LocalPlayer { get; set; }

        public event Action<NetworkConnectionToClient> OnClientConnected;
        public event Action<NetworkConnectionToClient> OnClientCreated; 
        public event Action<NetworkConnectionToClient> OnClientDisconnected;

        public override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            OnClientConnected?.Invoke(conn);
            OnClientCreated?.Invoke(conn);
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            OnClientDisconnected?.Invoke(conn);
            base.OnServerDisconnect(conn);
            Debug.Log("Disconnected " + conn.identity);
            
        }

        public void AddPlayer(PlayerBrain player)
        {
            _players.Add(player);
            if (player.isLocalPlayer)
                LocalPlayer = player;
            
                
        } 
        public void RemovePlayer(PlayerBrain player) => _players.Remove(player);
    }
}
