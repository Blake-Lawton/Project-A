using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using Data.Interaction;
using UnityEngine;

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
        
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float Duration => _duration;
        public bool IsBuff => _isBuff;
        

        public abstract BaseStatus CreateStatus(InteractionData interaction);

    }
}
