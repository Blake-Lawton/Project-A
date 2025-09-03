using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
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


        [Header("Statuses")] 
        [SerializeField] private GameObject _grid;
        
        
        private PlayerBrain _localPlayer;
        private PlayerBrain _target;


        public void SetUp(PlayerBrain localPlayer)
        {
            _localPlayer = localPlayer;
            localPlayer.Ability.TargetAcquired += OnSetTarget;
        }

        private void OnSetTarget(PlayerBrain target)
        {
            if (target == null)
            {
                _targetUIHolder.SetActive(false);
                if(target != null)
                    _target.Status.AddStatus -= OnAddStatus;
                _target = null;
                return;
            }

            if (target == _target)
            {
                return;
            }
            
            DestroyAllChildren(_grid);
           
            _targetUIHolder.SetActive(true);
            
            target.Status.AddStatus -= OnAddStatus;
            _target = target;
            target.Status.AddStatus += OnAddStatus;
            
            SetData();

            foreach (var icon in target.Status.GetStatusIcons())
                icon.transform.SetParent(_grid.transform);
            
        }

        
        private void OnAddStatus(BaseStatus status)
        {
            var statusIcon = status.GenerateIcon();
            statusIcon.transform.SetParent(_grid.transform);
        }
        
        private void SetData()
        {
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
                _allyHealthFill.fillAmount = _target.Health.GetCurrentHealthPrc();
            else
                _enemyHealthFill.fillAmount = _target.Health.GetCurrentHealthPrc();
            
        }
        
        private void DestroyAllChildren(GameObject parent)
        {
            foreach (Transform child in parent.transform)
                Destroy(child.gameObject);
        }
    }
}
