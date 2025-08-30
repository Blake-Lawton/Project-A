using Data.AbilityData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Data.AbilityData
{
    [CreateAssetMenu(fileName = "TargetedAbilityData", menuName = "Scriptable Objects/AbilityData/TargetedProjectileAbility")]
    public abstract class TargetedAbilityData : BaseAbilityData
    {
        
        [Title("Targeted Data")]
        [SerializeField] private float _range;
        
        public float Range => _range;
    }
}
