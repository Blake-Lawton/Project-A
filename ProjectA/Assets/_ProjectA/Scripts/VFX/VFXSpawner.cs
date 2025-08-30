using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _ProjectA.Scripts.Util
{
    [Serializable]
    public class VFXSpawner 
    {
        [SerializeField] private VFXHelper _vfxPrefab;
        [SerializeField] private float _destroyDelay;
        [SerializeField] private float _spawnTime;
        
        private Transform _spawnPoint;
        public float SpawnTime => _spawnTime;
        public VFXHelper VFXPrefab => _vfxPrefab;
        public float DestroyDelay => _destroyDelay;

        public VFXHelper SpawnVFX(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            var vfx = Object.Instantiate(_vfxPrefab, _spawnPoint.position, _spawnPoint.rotation);
            vfx.SetUp(this);
            return vfx;
        }

    }
}
