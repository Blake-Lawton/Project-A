using System;
using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
       
        private Camera _camera;        // Camera reference
        private Transform _target;     // Target to follow
        [SerializeField] private Vector3 _offset = new Vector3(0, 10, -10); // Offset from the target
        [SerializeField] private float _positionLerpSpeed = 5f;
        [SerializeField] private Vector3 _fixedRotationEuler = new Vector3(45f, 0f, 0f); // Top-down angle


        private void Awake()
        {
            _camera = Camera.main;
            _target = transform;
        }


        private void LateUpdate()
        {
            if (_camera == null || _target == null) return;

            // Smoothly move camera to follow the target with offset
            Vector3 desiredPosition = _target.position + _offset;
            _camera.transform.position = desiredPosition;

            // Keep a fixed rotation (top-down view)
            _camera.transform.rotation = Quaternion.Euler(_fixedRotationEuler);
        }
    }
}
