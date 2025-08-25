using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using _ProjectA.Status.StatusEffectData;
using Data.Interaction;
using Data.StatusEffectData;
using Sirenix.Serialization;
using UnityEngine;

namespace _ProjectA.Scripts.Status
{
    public class SlowStatus : BaseStatus
    {
        private SlowData _slowData;
        public SlowData SlowData => _slowData;
        public override void SetUp(InteractionData interaction, BaseStatusData data)
        {
            base.SetUp(interaction, data);
            _slowData = data as SlowData;
            if (_slowData == null)
            {
                Debug.LogError($"[SlowStatus] Failed to set up: Data is not a SlowData! Received {data?.GetType().Name ?? "null"}");
                return;
            }
        }

        public override void Apply()
        {
           
        }

        public override void Refresh()
        {
            _duration = _slowData.Duration;
        }

        public override void End()
        {
            _target.Status.RemoveStatus(this);
        }

        public override void Tick()
        {
            _duration -= Time.deltaTime;
            if(_duration <= 0)
                End();
        }
    }
}
