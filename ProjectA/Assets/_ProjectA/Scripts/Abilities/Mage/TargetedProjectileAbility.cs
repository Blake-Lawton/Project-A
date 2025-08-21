using System;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities.Mage
{
    public class TargetedProjectileAbility : BaseAbility
    {
        [SerializeField] protected TargetedProjectileData _iceBoltData;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private PlayerBrain _target;

        protected override void Awake()
        {
            base.Awake();
            _data = _iceBoltData;
        }

        public override void EndCast()
        {
            var iceBolt = Instantiate(_iceBoltData.Projectile, _firePoint.position, _firePoint.rotation);
            iceBolt.SetUp(_brain, _target, _iceBoltData);
        }

        public override void UseAbility()
        {
            
        }

        public override void StartCast()
        {
            _target = _brain.Ability.Target;
        }
        
        public override void EndAbility()
        {
            
        }

        public override bool CanCastAbility()
        {
            return !OnCooldown && !_brain.Movement.Moving && !_brain.Ability.OnGlobalCd && !_brain.Ability.IsCasting && _brain.Ability.HasTarget;
        }
    }
}
