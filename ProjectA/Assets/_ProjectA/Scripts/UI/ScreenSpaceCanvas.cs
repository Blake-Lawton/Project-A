using System;
using UnityEngine;

namespace _ProjectA.Scripts.UI
{
    public class ScreenSpaceCanvas : MonoBehaviour
    {
        public static ScreenSpaceCanvas Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
