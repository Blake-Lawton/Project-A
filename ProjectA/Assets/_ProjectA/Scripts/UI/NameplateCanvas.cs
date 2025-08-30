using System;
using UnityEngine;

namespace _ProjectA.Scripts.UI
{
    public class NameplateCanvas : MonoBehaviour
    {
        public static NameplateCanvas Instance;

        private Canvas _canvas;
        public Canvas Canvas => _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            Instance = this;
        }
    }
}
