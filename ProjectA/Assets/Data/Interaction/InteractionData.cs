

using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using _ProjectA.Scripts.Interface;

namespace Data.Interaction
{
    public class InteractionData
    {
        private PlayerBrain _perp;
        private PlayerBrain _victim;
        private BaseAbilityData _abilityData;
        public PlayerBrain Perp => _perp;
        public PlayerBrain Victim => _victim;
        public BaseAbilityData AbilityData => _abilityData;
        public InteractionData( PlayerBrain perp, PlayerBrain victim, BaseAbilityData abilityData)
        {
           _perp = perp;
           _victim = victim;
           _abilityData = abilityData;
        }
        
    }
}
