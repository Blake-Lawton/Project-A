using System;
using _ProjectA.Scripts.Controllers;

namespace _ProjectA.Scripts.Interface
{
   
    [Serializable]
    public abstract class AbilityModuleBase
    {
        public abstract void OnStartCast(PlayerBrain player, PlayerBrain target);
        public abstract void OnEndCast(PlayerBrain player, PlayerBrain target);
    
    }
}
