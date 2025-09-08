using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI.AbilitiyUIController;
using _ProjectA.Scripts.Util;
using Data.AbilityData;
using Data.Interaction;
using Data.Types;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ProjectA.Scripts.Abilities
{
    public abstract class BaseAbility : SerializedMonoBehaviour
    {
        [SerializeField]protected BaseAbilityData _baseData;
        protected PlayerBrain _brain;
        protected float _cooldown;
        protected AbilityIcon _ui;

        public bool OnCooldown => _cooldown > 0;
        public float Cooldown => _cooldown;
        public AbilityIcon UI => _ui;
        public BaseAbilityData BaseData => _baseData;

        public virtual void SetUpUI(AbilityIcon ui)
        {
            if (_brain.isLocalPlayer)
            {
                _ui = ui;
                _ui.SetUI(this, _brain);
            }
                
        }

      
        public abstract void StartCast();
      
        public abstract void Interrupt();
        public abstract void ConfirmHit(int id);

        public virtual CastResult CanCastAbility(PlayerBrain target, BaseAbility previousAbility)
        {
            if(!CorrectTarget(target, out CastResult result)) return result;
            if (OnCooldown) return CastResult.OnCooldown;
            if(_baseData.StandStill)
                if (_brain.Movement.Moving) return CastResult.Moving;
            if (_baseData.IsGlobal) 
                if (_brain.Ability.OnGlobalCd) return CastResult.OnGlobalCd;
            if (previousAbility != null && !previousAbility._baseData.Interruptible && _brain.Ability.IsCasting) return CastResult.CantInterrupt;
            if(!_baseData.CanInterrupt && _brain.Ability.IsCasting) return CastResult.IsCastingAlready;

            return CastResult.Success;
        }


        protected virtual void UpdateCooldown()
        {
            _cooldown -= Time.deltaTime;
        }

        

        public virtual void HandleCoolDown()
        {
            
            UpdateCooldown();
            if (_brain.isLocalPlayer)
                _ui.UpdateCd();
        }


        public virtual void Casting(float castTime)
        {
            if (_brain.Movement.Moving && _baseData.Interruptible && _brain.Ability.IsCasting)
                _brain.Ability.InterruptCastLocal();
        }

        public virtual void EndCast()
        {
            _cooldown = _baseData.Cooldown;
        }


        
        public virtual bool CorrectTarget(PlayerBrain target, out CastResult result)
        {

            if (!_baseData.RequireTarget)
            {
                result = CastResult.Success;
                return true;
            }
            if (target == null)
            {
                result = CastResult.NoTarget;
                return false;
            }

            if (target == _brain && _baseData.TargetSelf)
            {
                result = CastResult.Success;
                return true;
            }
            switch (_baseData.TargetType)
            {
                case TargetType.Ally:
                    if (_brain == target) // caster is trying to target self
                    {
                        result = CastResult.CantTargetSelf;
                        return false;
                    }

                    if (_brain.OnSameTeam(target))
                    {
                        result = CastResult.Success;
                        return true;
                    }
                    else
                    {
                        result = CastResult.CantTargetEnemy;
                        return false;
                    }

                case TargetType.Enemy:
                    if (_brain == target) // self-targeting not allowed for enemies
                    {
                        result = CastResult.CantTargetSelf;
                        return false;
                    }

                    if (!_brain.OnSameTeam(target))
                    {
                        result = CastResult.Success;
                        return true;
                    }
                    else
                    {
                        result = CastResult.CantTargetAlly;
                        return false;
                    }

                case TargetType.Both:
                    if (_brain == target)
                    {
                        result = CastResult.CantTargetSelf;
                        return false;
                    }

                    result = CastResult.Success;
                    return true;

                default:
                    result = CastResult.NoTarget;
                    return false;
            }
        }

        public virtual void SetUp()
        {
            _brain = GetComponentInParent<PlayerBrain>();
        }

        public  virtual void ReceiveVector3(Vector3 mouse)
        {
            
        }
    }
}
