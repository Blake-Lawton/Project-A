using _ProjectA.Scripts.Abilities.BaseClasses;
using Data.Types;
using Mirror;
using UnityEngine;

namespace _ProjectA.Classes.GreatSword.Abilties.RecklessLeap.Scripts
{
    public class GreatSwordRecklessLeap : MovementAbility
    {
        private GreatSwordRecklessLeapData Data => (GreatSwordRecklessLeapData)_baseData;
        [SerializeField,Sirenix.OdinInspector.ReadOnly]private bool _inMovement;
        [SerializeField,Sirenix.OdinInspector.ReadOnly]private float _time;
        [SerializeField,Sirenix.OdinInspector.ReadOnly]private Vector3 _targetLocation;
        [SerializeField,Sirenix.OdinInspector.ReadOnly]private Vector3 _startLocation;
        public override void StartCast()
        {
            _brain.Movement.SetMovementState(MovementState.NoInput);
            
            _targetLocation = FindTargetLocation();
            _startLocation = _brain.transform.position;
            if (_brain.isLocalPlayer)
                _brain.Ability.SendAbilityPosition(_targetLocation);
            
        }

        public override void Interrupt()
        {
            
        }

        public override void ConfirmHit(int id)
        {
            
        }

        public override void EndCast()
        {
            base.EndCast();
            
            
            
            _inMovement = true;
            marker = 0;
        }

        public override void ReceiveVector3(Vector3 targetLocation)
        {
            _targetLocation = targetLocation;
        }

        
        int marker = 0;
        protected override void OnTick(float deltaTime)
        {
            
            if (!_inMovement) return;

            _brain.Movement.SetMovementState(MovementState.InMovementAbility);
            marker++;
            _time += deltaTime;

            if (_time >= Data.LeapTime)
            {
                EndMovement();
                return;
            }

            // Regular lerp during movement
            float t = _time / Data.LeapTime;

            float yOffset = Data.LeapHeight.Evaluate(t);
            Vector3 newPos = Vector3.Lerp(_startLocation, _targetLocation, t);
            newPos.y = yOffset;

            Vector3 delta = newPos - transform.position;
            _brain.Movement.MoveDirection = delta;

            Debug.Log( marker + "Target location " +_targetLocation + " Start Location " + _startLocation+ " Current Position " + transform.position +" Velocity " + _brain.Movement.FinalVelocity + " Tick Time " + deltaTime + " timer " + _time );
        }

        protected override void EndMovement()
        {
            if(_brain.isServer)
                _brain.Movement.Teleport(_targetLocation);
            _inMovement = false;
            _brain.Movement.SetMovementState(MovementState.Input);
            _time = 0;
        }


        private Vector3 FindTargetLocation()
        {
            Ray ray = Camera.main.ScreenPointToRay(_brain.Movement.MouseDelta);

            // Raycast down from the camera
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                Vector3 targetPoint = hitInfo.point;
                targetPoint.y = _brain.transform.position.y;
                return targetPoint;  
            }
            
            return Vector3.zero;
        }
    }
}
