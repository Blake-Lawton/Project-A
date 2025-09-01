using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Managers;
using _ProjectA.Scripts.Util;
using Data.AbilityData;
using Data.Interaction;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Abilities.TargetedInstantAbility
{
    public class BasicTargetedInstantAbility : TargetedAbility
    {
        private TargetedInstantData TargetedInstantData => (TargetedInstantData)_baseData;

        
        private VFXHelper _castingVFX;
        private string _castingSFXID;
        private AudioSource _castingSFX;
       
        

        public override void SetUp()
        {
            base.SetUp();
            _castingVFX = _baseData.FindVFX("Casting").SpawnParentedVFX(_brain.JointFinder.FindJoint(Joint.LeftHand).transform);
            _castingVFX.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _castingSFX = _castingVFX.GetComponent<AudioSource>();
            SFXManagerWrapper.Instance.SetUpAudioSource(_castingSFX, _brain.isLocalPlayer);
        }
        public override void EndCast()
        {
            _castingSFX.Stop();
            _castingVFX.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if(!TargetInRange(_target))
                return;
            
            //SFXManagerWrapper.Instance.Play(_baseData.FindSFX("OnCast"), transform.position, _brain.isLocalPlayer);
          
           
            if (_brain.isClient)
            {
               _baseData.FindVFX("OnHit").SpawnVFX(_target.transform);
                SFXManagerWrapper.Instance.Play(_baseData.FindSFX("OnHit"), _target.transform.position, _brain.isLocalPlayer);
            }
                
            var interaction = new InteractionData(_brain, _target, this);
            InteractionManager.Instance.ProcessInteraction(interaction);
                
            
            _brain.Animation.Animator.SetTrigger(_baseData.Animations["EndCast"]);
        }

       

        public override void StartCast()
        {
            base.StartCast();
            _castingVFX.Particle.Play();
            _castingSFX.Play();
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
