using System.Collections.Generic;
using Data.Types;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Networking
{
    public class RemoteClient : NetworkBehaviour
    {
        
        //componenets
        // eventually we need to make this expand if we don't get proper data
        [SerializeField, ReadOnly] private float _snapShotBufferMultiplier = 2;
        private ServerStateSnapShot _fromSnapShot = default;
        private ServerStateSnapShot _toSnapShot = default;
        private float _clientAccumulatedTime;
        
        public float TimeBehindServer => _snapShotBuffer.Values[^1].TimeStamp - _clientAccumulatedTime - Time.unscaledDeltaTime;
        private float BufferTime => NetworkServer.sendInterval * _snapShotBufferMultiplier;
        
        private readonly SortedList<double, ServerStateSnapShot> _snapShotBuffer = new SortedList<double, ServerStateSnapShot>();

      
        
        public void Handle()
        {
            HandleClientStates();
        }
        
        
        [ClientRpc]
        public void SendClientStates(ServerStateSnapShot snapShot)
        {
            if (_snapShotBuffer.Count <= 0)
                _clientAccumulatedTime = snapShot.TimeStamp - BufferTime;
            
            _snapShotBuffer[snapShot.TimeStamp] = snapShot;
        }

       
        private void HandleClientStates()
        {
            if(_snapShotBuffer.Count <= 0)
                return;
            _clientAccumulatedTime += Time.unscaledDeltaTime;
            
            double targetTime = _snapShotBuffer.Values[^1].TimeStamp - BufferTime;
            double lowerBound = targetTime - BufferTime; // how far behind we can get
            double upperBound = targetTime + BufferTime; // how far ahead we can get
            _clientAccumulatedTime = (float)Mathd.Clamp(_clientAccumulatedTime, lowerBound, upperBound);
            
            
            int removeAt = -1;
            
            if (_snapShotBuffer.Count > 2)
            {
                
                for (int i = 0; i < _snapShotBuffer.Count - 1; i++)
                {
                    
                    if (_snapShotBuffer.Values[i].TimeStamp <= _clientAccumulatedTime &&
                        _snapShotBuffer.Values[i + 1].TimeStamp >= _clientAccumulatedTime)
                    {
                        _fromSnapShot = _snapShotBuffer.Values[i];
                        _toSnapShot = _snapShotBuffer.Values[i + 1];
                        removeAt = i; 
                        break;
                    }
                }
            }

            float interpolationFactor = Mathf.InverseLerp(_fromSnapShot.TimeStamp, _toSnapShot.TimeStamp, (float)_clientAccumulatedTime);
            
            // didn't find two snapshots around local time.
            // so pick either the first or last, depending on which is closer.
            if (removeAt == -1 && _snapShotBuffer.Count >= 1)
            {
                // oldest snapshot ahead of local time?
                if (_snapShotBuffer.Values[0].TimeStamp > _clientAccumulatedTime)
                {
                    _fromSnapShot = _snapShotBuffer.Values[0];
                    _toSnapShot = _snapShotBuffer.Values[0];
                    interpolationFactor = 0;
                }
                // otherwise initialize both to the last one
                else
                {
                    _fromSnapShot = _snapShotBuffer.Values[^1];
                    _toSnapShot = _snapShotBuffer.Values[^1];
                    interpolationFactor = 0;
                }
            }
          
            Vector3 interpolatedPosition = Vector3.LerpUnclamped(_fromSnapShot.Position, _toSnapShot.Position, interpolationFactor);
            Vector3 interpolatedRotation = Vector3.LerpUnclamped(_fromSnapShot.Rotation, _toSnapShot.Rotation, interpolationFactor);
            
        
            if (!isLocalPlayer)
            {
                transform.position = interpolatedPosition;
                transform.rotation = Quaternion.Euler(interpolatedRotation);
            }

            if (removeAt != -1)
            {
                _snapShotBuffer.RemoveRange(removeAt);
            }
                
        }
    }
}
