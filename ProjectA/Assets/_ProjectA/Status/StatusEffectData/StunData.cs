using _ProjectA.Status.Active;
using Data.Interaction;
using Data.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.StatusEffectData
{
    [CreateAssetMenu(fileName = "StunData", menuName = "Scriptable Objects/Statuses/Stun Data")]
    public class StunData : BaseStatusData
    {
        public override BaseStatus CreateStatus(InteractionData interaction)
        {
            var status = new StunStatus();
            status.SetUp(interaction, this);
            return status;
        }
    }
}
