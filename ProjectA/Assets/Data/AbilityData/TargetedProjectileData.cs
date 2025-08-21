using _ProjectA.Scripts.Abilities;
using Data.Interaction;
using UnityEngine;

namespace Data.AbilityData
{
    [CreateAssetMenu(fileName = "ProjectileAbilityData", menuName = "Scriptable Objects/Projectile Ability Data")]
    public class TargetedProjectileData : BaseAbilityData
    {
        [Header("Projectile Data")] 
        [SerializeField] private TargetedProjectile _projectile;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        
        public int Damage => _damage;
        public TargetedProjectile Projectile => _projectile;
        public float Speed => _speed;
        
        
        public override void ResolveEffect(InteractionData data)
        {
            if (data.Perp != null && data.Victim != null)
            {
                data.Victim.Health.TakeDamage(data.Perp.Status.DamageAmp * _damage);
            }
        }
    }
}
