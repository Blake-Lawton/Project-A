using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using Data.Interaction;
using Data.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.StatusEffectData
{
    [CreateAssetMenu(fileName = "SlowData", menuName = "Scriptable Objects/Statuses/Slow Data")]
    public class SlowData : BaseStatusData
    {
        [Header("Slow Data")]
        [SerializeField] private float _slowAmount;

        public float SlowAmount =>  _slowAmount;
        
        public override BaseStatus CreateStatus(InteractionData interaction)
        {
            var status = new SlowStatus();
            status.SetUp(interaction, this);
            return status;
        }
    }
}
