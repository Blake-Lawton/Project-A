using _ProjectA.Status.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.Active
{
    public class SlowStatus : BaseStatus
    {
       
        public SlowData SlowData => (SlowData)_data;
        
        public override void Apply()
        {
           _currentDuration = SlowData.Duration;
        }

        public override void Refresh()
        {
            _currentDuration = SlowData.Duration;
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
