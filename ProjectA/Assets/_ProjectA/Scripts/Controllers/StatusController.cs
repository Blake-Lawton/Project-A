using System;
using System.Collections.Generic;
using System.Linq;
using _ProjectA.Scripts.UI;
using _ProjectA.Status.Active;
using Data.Interaction;
using Data.StatusEffectData;
using Mirror;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
    public class StatusController : NetworkBehaviour
    {
        private List<BaseStatus> _statuses = new List<BaseStatus>();
        private List<BaseStatus> _statusesToRemove = new List<BaseStatus>();
        private Dictionary<ParticleSystem, ParticleSystem> _activeStatusVFX = new();
        
        [SerializeField]private Transform _statusVFXRoot;
        
        private NamePlate _namePlate;
        
        [SerializeField,FoldoutGroup("Status"), SyncVar] private bool _stunned;
        [SerializeField,FoldoutGroup("Status"), SyncVar] private bool _rooted;
        [Header("Slow")]
        [SerializeField,FoldoutGroup("Status"),SyncVar] private bool _slowed;
       
        [SerializeField,FoldoutGroup("Status"), SyncVar] private float _currentHighestSlow;
        
        public bool Stunned => _stunned;
        public bool Rooted => _rooted;
        public List<BaseStatus> Statuses => _statuses;
        

        //for now
        public float DamageAmp = 1;

        
        public void SetUp(NamePlate namePlate)
        {
            _namePlate = namePlate;
        }
        
        public void ApplyStatus(BaseStatusData statusData, InteractionData interaction)
        {
            var existing = _statuses.Find(s => s.Data == statusData && s.Interaction.Perp == interaction.Perp);

            if (existing != null)
            {
                existing.Refresh();
            }
            else
            {
                var newStatus = statusData.CreateStatus(interaction);
                _statuses.Add(newStatus);
                //prob could be put into CreatStatus
                newStatus.SetUpUI(_namePlate.GenerateIcon(newStatus));
                ApplyStatusVFXIfNeeded(statusData);
                newStatus.Apply();
            }
        }


        public void RemoveStatus(BaseStatus status)
        {
            _statusesToRemove.Add(status);
        }
        
        public void Handle()
        {
            foreach (var toRemove in _statusesToRemove)
            {
                var status = _statuses.Find(s => s.Data == toRemove.Data && s.Interaction.Perp == toRemove.Interaction.Perp);
                _statuses.Remove(status);
                RemoveStatusVFXIfNeeded(toRemove.Data);
            }
            
            _statusesToRemove.Clear();
            
            foreach (var status in _statuses)
            {
                status.Handle();
            }
            
            if(!isServer) return;
            CalculateHighestSlow();
            IsStunned();
        }

        private void RemoveStatusVFXIfNeeded(BaseStatusData status)
        {
            var prefab = status.StatusVFX;
            if (prefab == null) return;

            // Only remove if no other status still using it
            bool stillNeeded = _statuses.Any(s => s.Data.StatusVFX == prefab);
            if (!stillNeeded && _activeStatusVFX.TryGetValue(prefab, out var vfxInstance))
            {
                Destroy(vfxInstance.gameObject,3);
                vfxInstance.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _activeStatusVFX.Remove(prefab);
            }
        }


        private void ApplyStatusVFXIfNeeded(BaseStatusData status)
        {
            var prefab = status.StatusVFX;
            if (prefab == null) return;

            // Only spawn if not already active
            if (!_activeStatusVFX.ContainsKey(prefab))
            {
                var vfxInstance = Instantiate(prefab, _statusVFXRoot);
                _activeStatusVFX[prefab] = vfxInstance;
            }
            
           
        }


        #region StatusStates

        #region Slow

        

      
       
        private void CalculateHighestSlow()
        {
            
            _currentHighestSlow = _statuses
                .OfType<SlowStatus>()            // Only SlowStatus instances
                .Select(s => s.SlowData.SlowAmount)       // Select their slow values
                .DefaultIfEmpty(0f)              // Default to 0 if list is empty
                .Max();
            
            _slowed = _currentHighestSlow > 0;
            
        }

        public float SlowFactor()
        {
            return 1 - _currentHighestSlow;
        }

        #endregion

        
        #region Stun

        private void IsStunned()
        {
            _stunned = _statuses.OfType<StunStatus>().Any();
        }
        
        #endregion
        #endregion
    }
}
