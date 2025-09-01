using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Managers;
using _ProjectA.Scripts.UI.AbilitiyUIController;
using _ProjectA.Scripts.Util;
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
        
        private VFXHelper _castingVFX;
        private string _castingSFXID;
        private AudioSource _castingSFX;
       
        

        public override void SetUp()
        {
            base.SetUp();
            _castingVFX = _baseData.VFX["Casting"].SpawnParentedVFX(_brain.JointFinder.FindJoint(Joint.LeftHand).transform);
            _castingSFX = _castingVFX.GetComponent<AudioSource>();
            SFXManagerWrapper.Instance.SetUpAudioSource(_castingSFX, _brain.isLocalPlayer);
        }
        public override void EndCast()
        {
            Debug.Log(" TRIED TO STOP " + _castingSFXID);
           _castingSFX.Stop();
            _castingVFX.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if(!TargetInRange(_target))
                return;
            
            SFXManagerWrapper.Instance.Play(_baseData.SFX["OnCast"], transform.position, _brain.isLocalPlayer);
            var firePoint = _brain.JointFinder.FindJoint(Joint.LeftHand).transform;
            var projectile = Instantiate(TargetedProjectileData.Projectile, firePoint.position, firePoint.rotation);
            projectile.SetUp(TargetedProjectileData.Speed, _target, (target) =>
            {
                if (_brain.isClient)
                {
                   _baseData.VFX["OnHit"].SpawnVFX(target.transform);
                    SFXManagerWrapper.Instance.Play(_baseData.SFX["OnHit"], target.transform.position, _brain.isLocalPlayer);
                }
                    
                var interaction = new InteractionData(_brain, target, this);
                InteractionManager.Instance.ProcessInteraction(interaction);
                
            });
            
            _brain.Animation.Animator.SetTrigger(_baseData.Animations["EndCast"]);
        }

       

        public override void StartCast()
        {
            base.StartCast();
            _castingVFX.Particle.Play();
            _castingSFX.Play();
            Debug.Log(" SFX ID " + _castingSFXID);
            _brain.Animation.Animator.SetTrigger(_baseData.Animations["StartCast"]);
        }
        

        public override void Interrupt()
        {
            base.Interrupt();
            _castingVFX.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _castingSFX.Stop();
            _brain.Animation.Animator.SetTrigger("Idle");
        }

        public override void ConfirmHit()
        {
        }
    }
}
