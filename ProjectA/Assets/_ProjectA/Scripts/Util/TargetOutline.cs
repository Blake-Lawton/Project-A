using System;
using System.Collections.Generic;
using UnityEngine;

namespace _ProjectA.Scripts.Util
{
    public class TargetOutline : MonoBehaviour
    {
        [SerializeField] private List<Outline> _outlines;


        private void Awake()
        {
            var outlines = GetComponentsInChildren<Outline>();

            foreach (var outline in outlines)
            {
                _outlines.Add(outline);
            }
        }

        public void TargetEnemy()
        {
            foreach (var outline in _outlines)
            {
                outline.enabled = true;
                outline.OutlineColor = Color.red;
            }
        }

        public void TargetTeammate()
        {
            foreach (var outline in _outlines)
            {
                outline.enabled = true;
                outline.OutlineColor = Color.green;
            }
        }

        public void UnTarget()
        {
            foreach (var outline in _outlines)
            {
                outline.enabled = false;
            }
        }
    }
}
