using System;
using System.Collections.Generic;
using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Interface;
using _ProjectA.Scripts.UI.AbilitiyUIController;
using _ProjectA.Scripts.Util;
using Data.Interaction;
using Data.Types;
using Mirror;
using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Abilities.TargtedMeleeAbiltiy
{
    public class BasicTargetedMeleeAbility : TargetedAbility
    {
         protected TargetedMeleeData TargetedMeleeData => (TargetedMeleeData)_baseData;
       
        
     

        public override void UseAbility() { }

        public override void StartCast()
        {
            base.StartCast();
           
        }

        public override void EndAbility() { }

        public override void Interrupt()
        {
            
        }

     

        
        public override void EndCast()
        {
            _brain.Movement.ChangeRotationState(RotationState.RegularInput);
        }

        public override void ConfirmHit()
        {
            var joint = _brain.JointFinder.FindJoint(Joint.SwordTip).transform;
            TargetedMeleeData.VFX["Strike"].SpawnVFX(joint);
            SFXManager.Main.Play(TargetedMeleeData.SFX["Impact"]);
        }

       
       
    }
    
    
    
}


