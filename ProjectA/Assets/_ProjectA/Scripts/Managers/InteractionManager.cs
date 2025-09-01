using System;
using _ProjectA.Scripts.Abilities;
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
      public void ProcessInteraction(InteractionData data)
      {
         if (!data.Perp || !data.Victim)
         {
            Debug.LogError("PERP OR VICTIM NULL");
            return;
         }

         if ( data.Victim.Health.IsDead)
         {
            Debug.LogError("VICTIM IS DEAD");
            return;
         }
           

         BaseAbilityData abilityData = data.Ability.BaseData;
         if (isServer)
         {
            switch (abilityData.Type)
            {
               case AbilityType.Damage:
                  data.Victim.Health.TakeDamage(abilityData.HealthChange);
                  InteractionNumbersManager.Instance.DisplayDamage(abilityData.HealthChange, data);
                  break;
               case AbilityType.Heal:
                  data.Victim.Health.Heal(abilityData.HealthChange);
                  InteractionNumbersManager.Instance.DisplayHeal(abilityData.HealthChange, data);
                  break;
               case AbilityType.Shield:
                  data.Victim.Health.GiveShield(abilityData.HealthChange, data);
                  InteractionNumbersManager.Instance.DisplayShield(abilityData.HealthChange, data);
                  break;
            }
         }
         foreach (var status in abilityData.StatusEffects)
            data.Victim.Status.ApplyStatus(status, data);
         
      }
   }
   
   
   
}
