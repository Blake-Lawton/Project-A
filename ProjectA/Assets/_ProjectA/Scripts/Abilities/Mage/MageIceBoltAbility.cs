using System;
using Data.AbilityData;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities.Mage
{
    public class MageIceBoltAbility : BaseAbility
    {
        [SerializeField] protected TargetedProjectileAbilityData _iceBoltData;

        private void Awake()
        {
            _data = _iceBoltData;
        }

        public override void UseAbility()
        {
            Debug.Log("USED MAGE ICE BOLT");
        }
    }
}
