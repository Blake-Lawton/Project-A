using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI;
using Data.Interaction;
using Data.StatusEffectData;
using Sirenix.Serialization;
using UnityEngine;

namespace _ProjectA.Status.Active
{
    [Serializable]
    public abstract class BaseStatus
    {
        [OdinSerialize] protected BaseStatusData _data;
        [OdinSerialize] protected PlayerBrain _target;
        [OdinSerialize] protected PlayerBrain _perp;
        [SerializeField] protected float _duration;
        [SerializeField] protected float _currentDuration;
        private InteractionData _interaction;
        protected StatusNameplateIcon _nameplateIcon;
        public InteractionData Interaction => _interaction;
        public BaseStatusData Data => _data;
        public virtual void SetUp(InteractionData interaction, BaseStatusData data)
        {
            _target = interaction.Victim;
            _perp = interaction.Perp;
            _data = data;
            _duration = data.Duration;
            _currentDuration = data.Duration;
            _interaction = interaction;
        }

        public virtual void Apply()
        {
            _currentDuration = _data.Duration;
        }

        public virtual void Refresh()
        {
            _currentDuration = _data.Duration;
        }

        public virtual void End()
        {
            _nameplateIcon.Remove();
            _target.Status.RemoveStatus(this);
        }

        public virtual void Handle()
        {
           
            _nameplateIcon.Handle(_currentDuration, _data.Duration);
            _currentDuration -= Time.deltaTime;
            if(_currentDuration <= 0)
                End();
        }

        public void SetUpUI(StatusNameplateIcon icon)
        {
            _nameplateIcon = icon;
            icon.gameObject.SetActive(false);
            if (_data.ShowAllPlayers)
            {
                icon.gameObject.SetActive(true);
            }
            else
            {
                if(_perp.isLocalPlayer)
                    icon.gameObject.SetActive(true);
            }
                
                
        }
    }
}