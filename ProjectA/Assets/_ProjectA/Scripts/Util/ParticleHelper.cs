using System;
using UnityEngine;

namespace _ProjectA.Scripts.Util
{
    public class ParticleHelper : MonoBehaviour
    {
        [SerializeField] private bool _destroyOnAwake = false;
        [SerializeField] private float _destroyTime;

        private void Awake()
        {
            if(_destroyOnAwake)
                SetDestroyTime();
        }

        private void SetDestroyTime()
        {
            Destroy(gameObject, _destroyTime);
        }


        public void Detach()
        {
            SetDestroyTime();
            transform.parent = null;
        }
    }
}
