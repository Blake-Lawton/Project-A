using System;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Data.Interaction;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.Abilities
{
    public abstract class BaseAbility : MonoBehaviour
    {
        protected PlayerBrain _brain;
        protected BaseAbilityData _baseData;
        protected float _cooldown;
      
      

        public bool OnCooldown => _cooldown > 0;
        public float Cooldown => _cooldown;
        public BaseAbilityData BaseData => _baseData;
        

        public abstract void UseAbility();
        public abstract void StartCast();
        public abstract void EndAbility();
        public abstract void Interupt();
        public abstract bool CanCastAbility(PlayerBrain brain);
        
        public virtual void UpdateCooldown()
        {
            _cooldown -= Time.deltaTime;
        }
        protected virtual void Start()
        {
            _brain = GetComponentInParent<PlayerBrain>();
        }

        public virtual void Handle()
        {
            UpdateCooldown();
        }

        public abstract void EndCast();


        public abstract void ResolveAbility(InteractionData interaction);
    }
}
