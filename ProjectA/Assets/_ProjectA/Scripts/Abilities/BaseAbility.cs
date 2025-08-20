using _ProjectA.Scripts.Abilities.Mage;
using Data.AbilityData;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities
{
    public abstract class BaseAbility : NetworkBehaviour
    {
        protected AbilityData _data;
        public AbilityData Data => _data;
        public abstract void UseAbility();
    }
}
