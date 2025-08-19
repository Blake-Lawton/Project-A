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
         
         //hit some sort of environnnment
         if (data.Victim == null)
         {
            return;
         }
         
         if (data.Victim.Brain.Health.IsDead)
         {
            return;
         }
         
         //hit a teammate 
         if (data.Victim.Brain.Team == data.Perp.Brain.Team && !data.Victim.Brain.Health.IsDead)
         {
            if (isServer)
            {
               //if Hit self and server then apply force
               if (data.Victim.Brain == data.Perp.Brain)
               {
                  
               }
            }
            return;
         }
          
         // hit player
         var perpBrain = data.Perp.Brain;
         if (data.Victim.Brain.Team != perpBrain.Team && !data.Victim.Brain.Health.IsDead)
         {
            if (perpBrain.isLocalPlayer)
            {
               return;
            }
            
            // if another player hits another player
            if (perpBrain.isClient)
            {
               return;
            }
            
            //if hit on server
            if (perpBrain.isServer)
            {
               data.Victim.TakeDamage(data);
            }
         }
      }
   }
}
