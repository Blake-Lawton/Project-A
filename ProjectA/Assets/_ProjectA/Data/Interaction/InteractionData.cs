

using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using _ProjectA.Scripts.Interface;

namespace Data.Interaction
{
    public class InteractionData
    {
        private PlayerBrain _perp;
        private PlayerBrain _victim;
        private BaseAbility _ability;
        public PlayerBrain Perp => _perp;
        public PlayerBrain Victim => _victim;
        public BaseAbility Ability => _ability;
        public InteractionData(PlayerBrain perp, PlayerBrain victim, BaseAbility ability)
        {
           _perp = perp;
           _victim = victim;
           _ability = ability;
        }
        
    }
}
