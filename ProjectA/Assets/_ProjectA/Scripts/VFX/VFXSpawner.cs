using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _ProjectA.Scripts.Util
{
    [Serializable]
    public class VFXSpawner 
    {
        [SerializeField] private VFXHelper _vfxPrefab;
        [SerializeField] private float _destroyDelay;
        
        private Transform _spawnPoint;
      
        public VFXHelper VFXPrefab => _vfxPrefab;
        public float DestroyDelay => _destroyDelay;

        public VFXHelper SpawnVFX(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            var vfx = Object.Instantiate(_vfxPrefab, _spawnPoint.position, _spawnPoint.rotation);
            vfx.SetUp(this);
            return vfx;
        }
        
        public VFXHelper SpawnParentedVFX(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            var vfx = Object.Instantiate(_vfxPrefab, spawnPoint);
            vfx.SetUp(this);
            return vfx;
        }

    }
}
