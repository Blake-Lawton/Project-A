using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI;
using Data.Interaction;
using Data.StatusEffectData;
using Sirenix.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;


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
        public InteractionData Interaction => _interaction;
        public BaseStatusData Data => _data;
        public PlayerBrain Target => _target;
        public PlayerBrain Perp => _perp;

        public event Action<float, float> UpdateStatus;
        public event Action RemoveStatus;
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
            RemoveStatus?.Invoke();
            _target.Status.RemoveStatus(this);
        }

        public virtual void Handle()
        {
            UpdateStatus?.Invoke(_currentDuration, _data.Duration);
            _currentDuration -= Time.deltaTime;
            if(_currentDuration <= 0)
                End();
        }


        public StatusIcon GenerateIcon()
        {
            var icon = Object.Instantiate(_data.StatusIconPrefab);
            icon.SetUp(this);
            return icon;
        }
    }
}