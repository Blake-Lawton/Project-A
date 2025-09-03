using System;
using _ProjectA.Status.Active;
using Data.StatusEffectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI
{
    public class StatusIcon : MonoBehaviour
    {
        
        [SerializeField] private Image _icon;
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _stacks;

        private BaseStatus _status;

        public void SetUp(BaseStatus status)
        {
            gameObject.SetActive(false);
            if (status.Data.ShowAllPlayers)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if(status.Perp.isLocalPlayer)
                    gameObject.gameObject.SetActive(true);
            }

            _icon.sprite = status.Data.Icon;
            _stacks.gameObject.SetActive(status.Data.HasStacks);
            gameObject.name = status.Data.Name;
            status.UpdateStatus += Handle;
            status.RemoveStatus += Remove;
            _status = status;
        }

        private void Handle(float currentDuration, float totalDuration)
        {
            _fill.fillAmount = 1 - (currentDuration / totalDuration);
        }


        private void OnDestroy()
        {
            _status.UpdateStatus -= Handle;
            _status.RemoveStatus -= Remove;
        }

        private void Remove()
        {
            Destroy(gameObject);
        }
    }
}
