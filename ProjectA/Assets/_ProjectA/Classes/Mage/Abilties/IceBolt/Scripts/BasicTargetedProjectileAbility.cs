using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI.AbilitiyUIController;
using Data.AbilityData;
using Data.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Classes.Mage.Abilties.IceBolt.Scripts
{
    public class BasicTargetedProjectileAbility : TargetedAbility
    {
        
        protected TargetedProjectileData TargetedProjectileData => (TargetedProjectileData)_baseData;
        
        [SerializeField,ReadOnly] private ParticleSystem _castingVFX;
        private AMPAudioSource _castingSFX;
       
        

        public override void SetUp()
        {
            base.SetUp();
            _castingVFX = Instantiate(TargetedProjectileData.CastingVFXPrefab, _brain.JointFinder.FindJoint(Joint.LeftHand).transform);
            _castingSFX = _castingVFX.GetComponent<AMPAudioSource>();
        }
        public override void EndCast()
        {
            _castingSFX.Stop();
            _castingVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
          
            if(!TargetInRange(_target))
                return;
            
            SFXManager.Main.Play(TargetedProjectileData.CastedSFX, transform.position);
            var firePoint = _brain.JointFinder.FindJoint(Joint.LeftHand).transform;
            var projectile = Instantiate(TargetedProjectileData.Projectile, firePoint.position, firePoint.rotation);
            projectile.SetUp(TargetedProjectileData.Speed, _target, (target) =>
            {
                if (_brain.isClient)
                {
                    Instantiate(TargetedProjectileData.OnHitVFXPrefab, target.transform.position, Quaternion.identity);
                    SFXManager.Main.Play(TargetedProjectileData.OnHitSFX, target.transform.position);
                }
                    
                var interaction = new InteractionData(_brain, target, this);
                InteractionManager.Instance.ProcessInteraction(interaction);
                
            });
            
            _brain.Animation.Animator.SetTrigger(TargetedProjectileData.EndCastTrigger);
        }

       

        public override void StartCast()
        {
            base.StartCast();
            _castingVFX.Play();
            _brain.Animation.Animator.SetTrigger(TargetedProjectileData.StartCastTrigger);
            _castingSFX.Play();
        }
        
        public override void UseAbility()
        {
        }

        public override void EndAbility()
        {
        }

        public override void Interrupt()
        {
            base.Interrupt();
            _castingVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _castingSFX.Stop();
        }

        public override void ConfirmHit()
        {
        }
    }
}
