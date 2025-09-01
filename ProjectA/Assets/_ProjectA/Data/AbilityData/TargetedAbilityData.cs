using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.Mage;
using Data.AbilityData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Data.AbilityData
{
    public abstract class TargetedAbilityData : BaseAbilityData
    {
        
        [Title("Targeted Data")]
        [SerializeField] private float _range;
        
        public float Range => _range;
        
       
    }
}
