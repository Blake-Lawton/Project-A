using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using Data.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.StatusEffectData
{
    [Serializable]
    public abstract class BaseStatusData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _duration;
        [SerializeField] private bool _isBuff;
        [SerializeField] private bool _hasStacks;
        [SerializeField] private bool _showAllPlayers;
        [InfoBox("This can be null if no status vfx is needed")]
        [SerializeField] private ParticleSystem _statusVFX;
        
        public bool HasStacks => _hasStacks;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float Duration => _duration;
        public bool IsBuff => _isBuff;
        public ParticleSystem StatusVFX => _statusVFX;
        
        public bool ShowAllPlayers => _showAllPlayers;

        public abstract BaseStatus CreateStatus(InteractionData interaction);

    }
}
