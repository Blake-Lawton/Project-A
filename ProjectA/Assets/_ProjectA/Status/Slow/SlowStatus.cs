using _ProjectA.Status.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.Active
{
    public class SlowStatus : BaseStatus
    {
        public SlowData SlowData => (SlowData)_data;
        
    }
}
