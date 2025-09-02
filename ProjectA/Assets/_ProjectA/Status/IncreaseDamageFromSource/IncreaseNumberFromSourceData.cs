using _ProjectA.Status.Active;
using _ProjectA.Status.IncreaseDamageFromSource;
using Data.Interaction;
using Data.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.StatusEffectData
{
    [CreateAssetMenu(fileName = "IncreaseDamageFromSourceData", menuName = "Scriptable Objects/Statuses/Increase Damage From Source")]
    public class IncreaseNumberFromSourceData : BaseStatusData
    {
        
        [SerializeField] private float _percentage;
        public float Percentage => _percentage;
        public override BaseStatus CreateStatus(InteractionData interaction)
        {
            var status = new IncreaseNumberFromSourceStatus();
            status.SetUp(interaction, this);
            return status;
        }
    }
}
