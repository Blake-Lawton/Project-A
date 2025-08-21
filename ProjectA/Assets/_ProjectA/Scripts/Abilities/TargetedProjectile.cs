using System;
using _ProjectA.Managers;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Data.Interaction;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities
{
    public class TargetedProjectile : MonoBehaviour
    {
        private PlayerBrain _owner;
        private PlayerBrain _target;
        private TargetedProjectileData _abilityData;
        public void SetUp(PlayerBrain owner, PlayerBrain target, TargetedProjectileData abilityData)
        {
            _target = target;
            _owner = owner;
            _abilityData = abilityData;
        }
        
        private void Update()
        {
            transform.LookAt(_target.transform.position);
            transform.position += transform.forward * Time.deltaTime * _abilityData.Speed;

            if (Vector3.Distance(_target.transform.position, transform.position) < 0.1f)
            {
                Hit();
            }
        }


        private void Hit()
        {

            if (_owner.isServer)
            {
                var interactionData =  new InteractionData(_owner, _target, _abilityData);
                InteractionManager.Instance.ProcessInteraction(interactionData);
            }
          
            Destroy(gameObject);
        }
    }
}
