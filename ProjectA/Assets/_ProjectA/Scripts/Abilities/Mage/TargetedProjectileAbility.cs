using System;
using _ProjectA.Managers;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Data.Interaction;
using Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Abilities.Mage
{
    public class TargetedProjectileAbility : BaseAbility
    {
        [FormerlySerializedAs("_targetedProjectileData")]
        [FormerlySerializedAs("_projectileData")]
        [Header("Projectile Data")]
        [SerializeField] protected TargetedProjectileData _data;
        private PlayerBrain _target;
        [SerializeField,ReadOnly] private ParticleSystem _castingVFX;

        public TargetedProjectileData Data => _data;
        protected override void Start()
        {
            base.Start();
            _baseData = _data;
            _castingVFX = Instantiate(_data.CastingVFXPrefab, _brain.JointFinder.FindJoint(Joint.LeftHand));
          
        }

        public override void EndCast()
        {
            _castingVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            var firePoint = _brain.JointFinder.FindJoint(Joint.LeftHand);
            var projectile = Instantiate(_data.Projectile, firePoint.position, firePoint.rotation);
            projectile.SetUp(_data.Speed, _target, ((target) =>
            {
                if(_brain.isClient)
                    Instantiate(_data.OnHitVFXPrefab, _target.transform.position, Quaternion.identity);
                var interaction = new InteractionData(_brain, target, this);
                InteractionManager.Instance.ProcessInteraction(interaction);
               
            }));
            _brain.Animation.Animator.SetTrigger(_data.EndCastTrigger);
        }

        public override void ResolveAbility(InteractionData interaction)
        {
            if (!interaction.Perp || !interaction.Victim) return;
            
            
            interaction.Victim.Health.TakeDamage(interaction.Perp.Status.DamageAmp * _data.Damage, interaction);
            foreach (var status in _baseData.StatusEffects)
                interaction.Victim.Status.ApplyStatus(status, interaction);
        }


        public override void UseAbility()
        {
            
        }

        public override void StartCast()
        {
            _castingVFX.Play();
            _target = _brain.Ability.Target;
            _brain.Animation.Animator.SetTrigger(_data.StartCastTrigger);
            
        }
        
        public override void EndAbility()
        {
            
        }

        public override void Interupt()
        {
            _castingVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public override bool CanCastAbility(PlayerBrain target)
        {
            bool correctTarget = false;
            switch (_baseData.TargetType)
            {
                case TargetType.Ally:
                    correctTarget = _brain.OnSameTeam(target);
                    break;
                case TargetType.Enemy:
                    correctTarget = !_brain.OnSameTeam(target);
                    break;
                case TargetType.Both:
                    correctTarget = true;
                    break;
            }
            return !OnCooldown && !_brain.Movement.Moving && !_brain.Ability.OnGlobalCd && !_brain.Ability.IsCasting && _brain.Ability.HasTarget && correctTarget;
        }

      
    }
}
