using System.Collections;
using Data.Types;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private InputAction _jump;
        private InputAction _mouse;
        
     
        [Header("Movement")]
        [SerializeField, Sirenix.OdinInspector.ReadOnly] private Vector3 _finalVelocity;
        private Vector3 _currentVelocity;
        private Vector3 _moveDirection;
        private bool _pressedJump;
        
        public Vector3 MoveDirection => _moveDirection;
        public Vector3 Rotation => transform.eulerAngles;
        
        public bool InputJump => _pressedJump;
        
        public bool Moving => _finalVelocity != Vector3.zero;
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
            
            _jump = _input.Player.Jump;     
            _jump.Enable();
            _jump.performed += Jump;
        }


        private void Start()
        {
            if (isLocalPlayer)
                return;

            _jump.performed -= Jump;
            _movement.Disable();
            _jump.Disable();
            _mouse.Disable();

        }
        private void OnDisable()
        {
            _jump.performed -= Jump;
            _movement.Disable();
            _jump.Disable();
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
           
        
            if (input.Jump && IsGrounded())
            {
            }
            _pressedJump = false;
            
            
            transform.eulerAngles = input.Rotation;
            _cc.enabled = true;
            _finalVelocity = input.Movement;
          
            _cc.Move(_finalVelocity * (NetworkServer.sendInterval * _brain.CharacterData.MaxMoveSpeed * _brain.Status.SlowFactor()));
            _cc.enabled = false;
            var state = RecordState(input.Tick);
            return state;
        }

       

        #region Apply

        
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
            Vector2 input = _movement.ReadValue<Vector2>();
            // Map WASD / joystick directly to world-space X/Z
            _moveDirection = new Vector3(input.x, 0f, input.y);
        }
        
       
        public void RotationInput()
        { 
            // Get mouse position
            Vector2 mousePos = _mouse.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            // Raycast down from the camera
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                Vector3 targetPoint = hitInfo.point;
                // Only rotate on Y axis
                Vector3 lookDirection = targetPoint - transform.position;
                lookDirection.y = 0f;

                if (lookDirection.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
        }
        private void Jump(InputAction.CallbackContext callback)
        {
            if (isLocalPlayer)
            {
                _pressedJump = true;
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
        
       
        
        #endregion


      
    }
}