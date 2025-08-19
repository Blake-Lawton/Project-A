using System;
using _ProjectA.Scripts.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        
        [SerializeField] private GameObject _healthBarContainer;
        
        [SerializeField] private Image _healthBar;
        [SerializeField] private Vector3 _healthBarOffset;
        private Transform _target;
        private Camera _camera;
        public void Show(bool show)
        {
            _healthBarContainer.SetActive(show);
        }
        
        public void UpdateHealthBar(float health)
        {
            _healthBar.fillAmount = health;
        }


        public void SetUp(Transform target)
        {
            _target = target;
            _camera = Camera.main;
        }


        private void LateUpdate()
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(_target.position + _healthBarOffset);
            _healthBarContainer.transform.position = screenPosition;
        }
    }
}
