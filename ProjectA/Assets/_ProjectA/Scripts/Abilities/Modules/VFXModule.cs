using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.Interface;
using UnityEngine;

namespace _ProjectA.Scripts.Abilities.Modules
{
    [Serializable]
    public class VFXModule : AbilityModuleBase
    {
        public Transform Root;
        public GameObject VFXPrefab;
        
        public override void OnStartCast(PlayerBrain player, PlayerBrain target)
        {
            
        }
        

        public override void OnEndCast(PlayerBrain player, PlayerBrain target)
        {
        
        }
    }
}
