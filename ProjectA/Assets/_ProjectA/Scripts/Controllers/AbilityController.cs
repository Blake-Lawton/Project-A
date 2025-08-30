using System;
using System.Collections.Generic;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.UI;
using _ProjectA.Scripts.Util;
using Data.AbilityData;
using Data.Types;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
        
        private NamePlate _namePlate;


        [Header("Ability")]
        [SerializeField] private float _globalCd = .75f;
        [SerializeField, ReadOnly]  private float _currentGlobalCd;
        [SerializeField, ReadOnly] private BaseAbility _currentAbility;
        [SerializeField, ReadOnly] private float _castTime;
        public float CastTime => _castTime;
        [SerializeField, ReadOnly] public bool OnGlobalCd => _currentGlobalCd > 0;
        public PlayerBrain Target => _target;
        public bool IsCasting => _isCasting;
        public bool HasTarget => _target != null;
        private TargetOutline TargetOutline => _targetOutline;
        public float CurrentGlobalCd => _currentGlobalCd;
        public float GlobalCd => _globalCd;

        private void Awake()
        {
            
            _camera = Camera.main;
            _input = new PlayerInputActions();
            _mouse = _input.Player.Look;
            _input.Player.Target.performed += ctx => TargetPlayer(true);
            _input.Player.Ability1.performed += ctx => TryCast(0);
            _input.Player.Ability2.performed += ctx => TryCast(1);
           
        }


        public void SetUp(NamePlate namePlate)
        {
            _namePlate = namePlate;

            int i = 0;
            foreach (var abilityData in _abilityData)
            {
                var ability = abilityData.EquippedAbility();
                ability.transform.SetParent(_abilityHolder);
                ability.SetUp();
                ability.SetUpUI(PlayerUICanvas.Instance.AbilityUIController.AbilityIcons[i]);
                _abilities.Add(ability);
                i++;
            }
            _brain = GetComponent<PlayerBrain>();
            _targetOutline = GetComponentInChildren<TargetOutline>();
            Debug.Log(_targetOutline.name);
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


            switch (abilityToCast.CanCastAbility(_target, _currentAbility) )
            {
                case CastResult.Success:
                    if(!CheckStatus())
                        return;
                    
                    _currentGlobalCd = _globalCd;
                
                    if(isLocalPlayer)
                        StartCastPrediction(abilityIndex);

                    if (isServer)
                        ConfirmStartCastOnServer(abilityIndex);
                    
                    
                    break;
                case CastResult.NoTarget:
                    Debug.LogWarning("Casting Failed: No Target");
                    break;
                case CastResult.CantTargetSelf:
                    Debug.LogWarning("Casting Failed: Cannot Target Self");
                    break;
                case CastResult.CantTargetEnemy:
                    Debug.LogWarning("Casting Failed: Cannot Target Enemy");
                    break;
                case CastResult.CantTargetAlly:
                    Debug.LogWarning("Casting Failed: Cannot Target Ally");
                    break;
                case CastResult.OutOfRange:
                    Debug.LogWarning("Casting Failed: Target Out of Range");
                    break;
                case CastResult.OnCooldown:
                    Debug.LogWarning("Casting Failed: Ability On Cooldown");
                    break;
                case CastResult.CrowdControlled:
                    Debug.LogWarning("Casting Failed: Caster is Crowd Controlled");
                    break;
                case CastResult.Moving:
                    Debug.LogWarning("Casting Failed: Cannot Cast While Moving");
                    break;
                case CastResult.IsCastingAlready:
                    Debug.LogWarning("Casting Failed: Cannot Cast While Casting");
                    break;
                case CastResult.OnGlobalCd:
                    Debug.LogWarning("Casting Failed: Cannot Cast While Global CD");
                    break;
                case CastResult.NotInFront:
                    Debug.LogWarning("Casting Failed: Target is not in front of player");
                    break;
                case CastResult.CantInterrupt:
                    Debug.LogWarning("Casting Failed: Cannot Interrupt Current Ability");
                    break;
                default:
                    Debug.LogWarning("Casting Failed: Unknown Reason");
                    break;
            }
           
            
        }

        private bool CheckStatus()
        {
            var status = _brain.Status;
            if (status.Stunned)
            {
                Debug.LogWarning("Casting Failed: Stunned");
                return false;
            }
            return true;
        }
        
        private void StartCastPrediction(int abilityIndex)
        {
            _currentAbility = _abilities[abilityIndex];
            _isCasting = true;
            
            _namePlate.StartCast(_currentAbility);
            _currentAbility.StartCast();
           

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
        private void ConfirmStartCastOnServer(int abilityIndex)
        {
            _isCasting = true;
            _currentAbility = _abilities[abilityIndex];
            _currentAbility.StartCast();
            if(!isServer)
                ConfirmStartCastOnClients(abilityIndex, _target.NetworkIdentity);
        }

        [ClientRpc]
        private void ConfirmStartCastOnClients(int abilityIndex, NetworkIdentity target)
        {
            if(isLocalPlayer)
                Debug.Log("ConfirmCastOnServer");
            else if (isClient)
                CastOnClients(abilityIndex, target);
            
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
            foreach (var ability in _abilities)
                ability.Handle();

            _currentGlobalCd -= Time.deltaTime;
            
            
            if(!_isCasting)
                return;
            
            
            _castTime += Time.deltaTime;
            _currentAbility.Casting(_castTime);
            
            var castBarPct = _castTime / _currentAbility.BaseData.CastTime;
            
            _namePlate.UpdateCastBar(castBarPct);

            if (castBarPct >= 1f)
                CompleteCast();
        }

        public void InterruptCastLocal()
        {
            Debug.Log("WE INTERUPPEt");
            _currentAbility.Interrupt();
            _isCasting = false;
            _castTime = 0;
            _namePlate.Interrupt();
            InterruptCastCmd();
        }

        [Command]
        private void InterruptCastCmd()
        {
            _currentAbility.Interrupt();
            _isCasting = false;
            _castTime = 0;
            _namePlate.Interrupt();
            InterruptCastRpc();
        }

        [ClientRpc(includeOwner = false)]
        private void InterruptCastRpc()
        {
            _isCasting = false;
            _namePlate.Interrupt();
            _currentAbility.Interrupt();
            _castTime = 0;
        }

        private void CompleteCast()
        {
            _namePlate.CompleteCast(_currentAbility);
            _currentAbility.EndCast();
            _isCasting = false;
            _castTime = 0;

        }
        
        [ClientRpc]
        public void ConfirmHit()
        {
            _currentAbility.ConfirmHit();
        }
    }
}
