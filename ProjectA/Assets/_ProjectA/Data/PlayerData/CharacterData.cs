using Sirenix.OdinInspector;
using UnityEngine;

namespace _BazookaBrawl.Data.ChracterData
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Title("Name")] 
        [SerializeField] private string _name;
        [Header("Health")] 
        [SerializeField] private int _health = 150;
        
        
        [Header("Movement")] 
        [SerializeField] private float _maxMoveSpeed = 10;
        [SerializeField] private float _groundAcceleration = 5;
        [SerializeField] private float _groundDeceleration = 5;
        
        [Header("Jump")] 
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _airAcceleration = 3;
        [SerializeField] private float _airDeceleration = 3;

        [Header("Gravity")] 
        [SerializeField] private float _gravityMultiplier = .2f;

        public string Name => _name;
        public int Health => _health;
        public float MaxMoveSpeed => _maxMoveSpeed;
        public float GroundAcceleration => _groundAcceleration;
        public float GroundDeceleration => _groundDeceleration;
        
        public float JumpForce => _jumpForce;
        public float AirAcceleration => _airAcceleration;
        public float AirDeceleration => _airDeceleration;
        
        public float GravityMultiplier => _gravityMultiplier;
    }
}
