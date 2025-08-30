using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.TargtedMeleeAbiltiy;
using UnityEngine;


namespace _ProjectA.Data.AbilityData
{
    [CreateAssetMenu(fileName = "TargetedMeleeData", menuName = "Scriptable Objects/Abilities/TargetedMeleeData")]
    public class TargetedMeleeData : TargetedAbilityData
    {
        [Header("Melee Data")]
        [SerializeField] private BasicTargetedMeleeAbility _ability;

        [SerializeField] private float _strikeTime;
        
        public float StrikeTime => _strikeTime;
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
        
    }
}
