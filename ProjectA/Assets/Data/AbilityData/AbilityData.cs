using Data.Types;
using UnityEngine;

namespace Data.AbilityData
{
    public abstract class AbilityData : ScriptableObject
    {
        [Header("Base Data")]
        [SerializeField]private Sprite _sprite;
        [SerializeField]private string _name;
        [SerializeField]private float _cooldown;
        [SerializeField]private float _castTime;
        [SerializeField]private TargetType _targetType;
        [SerializeField]private bool _targetSelf;
        [SerializeField]private bool _requireTarget;
    
        public Sprite Sprite => _sprite;
        public string Name => _name;
        public float Cooldown => _cooldown;
        public float CastTime => _castTime;
        public TargetType TargetType => _targetType;
        public bool TargetSelf => _targetSelf;
        public bool RequireTarget => _requireTarget;
    
    }
}
