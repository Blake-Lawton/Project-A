using _ProjectA.Scripts.Abilities;
using _ProjectA.Scripts.Controllers;
using Data.AbilityData;
using Data.StatusEffectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI.AbilitiyUIController
{
    public class AbilityIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _fillMask;
        [SerializeField] private TMP_Text _cd;
        [SerializeField] private TMP_Text _keyBind;
        private BaseAbility _ability;
        private PlayerBrain _brain;

        public void SetUI(BaseAbility ability, PlayerBrain brain)
        {
            _icon.sprite = ability.BaseData.Sprite;
            _fillMask.fillAmount = 0;
            _cd.enabled = false;
            _keyBind.text = "KeyBind";
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
        
    }
}
