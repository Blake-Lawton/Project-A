using _ProjectA.Status.StatusEffectData;
using Data.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.Active
{
    public class StunStatus : BaseStatus
    {
        private StunData StunData => (StunData)_data;
        public override void Apply()
        {
            _currentDuration = StunData.Duration;
        }

        public override void Refresh()
        {
            _currentDuration = StunData.Duration;
        }

        public override void Handle()
        {
            _nameplateIcon.Handle(_currentDuration, _data.Duration);
            _currentDuration -= Time.deltaTime;
            if(_currentDuration <= 0)
                End();
        }
    }
}
