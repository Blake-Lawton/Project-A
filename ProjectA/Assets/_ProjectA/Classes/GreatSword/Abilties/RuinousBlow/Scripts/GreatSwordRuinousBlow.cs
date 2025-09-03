using System;
using System.Collections.Generic;
using _ProjectA.Data.AbilityData;
using _ProjectA.Data.Types;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.BaseClasses;
using _ProjectA.Scripts.Managers;
using Data.Interaction;
using Data.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectA.Classes.GreatSword.Abilties.RuinousBlow.Scripts
{
    public class GreatSwordRuinousBlow : TargetedAbility
    {

        public TargetedMeleeData MeleeData => (TargetedMeleeData)_baseData;

         [SerializeField]private List<StrikeData> _strikes = new List<StrikeData>();


        public override void SetUp()
        {
            base.SetUp();
            for (int i = 0; i < MeleeData.Strikes.Count; i++)
            {
                var strikeToCopy = MeleeData.Strikes[i];
                
                _strikes[i].StrikeTime = strikeToCopy.StrikeTime;
                _strikes[i].ID = strikeToCopy.ID;
            }
        }

        public override void StartCast()
       {
           base.StartCast();
           foreach(var strike in _strikes)
               strike.HasStruck = false;
           
           _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], _baseData.AbilityAnimationNumber);
           _brain.Movement.SetRotationState(RotationState.LookAtTarget, _target);
           
           SFXManagerWrapper.Instance.Play(_baseData.FindSFX("Swing"), transform.position, _brain.isLocalPlayer);
       }

       public override void Interrupt()
       {
           _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], 0);
           _brain.Movement.SetRotationState(RotationState.RegularInput, _target);
       }

       public override void ConfirmHit(int id)
       {
           
       }

       public override void Casting(float castTime)
       {
           base.Casting(castTime);

           foreach (var strike in _strikes)
           {
               if (strike.StrikeTime <= castTime && !strike.HasStruck)
               {
                   strike.HasStruck = true;
                   if(InteractionManager.Instance.CreateAndProcessInteraction(_brain, _target, this))
                       if(_brain.isServer)
                             _brain.Ability.ConfirmHit(strike.ID);
               }
           }
       }

       public override void EndCast()
       {
           base.EndCast();
           _brain.Animation.Animator.SetInteger(_baseData.Animations["Ability"], 0);
           _brain.Movement.SetRotationState(RotationState.RegularInput, _target);
       }
    }
}


