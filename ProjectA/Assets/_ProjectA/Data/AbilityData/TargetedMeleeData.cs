using System.Collections.Generic;
using _ProjectA.Data.Types;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.BaseClasses;
using Data.Types;
using UnityEngine;


namespace _ProjectA.Data.AbilityData
{
    [CreateAssetMenu(fileName = "TargetedMeleeData", menuName = "Scriptable Objects/Abilities/Targeted Melee Data")]
    public class TargetedMeleeData : TargetedAbilityData
    {
        [Header("Melee Data")]
        [SerializeField] private TargetedAbility _ability;
        
        public List<StrikeData> Strikes = new List<StrikeData>();
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
        
    }
}
