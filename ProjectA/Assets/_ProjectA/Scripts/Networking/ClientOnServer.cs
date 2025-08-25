using _ProjectA.Scripts.Controllers;
using Data.Types;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Networking
{
    public class ClientOnServer : NetworkBehaviour
    {
        //components
        private MovementController _movement;
        private ClientSidePrediction _csp;
        private RemoteClient _remote;
        
        //client csp
        private int _currentClientTick;
        private InputPayLoad[] _clientInputBuffer;
        private uint _latestInputTick;
        
        //server
        private uint _currentServerTick;
        private ServerStateSnapShot[] _serverGameStates;
        private float _serverTime;
        private float _accumulatedTime;
        
        
        private const int BUFFER_SIZE = 1024;
        
        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _csp = GetComponent<ClientSidePrediction>();
            _remote = GetComponent<RemoteClient>();
            _clientInputBuffer = new InputPayLoad[BUFFER_SIZE];
            _serverGameStates = new ServerStateSnapShot[BUFFER_SIZE];
        }

        public void HandleTick()
        {
            _currentServerTick++;
            ServerHandleTick();
          
        }

        public void HandleServerTime()
        {
            _serverTime += Time.unscaledDeltaTime;
        }
        [Command]
        public void SendInput(InputPayLoad payload)
        {
            _clientInputBuffer[payload.Tick % BUFFER_SIZE] = payload;
            if(payload.Tick > _latestInputTick)
                _latestInputTick = payload.Tick;
        }
        
        [Server]
        private void ServerHandleTick()
        {
            
            CspState cspState = default;
            while (_currentClientTick < _latestInputTick)
            {
                _currentClientTick++;
                if (_clientInputBuffer[_currentClientTick % BUFFER_SIZE].Equals(default(InputPayLoad)))
                {
                    //Debug.LogError("We missed a package and are copying old input");
                    _clientInputBuffer[_currentClientTick % BUFFER_SIZE] = _clientInputBuffer[_latestInputTick % BUFFER_SIZE];
                }
                
                InputPayLoad input = _clientInputBuffer[_currentClientTick % BUFFER_SIZE];
                
                cspState = _movement.ProcessInput(input);
                
                _clientInputBuffer[_currentClientTick % BUFFER_SIZE] = default;
            }
            
            _serverGameStates[_currentServerTick % BUFFER_SIZE] = new ServerStateSnapShot()
            {
                TimeStamp = _serverTime,
                Position = transform.position,
                Rotation = transform.eulerAngles,
            };
            
            
            //send to remote clients
            _remote.SendClientStates(_serverGameStates[_currentServerTick % BUFFER_SIZE]);
            
            if (!cspState.Equals(default(CspState)))
            {
                  _csp.ReceiveCspState(cspState);
            }
        }
      
    }
   
}
