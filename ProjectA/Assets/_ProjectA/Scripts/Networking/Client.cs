using System;
using _ProjectA.Scripts.Controllers;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Networking
{
    public class Client : NetworkBehaviour
    {
        private ClientSidePrediction _csp;
        private ClientOnServer _server;
        private RemoteClient _remote;
        private MovementController _movement;
        


        private float _accumulatedTime;
       

        public ClientSidePrediction Csp => _csp;
        public ClientOnServer Server => _server;
        public  RemoteClient Remote =>  _remote;
        public MovementController Movement => _movement;

        public event Action Tick; 
        private void Awake()
        {
            _csp = GetComponent<ClientSidePrediction>();
            _server = GetComponent<ClientOnServer>();
            _remote = GetComponent<RemoteClient>();
            _movement = GetComponent<MovementController>();
        }

        private void Start()
        {
            Application.targetFrameRate = isServer ? NetworkServer.sendRate : 144;
        }

        public void Handle()
        {
            
            _accumulatedTime += Time.deltaTime;
            
            while (_accumulatedTime >= NetworkServer.sendInterval)
            {
                if(isLocalPlayer)
                    _csp.HandleTick(this);
            
                if(isServer)
                    _server.HandleTick();
                
                _accumulatedTime -= NetworkServer.sendInterval;

                Tick?.Invoke();
            }
          
            if(isLocalPlayer)
                _csp.SmoothMovement(_accumulatedTime);
            
            if(isServer)
                _server.HandleServerTime();
            
            if(!isServer)
                _remote.Handle();
        }
    }
}
