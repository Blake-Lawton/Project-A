using _BazookaBrawl.Scripts.Manager;
using Data.Interaction;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming

namespace _ProjectA.Managers
{
   public class InteractionManager : NetworkBehaviour
   {
      //create singleton
      public static InteractionManager Instance;

      [SerializeField] private InteractionNumbersManager _numbers;
      
      public void Awake()
      {
         Instance = this;
      }


      // a seciton for on server
      // seciton for local player
      public void ProcessInteraction(InteractionData data)
      {
         
         if (data.Victim == null || data.Victim.Health.IsDead) 
            return;

         if (data.Perp.isServer)
         {
            // Defer to the ability itself to decide what happens
            data.AbilityData.ResolveEffect(data);
         }
        
      }
   }
}
