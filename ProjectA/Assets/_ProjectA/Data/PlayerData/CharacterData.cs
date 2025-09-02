using Sirenix.OdinInspector;
using UnityEngine;

namespace _BazookaBrawl.Data.ChracterData
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Title("Name")] 
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        
        [Header("Health")] 
        [SerializeField] private int _health = 150;
        
        
        [Header("Movement")] 
        [SerializeField] private float _maxMoveSpeed = 10;
        [SerializeField] private float _groundAcceleration = 5;
        [SerializeField] private float _groundDeceleration = 5;


        [Header("Stats")] 
        [SerializeField] private float _globalCD = .75f;
        public float GlobalCD => _globalCD;

        public Sprite Icon => _icon;
        public string Name => _name;
        public int Health => _health;
        public float MaxMoveSpeed => _maxMoveSpeed;
        public float GroundAcceleration => _groundAcceleration;
        public float GroundDeceleration => _groundDeceleration;
        
     
    }
}
