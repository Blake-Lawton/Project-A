using _ProjectA.Scripts.Controllers;
using UnityEngine;

namespace _ProjectA.Scripts.Interface
{
   public interface IDamager 
   {
      public PlayerBrain Brain { get; set; }
   }
}
