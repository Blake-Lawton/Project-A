using _ProjectA.Data.AbilityData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Abilities.Mage;
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
        
        [Header("Animation")]
        [SerializeField] private string _startCastTrigger = "StartCast";
        [SerializeField] private string _endCastTrigger = "EndCast";
        
        [Header("VFX")]
        [SerializeField] private ParticleSystem _castingVFXPrefab;
        [SerializeField] private ParticleSystem _onHitVFXPrefab;

        [Header("SFX")]
        [ExternPropertyAttributes.InfoBox("The casting Loop is found on the casting VFX because the audio system is ass")]
        [SerializeField] private SFXObject _castedSFX;
        [SerializeField] private SFXObject _onHitSFX;
        public TargetedProjectile Projectile => _projectile;
        public float Speed => _speed;
        public string StartCastTrigger => _startCastTrigger;
        public string EndCastTrigger => _endCastTrigger;
        public ParticleSystem CastingVFXPrefab => _castingVFXPrefab;
        public ParticleSystem OnHitVFXPrefab => _onHitVFXPrefab;
        public SFXObject CastedSFX => _castedSFX;
        public SFXObject OnHitSFX => _onHitSFX;
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
    }
}
