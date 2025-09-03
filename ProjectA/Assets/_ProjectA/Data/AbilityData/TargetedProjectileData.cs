using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.BaseClasses;
using _ProjectA.Scripts.Controllers;
using Data.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.AbilityData
{
    [CreateAssetMenu(fileName = "ProjectileAbilityData", menuName = "Scriptable Objects/Abilities/Projectile Ability Data")]
    public class TargetedProjectileData : TargetedAbilityData
    {
        [Title("Projectile Data")] 
        [SerializeField] private TargetedAbility _ability;
        [SerializeField] private TargetedProjectile _projectile;
       
        [SerializeField] private float _speed;
        
        public TargetedProjectile Projectile => _projectile;
        public float Speed => _speed;
      
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
    }
}
