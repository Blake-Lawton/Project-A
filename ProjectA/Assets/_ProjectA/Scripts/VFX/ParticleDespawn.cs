using System.Collections.Generic;
using UnityEngine;

namespace _ProjectA.Scripts.Util
{
    public class ParticleDespawn : MonoBehaviour
    {
        [SerializeField] private List<ParticleHelper> _detachParticles;
        [SerializeField] private List<ParticleHelper> _destroyOnHit;


        public void Despawn()
        {
            foreach (var p in _detachParticles)
            {
                p.Detach();
            }

            foreach (var p in _destroyOnHit)
            {
                if(p != null)
                    Destroy(p.gameObject);
            }
        }
    }
}
