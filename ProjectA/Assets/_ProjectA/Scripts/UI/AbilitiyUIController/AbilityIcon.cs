using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Data.StatusEffectData;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI.AbilitiyUIController
{
    public class AbilityIcon : MonoBehaviour
    {
        [SerializeField] private int _abilityNumber;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _fillMask;
        [SerializeField] private TMP_Text _cd;
        [SerializeField] private TMP_Text _keyBind;
        private BaseAbility _ability;
        private PlayerBrain _brain;
        private PlayerInputActions _input;

        public void SetUI(BaseAbility ability, PlayerBrain brain)
        {
            _input = new PlayerInputActions();
            _icon.sprite = ability.BaseData.Sprite;
            _fillMask.fillAmount = 0;
            _cd.enabled = false;
            _keyBind.text = GetKeybindForAbility(_abilityNumber);
            _brain = brain;
            _ability = ability;   
        }


        public void UpdateCd()
        {
            if (_ability.OnCooldown)
            {
                _fillMask.fillAmount = _ability.Cooldown / _ability.BaseData.Cooldown;
                _cd.text = Mathf.Round(_ability.Cooldown).ToString();
            }
            else if (_ability.BaseData.IsGlobal && _brain.Ability.OnGlobalCd)
            {
                _fillMask.fillAmount = _brain.Ability.CurrentGlobalCd / _brain.Ability.GlobalCd;
            }
            else
            {
                _fillMask.fillAmount = 0;
            }
        }
        private string GetKeybindForAbility(int index)
        {
            InputAction action = index switch
            {
                1 => _input.Player.Ability1,
                2 => _input.Player.Ability2,
                3 => _input.Player.Ability3,
                4 => _input.Player.Ability4,
                5 => _input.Player.Ability5,
                6 => _input.Player.Ability6,
                _ => null
            };

            if (action == null) return string.Empty;

            // Pretty display string, e.g. "Q" instead of "<Keyboard>/q"
            return action.GetBindingDisplayString();
        }
        
    }
    
    
 
}
