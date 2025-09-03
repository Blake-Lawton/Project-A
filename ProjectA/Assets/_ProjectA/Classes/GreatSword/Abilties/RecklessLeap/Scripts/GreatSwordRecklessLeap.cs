using _ProjectA.Scripts.Abilities.BaseClasses;
using Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Classes.GreatSword.Abilties.RecklessLeap.Scripts
{
    public class GreatSwordRecklessLeap : MovementAbility
    {
        private GreatSwordRecklessLeapData Data => (GreatSwordRecklessLeapData)_baseData;
        [SerializeField,ReadOnly]private bool _inMovement;
        [SerializeField,ReadOnly]private float _time;
        [SerializeField,ReadOnly]private Vector3 _targetLocation;
        [SerializeField,ReadOnly]private Vector3 _startLocation;
        public override void StartCast()
        {
            _brain.Movement.SetMovementState(MovementState.NoInput);
            _targetLocation = FindTargetLocation();
            _brain.Ability.TargetLocation = _targetLocation;
            _startLocation = transform.position;
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
            _brain.Movement.SetMovementState(MovementState.InMovementAbility);
            _inMovement = true;
        }


        protected override void OnTick(float deltaTime)
        {
            
            if (_inMovement)
            {
                _time += deltaTime;
                if (Data.LeapTime <= _time)
                    EndMovement();
                
                
                float yPos = Data.LeapHeight.Evaluate(_time / Data.LeapTime);
                Vector3 newPos = Vector3.Lerp(_startLocation, _targetLocation, _time / Data.LeapTime);
                
                newPos.y = yPos;
                
                Vector3 delta = newPos - transform.position;
                _brain.Movement.FinalVelocity = delta;
            }
        }

        protected override void EndMovement()
        {
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
                return targetPoint;  
            }
            
            return Vector3.zero;
        }
    }
}
