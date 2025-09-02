using System;
using _ProjectA.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI
{
    public class TargetUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _targetUIHolder;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Image _playerIcon;
        [SerializeField] private Image _enemyHealthFill;
        [SerializeField] private Image _allyHealthFill;

        private PlayerBrain _localPlayer;
        private PlayerBrain _target;


        public void SetUp(PlayerBrain localPlayer)
        {
            _localPlayer = localPlayer;
            localPlayer.Ability.TargetAcquired += SetTarget;
        }

        private void SetTarget(PlayerBrain playerBrain)
        {
            if (playerBrain == null)
            {
                _targetUIHolder.SetActive(false);
                return;
            }
            _targetUIHolder.SetActive(true);
            _target = playerBrain;
            _playerName.text = _target.CharacterData.name;
            _playerIcon.sprite = _target.CharacterData.Icon;
            if (_localPlayer.OnSameTeam(_target))
            {
                _enemyHealthFill.gameObject.SetActive(false);
                _allyHealthFill.gameObject.SetActive(true);
            }
            else
            {
                _allyHealthFill.gameObject.SetActive(false);
                _enemyHealthFill.gameObject.SetActive(true);
            }
        }


        private void Update()
        {
            if(!_target)
                return;
            
            if (_localPlayer.OnSameTeam(_target))
            {
                _allyHealthFill.fillAmount = _target.Health.GetCurrentHealthPrc();
            }
            else
            {
                _enemyHealthFill.fillAmount = _target.Health.GetCurrentHealthPrc();
            }
        }
    }
}
