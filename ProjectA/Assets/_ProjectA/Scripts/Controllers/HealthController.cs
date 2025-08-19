using System;
using _ProjectA.Scripts.Interface;
using _ProjectA.Scripts.UI;
using Data.Interaction;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
    public class HealthController : NetworkBehaviour , IDamageable
    {

        [Header("UI")] 
        [SerializeField] private HealthBarUI _healthBarPrefab;
        private HealthBarUI _healthBar;
        public PlayerBrain Brain { get; set; }
       [Header("Health Data")]
       [Mirror.ReadOnly, SyncVar, SerializeField]private int _currentHealth;
       [Mirror.ReadOnly, SyncVar, SerializeField]private int _maxHealth;

       [SyncVar]private bool _isDead;
       
       public bool IsDead => _isDead;
       
       public Action<PlayerBrain> OnDeathServer;
       public Action OnDeath;
       public Action<Vector3, float, float> OnRagdoll;
      

       private void Awake()
        {
            Brain = GetComponent<PlayerBrain>();
            _maxHealth = Brain.CharacterData.Health;
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _healthBar = Instantiate(_healthBarPrefab, ScreenSpaceCanvas.Instance.transform);
            _healthBar.SetUp(transform);
        }

        public void Handle()
        {
            _healthBar.UpdateHealthBar((float)_currentHealth / _maxHealth);
        }
        

        public void TakeDamage(InteractionData data)
        {
            
            if(_currentHealth <= 0)
                Death(data);
        }

        public void Heal(int heal)
        {
            _currentHealth += heal;
            Debug.Log("Healed " + heal + " On Player " + name);
        }
        
        [Server]
        [Button]
        [GUIColor(1f, 0.3f, 0.3f)]
        public void Death(InteractionData data)
        {
            _isDead = true;
            OnDeathServer?.Invoke(Brain);
            DeathRPC();
          
        }
        
        [ClientRpc]
        private void DeathRPC()
        {
            _healthBar.Show(false);
            OnDeath?.Invoke();
        }
        
      

        public void Reset()
        {
            _isDead = false;
            _currentHealth = _maxHealth;
        
            _healthBar.Show(true);
        }
    }
}
