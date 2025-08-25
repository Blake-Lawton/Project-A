using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using Data.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.AbilityData
{
    [CreateAssetMenu(fileName = "ProjectileAbilityData", menuName = "Scriptable Objects/Projectile Ability Data")]
    public class TargetedProjectileData : BaseAbilityData
    {
        [Header("Projectile Data")] 
        [SerializeField] private TargetedProjectileAbility _ability;
        [SerializeField] private TargetedProjectile _projectile;
       
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        
        [Header("Animation")]
        [SerializeField] private string _startCastTrigger = "StartCast";
        [SerializeField] private string _endCastTrigger = "EndCast";
        
        [Header("VFX")]
        [SerializeField] private ParticleSystem _castingVFXPrefab;
        [SerializeField] private ParticleSystem _onHitVFXPrefab;
        public TargetedProjectile Projectile => _projectile;
        public float Speed => _speed;
        public int Damage => _damage;
        public string StartCastTrigger => _startCastTrigger;
        public string EndCastTrigger => _endCastTrigger;
        public ParticleSystem CastingVFXPrefab => _castingVFXPrefab;
        public ParticleSystem OnHitVFXPrefab => _onHitVFXPrefab;
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
    }
}
