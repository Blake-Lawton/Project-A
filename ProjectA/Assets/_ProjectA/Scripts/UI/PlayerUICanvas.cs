using _ProjectA.Scripts.UI.AbilitiyUIController;
using UnityEngine;

namespace _ProjectA.Scripts.UI
{
   public class PlayerUICanvas : MonoBehaviour
   {
   
      [SerializeField] private AbilityUIController _abilityUIController;
      public static PlayerUICanvas Instance;

      public AbilityUIController AbilityUIController => _abilityUIController;
      private void Awake()
      {
         Instance = this;
         _abilityUIController = GetComponentInChildren<AbilityUIController>();
      }
   }
}
