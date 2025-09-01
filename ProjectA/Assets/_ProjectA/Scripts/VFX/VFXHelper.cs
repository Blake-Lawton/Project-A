using System;
using UnityEngine;

namespace _ProjectA.Scripts.Util
{
    public class VFXHelper : MonoBehaviour
    {
        private ParticleSystem _particle;
        
        public ParticleSystem Particle => _particle;

        public void SetUp(VFXSpawner spawner)
        {
            _particle = GetComponent<ParticleSystem>();
            if (spawner.DestroyDelay != 0)
            {
                Destroy(gameObject, spawner.DestroyDelay); 
            }
            
           
        }
        
    }
}
