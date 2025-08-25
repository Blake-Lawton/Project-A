using System.Collections.Generic;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using Data.Interaction;
using Data.StatusEffectData;
using Data.Types;
using UnityEngine;

namespace Data.AbilityData
{
    public abstract class BaseAbilityData : ScriptableObject
    {
        [Header("Base Data")]
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _castTime;
        [SerializeField] private TargetType _targetType;
        [SerializeField] private bool _targetSelf;
        [SerializeField] private bool _requireTarget;
        [SerializeField] private bool _interruptible;
        [SerializeField] private float _range;
        
        
        [Header("Status Effects")]
        [SerializeField] private List<BaseStatusData> _statusEffects;
        public Sprite Sprite => _sprite;
        public string Name => _name;
        public float Cooldown => _cooldown;
        public float CastTime => _castTime;
        public TargetType TargetType => _targetType;
        public bool TargetSelf => _targetSelf;
        public bool RequireTarget => _requireTarget;
        public bool Interruptible => _interruptible;
        public float Range => _range;
        public List<BaseStatusData> StatusEffects => _statusEffects;
        
        public abstract BaseAbility EquippedAbility();
    }
}
