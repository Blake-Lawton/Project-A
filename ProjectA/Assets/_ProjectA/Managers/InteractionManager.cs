using System;
using Data.Interaction;
using Mirror;

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
         
         if (data.Victim == null || data.Victim.Health.IsDead) 
            return;

         if (data.Perp.isServer)
         {
            data.Ability.ResolveAbility(data);
         }
      }
   }
   
   
   
}
