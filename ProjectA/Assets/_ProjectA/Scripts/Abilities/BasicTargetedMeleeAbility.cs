using System.Collections.Generic;
using _ProjectA.Data.AbilityData;
using _ProjectA.Data.Types;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.BaseClasses;
using _ProjectA.Scripts.Managers;
using _ProjectA.Scripts.Util;
using Data.Interaction;
using Data.Types;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Classes.GreatSword.Abilties.Strike.Scirpts
{
    public class BasicTargetedMeleeAbility : TargetedAbility
    {
        protected TargetedMeleeData TargetedMeleeData => (TargetedMeleeData)_baseData;
        [SerializeField] protected List<StrikeData> Strikes = new List<StrikeData>();

     

        public override void StartCast()
        {
            base.StartCast();
            foreach (var strike in Strikes)
                strike.HasStruck = false;
            
            _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], _baseData.AbilityAnimationNumber);
            _brain.Movement.SetRotationState(RotationState.LookAtTarget, _target);
            SFXManagerWrapper.Instance.Play(TargetedMeleeData.FindSFX("Swing"), transform.position, _brain.isLocalPlayer);
        }

   

        public override void Casting(float castTime)
        {
            foreach (var strike in Strikes)
            {
                if (strike.StrikeTime <= castTime && !strike.HasStruck)
                {
                    strike.HasStruck = true;
                    if (InteractionManager.Instance.CreateAndProcessInteraction(_brain, _target, this))
                    {
                        if(_brain.isServer)
                            _brain.Ability.ConfirmHit(strike.ID);
                    }
                        
                }
                   
            }
            
        }

        public override void EndCast()
        {
            _brain.Movement.SetRotationState(RotationState.RegularInput);
            _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], 0);
        }


        
        public override void ConfirmHit(int id)
        {
            base.EndCast();
            var vfx = TargetedMeleeData.FindVFX("Strike");
            if( vfx != null)
                vfx.SpawnVFX(Strikes[id].StrikeLocation);
            SFXManagerWrapper.Instance.Play(TargetedMeleeData.FindSFX("Impact"),transform.position, _brain.isLocalPlayer);
        }

        public override void Interrupt()
        {
            _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], 0);
        }
    }
}
