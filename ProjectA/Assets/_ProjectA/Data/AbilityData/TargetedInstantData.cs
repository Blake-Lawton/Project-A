using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.Mage;
using UnityEngine;

namespace _ProjectA.Data.AbilityData
{
    [CreateAssetMenu(fileName = "TargetedInstantData", menuName = "Scriptable Objects/Abilities/Targeted Instant Data")]
    public class TargetedInstantData : TargetedAbilityData
    {
        [SerializeField]private TargetedAbility _ability;
        
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
    }
}
