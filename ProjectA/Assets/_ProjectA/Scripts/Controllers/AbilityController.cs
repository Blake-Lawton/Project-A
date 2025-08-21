using System;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.UI;
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
        [SerializeField, ReadOnly]private PlayerBrain _target;
        
        [SerializeField]private BaseAbility[] _abilities;
        
     
        
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

        private void Awake()
        {
            _brain = GetComponent<PlayerBrain>();
            _camera = Camera.main;
            _input = new PlayerInputActions();
            _mouse = _input.Player.Look;
            _input.Player.Target.performed += ctx => TargetPlayer();
            _input.Player.Ability1.performed += ctx => TryCast(0);
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
        private void TargetPlayer()
        {
            Vector2 mousePos = _mouse.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePos);
            
            // Raycast down from the camera
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out PlayerBrain playerBrain))
                    _target = playerBrain;
                else
                    _target = null;
                
            }
            Debug.Log(" TARGET IS   = " + _target);
            if(_target != null)
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
            
            if (abilityToCast.CanCastAbility())
            {
                if(isLocalPlayer)
                    StartCastPrediction(abilityIndex);

                if (isServer)
                    ConfirmCastOnServer(abilityIndex);
            }
            else
            {
                /*if(isLocalPlayer)
                    CantStartCast(abilityIndex);

                if (isServer)
                    FailedToCast(abilityIndex);*/
            }
            
        }

        
        [Client]
        private void StartCastPrediction(int abilityIndex)
        {
            _isCasting = true;
            _currentAbility = _abilities[abilityIndex];
            _namePlate.StartCast(_currentAbility);
            _currentAbility.StartCast();
            _localCastStartTime = NetworkTime.time;
            TryCastCmd(abilityIndex);
        }

        [Command]
        private void TryCastCmd(int abilityIndex)
        {
            TryCast(abilityIndex);
        }

        [Server]
        private void ConfirmCastOnServer(int abilityIndex)
        {
            _isCasting = true;
            _currentAbility = _abilities[abilityIndex];
            _currentAbility.StartCast();
            _serverCastStartTime = NetworkTime.time;
            ConfirmCastOnClients(_serverCastStartTime, abilityIndex);
        }

        [ClientRpc]
        private void ConfirmCastOnClients(double startTime, int abilityIndex)
        {
           // if (isLocalPlayer)
                //kinda do something here yeahn?
            if (isClient)
                CastOnClients(abilityIndex);
            
            
            _serverCastStartTime = startTime;
        }

        [Client]
        private void CastOnClients(int abilityIndex)
        {
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
                if (_brain.Movement.Moving && _currentAbility.Data.Interruptible)
                    InterruptCastLocal();
                
                double elapsed = NetworkTime.time - _localCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.Data.CastTime);
            }


            if (isServer)
            {
                double elapsed = NetworkTime.time - _serverCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.Data.CastTime);
            }
            
            
            if (isClient && !isLocalPlayer)
            {
                double elapsed = NetworkTime.time - _serverCastStartTime;
                castBarPct = (float)(elapsed / _currentAbility.Data.CastTime);
            }
            
            _namePlate.UpdateCastBar(castBarPct);

            if (castBarPct >= 1f)
                CompleteCast();
        }

        private void InterruptCastLocal()
        {
            _isCasting = false;
            _namePlate.Interrupt();
            InterruptCastCmd();
        }

        [Command]
        private void InterruptCastCmd()
        {
            _isCasting = false;
            _namePlate.Interrupt();
            InterruptCastRpc();
        }

        [ClientRpc(includeOwner = false)]
        private void InterruptCastRpc()
        {
            _isCasting = false;
            _namePlate.Interrupt();
        }

        private void CompleteCast()
        {
            _namePlate.CompleteCast();
            _currentAbility.EndCast();
            _isCasting = false;
            
        }
    }
}
