using System.Collections.Generic;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Util;
using Data.Interaction;
using Data.StatusEffectData;
using Data.Types;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Data.AbilityData
{
    public abstract class BaseAbilityData : SerializedScriptableObject
    {
        [Header("Base Data")]
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _castTime;
        [SerializeField] private AbilityType _type;
        [SerializeField] private int _healthChange;
        [Header("UI")]
        [SerializeField] private bool _showCastBar;

        [Header("Casting Requirements")] 
        [SerializeField] private TargetType _targetType;
        [SerializeField] private bool _isGlobal;
        [SerializeField] private bool _targetSelf;
        [SerializeField] private bool _requireTarget;
        [SerializeField] private bool _standStill;
       
        
        [Header("Ability Defines")]
        [SerializeField] private bool _interruptible;
        [SerializeField] private bool _canInterrupt;
        
        [Header("Status Effects")]
        [OdinSerialize] private List<BaseStatusData> _statusEffects;
        
        
        [Header("Animations ")]
        [OdinSerialize]private Dictionary<string, string> _animations;
        [Header("SFX")]
        [OdinSerialize]private Dictionary<string, SFXObject> _SFX;
        [Header("VFX")]
        [OdinSerialize]private Dictionary<string, VFXSpawner> _VFX;
        public Dictionary<string, SFXObject> SFX => _SFX;
        public Dictionary<string, VFXSpawner> VFX => _VFX;
        public Dictionary<string, string> Animations => _animations;
        public Sprite Sprite => _sprite;
        public string Name => _name;
        public float Cooldown => _cooldown;
        public float CastTime => _castTime;
        public TargetType TargetType => _targetType;
        public bool TargetSelf => _targetSelf;
        public bool RequireTarget => _requireTarget;
        public bool Interruptible => _interruptible;
        public bool IsGlobal => _isGlobal;
        public bool ShowCastBar => _showCastBar;
        public bool  StandStill => _standStill;
        public bool CanInterrupt => _canInterrupt;
        public List<BaseStatusData> StatusEffects => _statusEffects;
        public AbilityType Type => _type;
        public int HealthChange => _healthChange;
        public abstract BaseAbility EquippedAbility();

        public SFXObject FindSFX(string sfxKey)
        {
            if(SFX.ContainsKey(sfxKey))
                return _SFX[sfxKey];
            else
            {
                Debug.LogError("Unable to find SFX with key " + sfxKey);
                return null;
            }
        }

        public VFXSpawner FindVFX(string vfxKey)
        {
            if(_VFX.ContainsKey(vfxKey))
                return _VFX[vfxKey];
            else
            {
                Debug.LogError("Unable to find VFX with key " + vfxKey);
                return null;
            }
        }
        
    }
  
}
