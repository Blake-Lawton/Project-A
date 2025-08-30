using System.Collections.Generic;
using UnityEngine;

namespace _ProjectA.Scripts.UI.AbilitiyUIController
{
    public class AbilityUIController : MonoBehaviour
    {
   
        [SerializeField] private List<AbilityIcon> _abilityIcons;
    
        public List<AbilityIcon> AbilityIcons => _abilityIcons;
    }
}
