using System;
using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI.AbilitiyUIController;
using AMPInternal;
using Data.AbilityData;
using Data.Interaction;
using Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Abilities.Mage
{
    public abstract class TargetedAbility : BaseAbility
    {
       
        protected PlayerBrain _target;
        protected TargetedAbilityData TargetData => (TargetedAbilityData)_baseData;


     

        public override void StartCast()
        {
            _target = _brain.Ability.Target;
            _cooldown = _baseData.Cooldown;
        }
        
        
        public override CastResult CanCastAbility(PlayerBrain target, BaseAbility previousAbility)
        {
            var castResult =  base.CanCastAbility(target, previousAbility);
            if(castResult != CastResult.Success)
                return castResult;
            if (!TargetInFront(target)) return CastResult.NotInFront;
            if (!TargetInRange(target)) return CastResult.OutOfRange;

            return castResult;

        }

        public override void Interrupt()
        {
            
        }

        #region Target Stuff

        protected bool TargetInRange(PlayerBrain target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= TargetData.Range)
                return true;
            return false;
        }
        
        protected bool TargetInFront(PlayerBrain target)
        {
           
            Vector3 toTarget = (target.transform.position - transform.position).normalized;
            Vector3 forward = transform.forward.normalized;

            // 180° cone = just check if the target is not behind
            return Vector3.Dot(forward, toTarget) >= 0f;
        }

        #endregion
       
    }
}
