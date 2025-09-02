using System;
using _ProjectA.Classes.GreatSword.Abilties.RuinousBlow.Scripts;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Managers;
using Data.AbilityData;
using Data.Interaction;
using Data.Types;
using Mirror;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace _ProjectA.Managers
{
   public class InteractionManager : NetworkBehaviour
   {
      //create singleton
      public static InteractionManager Instance;

      
      public void Awake()
      {
         Instance = this;
      }


      // a seciton for on server
      // seciton for local player
      public bool ProcessInteraction(InteractionData data, int interactionNumber = 0)
      {

         if (interactionNumber == 0)
         {
            interactionNumber = data.Ability.BaseData.InteractionNumber;
         }
         if (!data.Perp || !data.Victim)
         {
            Debug.LogError("PERP OR VICTIM NULL");
            return false;
         }

         if ( data.Victim.Health.IsDead)
         {
            Debug.LogError("VICTIM IS DEAD");
            return false;
         }
           

         BaseAbilityData abilityData = data.Ability.BaseData;


         if (data.Victim.Status.FindIncreaseDamageFromSource(data.Perp, out float increase))
         {
            interactionNumber = (int)((1f + increase) * interactionNumber);
         }
            
         if (isServer)
         {
            switch (abilityData.Type)
            {
               case AbilityType.Damage:
                  data.Victim.Health.TakeDamage(interactionNumber);
                  InteractionNumbersManager.Instance.DisplayInteraction(interactionNumber, data);
                  break;
               case AbilityType.Heal:
                  data.Victim.Health.Heal(interactionNumber);
                  InteractionNumbersManager.Instance.DisplayInteraction(interactionNumber, data);
                  break;
               case AbilityType.Shield:
                  data.Victim.Health.GiveShield(interactionNumber, data);
                  InteractionNumbersManager.Instance.DisplayInteraction(interactionNumber, data);
                  break;
            }
         }
         foreach (var status in abilityData.StatusEffects)
            data.Victim.Status.ApplyStatus(status, data);
         return true;
      }

      public bool CreateAndProcessInteraction(PlayerBrain brain, PlayerBrain target, BaseAbility ability, int healthChange = 0)
      {
         InteractionData interactionData = new InteractionData(brain, target, ability);
         return ProcessInteraction(interactionData, healthChange);
      }
   }
   
   
   
}
