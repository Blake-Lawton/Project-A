using System;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.Abilities
{
    public abstract class BaseAbility : NetworkBehaviour
    {
        protected PlayerBrain _brain;
        protected BaseAbilityData _data;
        protected float _cooldown;
        [Header("UI")]
        [SerializeField]private Image _icon;
        [SerializeField]private Image _fill;
        [SerializeField]private Text _cooldownText;

        public bool OnCooldown => _cooldown > 0;
        public float Cooldown => _cooldown;
        public BaseAbilityData Data => _data;
        public abstract void UseAbility();
        public abstract void StartCast();
        public abstract void EndAbility();
        
        public abstract bool CanCastAbility();
        public virtual void SetUpAbility()
        {
            _icon.sprite = _data.Sprite;
        }
        
        public virtual void UpdateCooldown()
        {
            _cooldown -= Time.deltaTime;
            _fill.fillAmount = _cooldown / _data.Cooldown;
            _cooldownText.text = _cooldown.ToString();
        }
        protected virtual void Awake()
        {
            _brain = GetComponentInParent<PlayerBrain>();
        }

        public virtual void Handle()
        {
            UpdateCooldown();
        }

        public abstract void EndCast();

    }
}
