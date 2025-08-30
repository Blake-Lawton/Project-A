using _ProjectA.Status.Active;
using Data.StatusEffectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectA.Scripts.UI
{
    public class StatusNameplateIcon : MonoBehaviour
    {
        
        [SerializeField] private Image _icon;
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _stacks;


        public void SetUp(BaseStatus statusData)
        {
            _icon.sprite = statusData.Data.Icon;
            _stacks.gameObject.SetActive(statusData.Data.HasStacks);
            gameObject.name = statusData.Data.Name;
        }

        public void Handle(float currentDuration, float totalDuration)
        {
            _fill.fillAmount = 1 - (currentDuration / totalDuration);
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}
