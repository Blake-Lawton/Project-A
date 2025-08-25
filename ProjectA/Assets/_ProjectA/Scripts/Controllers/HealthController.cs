using System;
using _BazookaBrawl.Data.ChracterData;
using _ProjectA.Managers;
using _ProjectA.Scripts.Interface;
using _ProjectA.Scripts.UI;
using Data.Interaction;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
    public class HealthController : NetworkBehaviour
    {

        [Header("UI")] 
        
        private NamePlate _namePlate;
        private PlayerBrain _brain;
       [Header("Health Data")]
       [Mirror.ReadOnly, SyncVar, SerializeField]private int _currentHealth;
       [Mirror.ReadOnly, SyncVar, SerializeField]private int _maxHealth;

       [SyncVar]private bool _isDead;
       
       public bool IsDead => _isDead;
       
       public Action<PlayerBrain> OnDeathServer;
       public Action OnDeath;
       
       

       private void Awake()
        {
            _brain = GetComponent<PlayerBrain>();
            _maxHealth = _brain.CharacterData.Health;
            _currentHealth = _maxHealth;
        }

        public void SetUp(NamePlate namePlate, CharacterData data)
        {
            _namePlate = namePlate;
            _maxHealth = data.Health;
            _currentHealth = data.Health;
        }

        public void Handle()
        {
            _namePlate.UpdateHealthBar((float)_currentHealth / _maxHealth);
        }
        
        [Server]
        public void TakeDamage(int damage, InteractionData data)
        {
            InteractionNumbersManager.Instance.DisplayDamage(damage, data);
            _currentHealth -= damage;
            if(_currentHealth <= 0)
                Death();
        }

        [Server]
        public void Heal(int heal)
        {
            _currentHealth += heal;
            Debug.Log("Healed " + heal + " On Player " + name);
        }
        
        [Server]
        private void Death()
        {
            _isDead = true;
            OnDeathServer?.Invoke(_brain);
            DeathRPC();
          
        }
        
        [ClientRpc]
        private void DeathRPC()
        {
            _namePlate.Show(false);
            OnDeath?.Invoke();
        }
        
      

        public void Reset()
        {
            _isDead = false;
            _currentHealth = _maxHealth;
        
            _namePlate.Show(true);
        }

       
    }
}
