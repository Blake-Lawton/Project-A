using _ProjectA.Scripts.Abilities.BaseClasses;
using Data.AbilityData;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities
{
    public abstract class MovementAbilityData : BaseAbilityData
    {
        

        [SerializeField] private float _range;
        
        
        public float Range => _range;
       
        
        
    }
}
