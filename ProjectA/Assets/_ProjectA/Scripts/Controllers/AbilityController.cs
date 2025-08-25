using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.UI;
using _ProjectA.Scripts.Util;
using Data.AbilityData;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ProjectA.Scripts.Controllers
{
    public class AbilityController : NetworkBehaviour
    {
        private PlayerBrain _brain;
        private PlayerInputActions _input;
        private InputAction _mouse;
        private Camera _camera;
        private TargetOutline _targetOutline;
        [SerializeField, ReadOnly]private PlayerBrain _target;
        [SerializeField] private List<BaseAbilityData> _abilityData;
        [SerializeField]private List<BaseAbility> _abilities;

        [SerializeField] private Transform _abilityHolder;
        
        [Header("Casting Bar")]
        [SerializeField, ReadOnly]  private bool _isCasting;
        [SerializeField, ReadOnly] private double _localCastStartTime;
        [SerializeField, ReadOnly] private double _serverCastStartTime;
        private NamePlate _namePlate;

        [Header("Ability")]
        [SerializeField, ReadOnly]  private float _globalCd;
        [SerializeField, ReadOnly] private BaseAbility _currentAbility;
        [SerializeField, ReadOnly] public bool OnGlobalCd => _globalCd > 0;
        public PlayerBrain Target => _target;
        public bool IsCasting => _isCasting;
        public bool HasTarget => _target != null;
        private TargetOutline TargetOutline => _targetOutline;

        private void Awake()
        {
            _brain = GetComponent<PlayerBrain>();
            _camera = Camera.main;
            _input = new PlayerInputActions();
            _mouse = _input.Player.Look;
            _input.Player.Target.performed += ctx => TargetPlayer(true);
            _input.Player.Ability1.performed += ctx => TryCast(0);
            _targetOutline = GetComponentInChildren<TargetOutline>();
            int i = 0;
            foreach (var abilityData in _abilityData)
            {
                var ability = abilityData.EquippedAbility();
                ability.transform.SetParent(_abilityHolder);
                _abilities.Add(ability);
                i++;
            }
        }


        public void SetUp(NamePlate namePlate)
        {
            _namePlate = namePlate;
        }
        
        private void Start()
        {
            if(isLocalPlayer)
                _input.Player.Enable();
        }

        [Client]
        private void TargetPlayer(bool isInput)
        {
            Vector2 mousePos = _mouse.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePos);
            
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out PlayerBrain playerBrain))
                {
                    if (_target != null)
                        _target.Ability.TargetOutline.UnTarget();
                    _target = playerBrain;
                    if(_target.Team == _brain.Team)
                        _target.Ability.TargetOutline.TargetTeammate();
                    else
                        _target.Ability.TargetOutline.TargetEnemy();
                }
                else
                {
                    if (isInput)
                    {
                        if (_target != null)
                            _target.Ability.TargetOutline.UnTarget();
                        _target = null;
                    }
                       
                }
                    
                
            }
            Debug.Log(" TARGET IS   = " + _target);
            
            
            if(!isInput)
                return;
            
            if (_target != null)
                TargetPlayerCmd(_target.NetworkIdentity);
            else
                UnTargetCmd();
        }


        
        
        [Command]
        private void TargetPlayerCmd(NetworkIdentity target)
        {
            if (target != null)
            {
                _target = target.GetComponent<PlayerBrain>();
            }
            else
            {
                target = null;
            }
        }

        [Command]
        private void UnTargetCmd()
        {
            _target = null;
        }
        
        
        private void TryCast(int abilityIndex)
        {
            
            var abilityToCast = _abilities[abilityIndex];
            if (abilityToCast.BaseData.RequireTarget)
                 TargetPlayer(false);
            
            if (abilityToCast.CanCastAbility(_target))
            {
                if(isLocalPlayer)
                    StartCastPrediction(abilityIndex);

                if (isServer)
                    ConfirmCastOnServer(abilityIndex);
            }
            
        }

        
        
        private void StartCastPrediction(int abilityIndex)
        {
            _currentAbility = _abilities[abilityIndex];
            _isCasting = true;
            
            _namePlate.StartCast(_currentAbility);
            _currentAbility.StartCast();
            _localCastStartTime = NetworkTime.time;

            if (_currentAbility.BaseData.RequireTarget)
            {
                TryCastCmd(abilityIndex, _target.NetworkIdentity);
            }
            else
            {
                TryCastCmd(abilityIndex, null);
            }
                
            
            
        }

        [Command]
        private void TryCastCmd(int abilityIndex, NetworkIdentity target)
        {
            if(target != null)
                _target = target.GetComponent<PlayerBrain>();
            
            TryCast(abilityIndex);
        }

        [Server]
        private void ConfirmCastOnServer(int abilityIndex)
        {
            _isCasting = true;
            _currentAbility = _abilities[abilityIndex];
            _currentAbility.StartCast();
            _serverCastStartTime = NetworkTime.time;
            ConfirmCastOnClients(_serverCastStartTime, abilityIndex, _target.NetworkIdentity);
        }

        [ClientRpc]
        private void ConfirmCastOnClients(double startTime, int abilityIndex, NetworkIdentity target)
        {
         
            if(isLocalPlayer)
                Debug.Log("ConfirmCastOnServer");
            else if (isClient)
                CastOnClients(abilityIndex, target);
            
            
            _serverCastStartTime = startTime;
        }

        [Client]
        private void CastOnClients(int abilityIndex, NetworkIdentity target)
        {
            _target = target.GetComponent<PlayerBrain>();
            _isCasting = true;
            _currentAbility = _abilities[abilityIndex];
            _currentAbility.StartCast();
            _namePlate.StartCast(_currentAbility);
        }

        public void Handle()
        {
            /*foreach (var ability in _abilities)
                ability.Handle();*/

            
            if(!_isCasting)
                return;

            
            var castBarPct = 0f;

            if (isLocalPlayer)
            {
                if (_brain.Movement.Moving && _currentAbility.BaseData.Interruptible)
                    InterruptCastLocal();
                
                double elapsed = NetworkTime.time - _localCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.BaseData.CastTime);
            }


            if (isServer)
            {
                double elapsed = NetworkTime.time - _serverCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.BaseData.CastTime);
            }
            
            
            if (isClient && !isLocalPlayer)
            {
                double elapsed = NetworkTime.time - _serverCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.BaseData.CastTime);
            }
            
            _namePlate.UpdateCastBar(castBarPct);

            if (castBarPct >= 1f)
                CompleteCast();
        }

        private void InterruptCastLocal()
        {
            _currentAbility.Interupt();
            _isCasting = false;
            _namePlate.Interrupt();
            InterruptCastCmd();
        }

        [Command]
        private void InterruptCastCmd()
        {
            _currentAbility.Interupt();
            _isCasting = false;
            _namePlate.Interrupt();
            InterruptCastRpc();
        }

        [ClientRpc(includeOwner = false)]
        private void InterruptCastRpc()
        {
            _isCasting = false;
            _namePlate.Interrupt();
            _currentAbility.Interupt();
        }

        private void CompleteCast()
        {
            _namePlate.CompleteCast();
            _currentAbility.EndCast();
            _isCasting = false;
            
        }

        public void EquippedAbility(BaseAbility ability)
        {
            
        }
    }
}
