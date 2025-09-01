using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.Mage;
using UnityEngine;


namespace _ProjectA.Data.AbilityData
{
    [CreateAssetMenu(fileName = "TargetedMeleeData", menuName = "Scriptable Objects/Abilities/Targeted Melee Data")]
    public class TargetedMeleeData : TargetedAbilityData
    {
        [Header("Melee Data")]
        [SerializeField] private TargetedAbility _ability;

        [SerializeField] private float _strikeTime;
        
        public float StrikeTime => _strikeTime;
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
        
    }
}
