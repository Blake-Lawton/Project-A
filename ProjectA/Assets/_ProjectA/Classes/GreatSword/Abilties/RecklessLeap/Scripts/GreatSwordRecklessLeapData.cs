using _ProjectA.Scripts.Abilities;
using UnityEngine;

namespace _ProjectA.Classes.GreatSword.Abilties.RecklessLeap.Scripts
{
    [CreateAssetMenu(fileName = "GreatSwordRecklessLeapData", menuName = "Scriptable Objects/Abilities/GreatSword/GreatSwordRecklessLeapData")]
    public class GreatSwordRecklessLeapData : MovementAbilityData
    {
        [SerializeField] private GreatSwordRecklessLeap _ability;
        [SerializeField] private float _leapTime;
        [SerializeField] private AnimationCurve _leapHeight;

        public float LeapTime => _leapTime;
        public AnimationCurve LeapHeight => _leapHeight;
        public override BaseAbility EquippedAbility()
        {
            var ability = Instantiate(_ability);
            return ability;
        }
    }
}
