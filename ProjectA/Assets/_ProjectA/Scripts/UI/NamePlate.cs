using System;
using _ProjectA.Scripts.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI
{
    public class NamePlate : MonoBehaviour
    {
        
        [SerializeField] private Vector3 _namePlateOffSet;
        
        [Header("HealthBar")]
        [SerializeField] private Image _healthBarFill;

        [Header("Cast Bar")] 
        [SerializeField] private GameObject _castBarContainer;
        [SerializeField] private Image _castBarFill;
        [SerializeField] private TMP_Text _abilityName;
        
        private Transform _target;
        private Camera _camera;
        public void Show(bool show)
        {
            gameObject.SetActive(show);
        }
        
        public void UpdateHealthBar(float health)
        {
            _healthBarFill.fillAmount = health;
        }

        public void UpdateCastBar(float currentCast)
        {
            _castBarFill.fillAmount = currentCast;
        }

        public void NameAbility(string abilityName)
        {
            _abilityName.text = abilityName;
        }

        public void ShowCastBar(bool show)
        {
            _castBarContainer.gameObject.SetActive(show);
        }
        
        public void SetUp(Transform target)
        {
            _target = target;
            _camera = Camera.main;
            ShowCastBar(false);
        }


        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 worldPos = _target.position + _namePlateOffSet;
            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
    }
}
