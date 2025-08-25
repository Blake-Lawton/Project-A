using _ProjectA.Scripts.Controllers;
using Data.Types;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Networking
{
    public class ClientSidePrediction : NetworkBehaviour
    {
        //Components
        private MovementController _movement;
        private ClientOnServer _server;
        
        //data
        private uint _currentTick = 2;
        private InputPayLoad[] _inputBuffer;
        private CspState[] _clientState;
        private CspState _latestServerCspState;
        private CspState _latestProcessedCspState;
        private const int BUFFER_SIZE = 1024;
        
      
        
        private GameObject _serverDummy;
        
        public uint CurrentTick => _currentTick;
        
        private void Awake()
        {
            _server = GetComponent<ClientOnServer>();
            _movement = GetComponent<MovementController>();
            _latestServerCspState = new CspState(){Position = Vector3.zero,  Tick = 2};
            
            _clientState = new CspState[BUFFER_SIZE];
            _inputBuffer = new InputPayLoad[BUFFER_SIZE];
        }

    
        private void OnDestroy() => Destroy(_serverDummy);
        
        public void HandleTick(Client client)
        {
            HandlePrediction(client);
            _currentTick++;
        }

        
        public void SmoothMovement(float _accumulateTime)
        {
            transform.position = Vector3.Lerp(_clientState[(_currentTick - 2) % BUFFER_SIZE].Position, 
                _clientState[(_currentTick -1) % BUFFER_SIZE].Position, _accumulateTime / NetworkServer.sendInterval);
        }
        
        private void HandlePrediction(Client client)
        {
            
            if ( _latestProcessedCspState.Equals(default(CspState)) &&
                    !_latestServerCspState.Equals(default(CspState)) || 
                    !_latestServerCspState.Equals(_latestProcessedCspState))
            {
                ClientReconciliation();
            }
            //record movement input
            _movement.TakeInput();
            
            //package it
            InputPayLoad input = new InputPayLoad(client);
            
            //Movement need to put player at the exact place from previous calculations then calculate the next movement
            transform.position = _clientState[(_currentTick - 1) % BUFFER_SIZE].Position;
            
            CspState cspState = _movement.ProcessInput(input);
            
            //Shooting input is dealt with on the bazooka controller
           
            
            //RecordInput
            uint bufferIndex = _currentTick % BUFFER_SIZE;
            _inputBuffer[bufferIndex] = input;
            _clientState[bufferIndex] = cspState;
            
            //send it
            if(!isServer)
                _server.SendInput(input);
        }
        
      
        
        [TargetRpc]
        public void ReceiveCspState(CspState latestPayLoad)
        {
            if (_latestServerCspState.Tick > latestPayLoad.Tick)
                return;
            
            _latestServerCspState = latestPayLoad;
        }
        
        private void ClientReconciliation()
        {
            if(_latestServerCspState.Tick <= _latestProcessedCspState.Tick)
                return;
            
            _latestProcessedCspState = _latestServerCspState;
            
            uint bufferIndex = _latestProcessedCspState.Tick % BUFFER_SIZE;
        
            if (Vector3.Distance(_clientState[bufferIndex].Position, _latestProcessedCspState.Position) > .001f)
            {
                Debug.LogWarning("We had to reconsile from tick " + _latestProcessedCspState.Tick + " AT "+ _latestServerCspState.Position +"   to   " + _currentTick + " AT " + transform.position);
                
                _movement.SetState(_latestProcessedCspState);
                
                _clientState[bufferIndex] = _latestProcessedCspState;
                uint latestTick = _latestProcessedCspState.Tick + 1;
                    
                while (_currentTick > latestTick)
                {
                    bufferIndex = latestTick % BUFFER_SIZE;
                    var state = _movement.ProcessInput(_inputBuffer[bufferIndex]);
                    _clientState[bufferIndex] = state;
                    latestTick++;
                }
            }
        }
    }
}