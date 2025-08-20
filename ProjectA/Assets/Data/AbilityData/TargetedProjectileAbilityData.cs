using UnityEngine;

namespace Data.AbilityData
{
    [CreateAssetMenu(fileName = "ProjectileAbilityData", menuName = "Scriptable Objects/Projectile Ability Data")]
    public class TargetedProjectileAbilityData : AbilityData
    {
        [Header("Projectile Data")] 
        [SerializeField]private int _damage;
        
        public int Damage => _damage;
    }
}
