using System;
using UnityEngine;

namespace _ProjectA.Scripts.UI
{
    public class ScreenSpaceCanvas : MonoBehaviour
    {
        public static ScreenSpaceCanvas Instance;

        private Canvas _canvas;
        public Canvas Canvas => _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            Instance = this;
        }
    }
}
