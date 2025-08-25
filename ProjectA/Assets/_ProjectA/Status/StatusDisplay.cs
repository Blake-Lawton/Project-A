using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using Data.StatusEffectData;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _ProjectA.Status
{
    public class StatusDisplay : SerializedMonoBehaviour
    {
        [SerializeField, ReadOnly]
        private List<BaseStatus> _statuses = new List<BaseStatus>();
        private StatusController _statusController;
        
        
        #if UNITY_EDITOR
        private void Awake()
        {
            _statusController = GetComponent<StatusController>();
        }

        private void Update()
        {
            _statuses.Clear();
            foreach (var status in _statusController.Statuses)
            {
                _statuses.Add(status);
            }
        }
        #endif
    }
}
