using System;
using System.Collections.Generic;
using System.Linq;
using _ProjectA.Scripts.Status;
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
     
        [SerializeField,FoldoutGroup("Status")] private bool _stunned;
        
        [SerializeField,FoldoutGroup("Status")] private bool _rooted;
        
        [Header("Slow")]
        [SerializeField,FoldoutGroup("Status")] private bool _slowed;
       
        [SerializeField,FoldoutGroup("Status")] private float _currentHighestSlow;
        
        public List<BaseStatus> Statuses => _statuses;
        
        //for now
        public int DamageAmp = 1;

        

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
                
            }
            _statusesToRemove.Clear();
            
            
            
            foreach (var status in _statuses)
            {
                status.Tick();
            }
            
            CalculateHighestSlow();
        }


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
    }
}
