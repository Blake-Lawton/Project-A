using System;
using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.UI;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ProjectA.Scripts.Controllers
{
    public class AbilityController : NetworkBehaviour
    {
       
        private PlayerInputActions _input;
        private InputAction _mouse;
        private Camera _camera;
        [SerializeField, ReadOnly]private PlayerBrain _target;
        
        [SerializeField]private BaseAbility[] _abilities;
        
        private float _globalCd;
        
        [Header("Casting Bar")]
        private NamePlate _namePlate;


        private void Awake()
        {
            _camera = Camera.main;
            _input = new PlayerInputActions();
            _input.Enable();
            _mouse = _input.Player.Look;
            
            
            _input.Player.Target.performed += ctx => TargetPlayer();
            _input.Player.Ability1.performed += ctx => TryCast(0);
        }


        private void Start()
        {
            if(isLocalPlayer)
                return;
            
            _input.Player.Disable();
        }

        private void TargetPlayer()
        {
            Vector2 mousePos = _mouse.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePos);

            Debug.Log(" TRYING TO TARGET PLAYER");
            // Raycast down from the camera
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out PlayerBrain playerBrain))
                    _target = playerBrain;
                else
                    _target = null;
                
            }
            Debug.Log(" TARGET IS   = " + _target);
        }

        private void TryCast(int i)
        {
            _abilities[i].UseAbility();
            Debug.Log(_abilities[i].Data.Name);
        }

        public void Handle()
        {
            
        }

        public void SetUp(NamePlate namePlate)
        {
            _namePlate = namePlate;
        }
    }
}
