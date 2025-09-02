using System;
using AMPInternal;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectA.Scripts.Managers
{
    public class SFXManagerWrapper : MonoBehaviour
    {
        public static SFXManagerWrapper Instance;

        private SFXManager _sfxManager;

        [SerializeField] private float _localVolume = .8f;
        [SerializeField] private float _nonLocalVolume =.5f;
        
        [SerializeField] private int _localPriority = 50;
        [SerializeField] private int _nonLocalPriority = 100;
        
        private void Start()
        {
            Instance = this;
            _sfxManager = SFXManager.Main;
        }

        public void Play(SFXObject sfx, Vector3 position, bool isLocal, Transform parent = null)
        {
            if (sfx == null)
            {
                Debug.LogError(" NO SFX FOUND");
                return;
            }
           
            // sfx.SFXLayers[0].SpatialBlend
            if (isLocal)
            {
                sfx.SetPriorty(_localPriority);
                _sfxManager.Play(sfx, position,0, _localVolume, parent);
            }
            else
            {
                sfx.SetPriorty(_nonLocalPriority);
                _sfxManager.Play(sfx, position, _nonLocalVolume, _nonLocalVolume, parent);
            }
        }


        public void SetUpAudioSource(AudioSource source, bool isLocal)
        {
            if (isLocal)
            {
                source.priority = _localPriority;
                source.volume = _localVolume;
            }
            else
            {
                source.priority = _nonLocalPriority;
                source.volume = _nonLocalVolume;
            }
        }
    }
}
