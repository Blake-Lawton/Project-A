using Data.Types;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities.BaseClasses
{
    public abstract class MovementAbility : BaseAbility
    {
        protected MovementAbilityData MovementAbilityData => (MovementAbilityData)_baseData;


        public override void SetUp()
        {
            base.SetUp();
            _brain.client.Tick += OnTick;
        }

        protected abstract void OnTick(float sendRate);

        protected abstract void EndMovement();


    }
}
