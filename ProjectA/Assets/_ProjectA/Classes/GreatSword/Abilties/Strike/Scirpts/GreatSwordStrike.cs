using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
 using _ProjectA.Scripts.Managers;
using _ProjectA.Scripts.Util;
using Data.Interaction;
using Data.Types;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Classes.GreatSword.Abilties.Strike.Scirpts
{
    public class GreatSwordStrike : TargetedAbility
    {
        protected TargetedMeleeData TargetedMeleeData => (TargetedMeleeData)_baseData;
        [SerializeField] private Transform _spawnPoint;
        private bool _hasStrike;

     

        public override void StartCast()
        {
            base.StartCast();
            _hasStrike = false;
            _brain.Animation.Animator.SetTrigger(TargetedMeleeData.Animations["Strike"]);
            _brain.Movement.ChangeRotationState(RotationState.LookAtTarget, _target);
            SFXManagerWrapper.Instance.Play(TargetedMeleeData.SFX["Swing"],transform.position, _brain.isLocalPlayer);
        }

   

        public override void Casting(float castTime)
        {
            if(castTime >= TargetedMeleeData.StrikeTime)
                Strike();
        }

        public override void EndCast()
        {
            
        }


        private void Strike()
        {
            if(_hasStrike)
                return;
            
            _hasStrike = true;
            
            var interaction = new InteractionData(_brain, _target, this);
            InteractionManager.Instance.ProcessInteraction(interaction);
            if(_brain.isServer)
                 _brain.Ability.ConfirmHit();
            
          
        }
        
        
        public override void ConfirmHit()
        {
            TargetedMeleeData.VFX["Strike"].SpawnVFX(_spawnPoint);
            SFXManagerWrapper.Instance.Play(TargetedMeleeData.SFX["Impact"],transform.position, _brain.isLocalPlayer);
        }
    }
}
