using System.Collections;
using _ProjectA.Scripts.Abilities;
using Data.Types;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _ProjectA.Scripts.Controllers
{
    public class MovementController : NetworkBehaviour 
    {
        //Components
        private PlayerBrain _brain;
        private CharacterController _cc;
        //Inputs
        private PlayerInputActions _input;
        private InputAction _movement;
     
        private InputAction _mouse;
        
     
        [Header("Movement")]
        [SerializeField, Sirenix.OdinInspector.ReadOnly] private Vector3 _finalVelocity;
        private Vector3 _currentVelocity;
        private Vector3 _moveDirection;
     
        private Vector2 _mouseDelta;

        [Header("States")] 
        [SerializeField, ReadOnly] private RotationState _rotationState;
        [SerializeField, ReadOnly] private MovementState _movementState;
        private PlayerBrain _target;
        public Vector3 MoveDirection => _moveDirection;
        public Vector3 Rotation => transform.eulerAngles;

        public Vector3 FinalVelocity
        {
            get => _finalVelocity;
            set => _finalVelocity = value;
        }

        public bool Moving => _finalVelocity != Vector3.zero;
        public Vector2 MouseDelta => _mouseDelta;
        public CharacterController CC => _cc;
        void Awake()
        {
             _cc = GetComponent<CharacterController>();
            _input = new PlayerInputActions();
            _brain = GetComponent<PlayerBrain>();
            
            _movement = _input.Player.Move;
            _movement.Enable();
            
            _mouse = _input.Player.Look;
            _mouse.Enable();
           
        }


        private void Start()
        {
            if (isLocalPlayer)
                return;

          
            _movement.Disable();
          
            _mouse.Disable();

        }
        private void OnDisable()
        {
           
            _movement.Disable();
           
            _mouse.Disable();
        }

        public void Handle()
        {
            //debug
            if (Input.GetKeyDown(KeyCode.L) && Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else if (Input.GetKeyDown(KeyCode.L))
                Cursor.lockState = CursorLockMode.None;
        }
        

        public CspState ProcessInput(InputPayLoad input)
        {
           
            
            
            
            transform.eulerAngles = input.Rotation;
            

            if (_brain.Status.Stunned || _brain.Status.Rooted)
                _finalVelocity = Vector3.zero;
            
            //^^^^ for this above here dog
            //ability needs to send a data of being able to be stopped during the movement or not
            //will fix later

            switch (_movementState)
            {
                case MovementState.Input:
                    _cc.enabled = true;
                    _finalVelocity = input.Movement;
                    _cc.Move(_finalVelocity * (NetworkServer.sendInterval * _brain.CharacterData.MaxMoveSpeed * _brain.Status.SlowFactor()));
                    _cc.enabled = false;
                    break;
                case MovementState.InMovementAbility:
                    _cc.enabled = true;
                    _cc.Move(_finalVelocity);
                    _cc.enabled = false;
                    break;
            }
          
            var state = RecordState(input.Tick);
            return state;
        }

       
        public void SetRotationState(RotationState newState, PlayerBrain target = null)
        {
            _target = target;
            _rotationState = newState;
        }
        public void SetMovementState(MovementState newState)
        {
           _movementState = newState;
        }

        #region Tele

        
        [Server]
        public void Teleport(Transform point)
        {
            _cc.enabled = false;
            transform.position = point.position;
            _cc.enabled = true;
        }
        #endregion
      

        #region Input

        public void TakeInput()
        {
            MovementInput();
        }
        
        private void MovementInput()
        {
            Vector2 input = Vector2.zero;

            switch (_movementState)
            {
                case MovementState.Input:
                    input = _movement.ReadValue<Vector2>();
                    break;
                case MovementState.NoInput:
                    break;
                case MovementState.InMovementAbility:
                    break;
            }
           
            
            // Map WASD / joystick directly to world-space X/Z
            _moveDirection = new Vector3(input.x, 0f, input.y);
        }
        
       
        public void RotationInput()
        {
            Vector2 mousePos = _mouse.ReadValue<Vector2>();
            _mouseDelta = mousePos;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            // Raycast down from the camera
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                Vector3 targetPoint = hitInfo.point;
                Vector3 lookDirection = targetPoint - transform.position;
                lookDirection.y = 0f; // Only rotate on Y axis

                if (lookDirection.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    float rotationSpeed = 10f; // Adjust speed to taste
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }

        public void RotationHandle()
        {
            
            if(_brain.Status.Stunned)
                return;
            switch (_rotationState)
            {
                case RotationState.LockRotation:
                    break;
                case RotationState.RegularInput:
                    RotationInput();
                    break;
                case RotationState.LookAtTarget:
                    LookAtTarget();
                    break;
            }
        }
        
        #endregion
       
           
        #region PayLoads

      

        private CspState RecordState(uint tick)
        {
            return new CspState()
            {
                Position = transform.position,
                Tick = tick
            };
            
        }
        
        [Client]
        public void SetState(CspState cspState)
        {
            _cc.enabled = false;
            transform.position = cspState.Position;
            _cc.enabled = true;
        }

        #endregion


        #region Utility

        private bool IsGrounded()  => _cc.isGrounded;
        
        

        private void LookAtTarget()
        {
            Vector3 lookDir = _target.transform.position - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 20f * Time.deltaTime);
            }
        }
        
        #endregion


        
     
    }
}