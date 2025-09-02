using _ProjectA.Scripts.UI.AbilitiyUIController;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectA.Scripts.UI
{
   public class PlayerUICanvas : MonoBehaviour
   {
      
      public static PlayerUICanvas Instance;
   
      [SerializeField] private AbilityUIController _abilityUI;
      [SerializeField] private TargetUIController _targetUI;
      

      public AbilityUIController AbilityUI => _abilityUI;
      public TargetUIController TargetUI => _targetUI;
      private void Awake()
      {
         Instance = this;
         _abilityUI = GetComponentInChildren<AbilityUIController>();
         _targetUI = GetComponentInChildren<TargetUIController>();
      }
   }
}
