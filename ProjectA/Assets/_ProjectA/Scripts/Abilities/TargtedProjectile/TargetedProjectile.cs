using System;
using System.Collections.Generic;
using _ProjectA.Managers;
using _ProjectA.Scripts.Abilities.Mage;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Util;
using Data.AbilityData;
using Data.Interaction;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities
{
    public class TargetedProjectile : MonoBehaviour
    {
        private PlayerBrain _owner;
        private PlayerBrain _target;
        private float _speed;
        private TargetedAbility _ability;
        private Action<PlayerBrain> _onHit;
        private ParticleDespawn _particles;
        
        public void SetUp(float speed,PlayerBrain target, Action<PlayerBrain> onHit)
        {
            _target = target;
            _speed = speed;
            _onHit = onHit;
            _particles = GetComponent<ParticleDespawn>();
        }
        
        private void Update()
        {
            transform.LookAt(_target.transform.position);
            transform.position += transform.forward * Time.deltaTime * _speed;

            if (Vector3.Distance(_target.transform.position, transform.position) < 0.25f)
            {
                Hit();
            }
        }


        private void Hit()
        {
            _particles.Despawn();
            _onHit?.Invoke(_target);
            Destroy(gameObject);
        }
    }
}
