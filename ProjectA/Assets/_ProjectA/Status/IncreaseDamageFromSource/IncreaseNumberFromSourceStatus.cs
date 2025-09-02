using _ProjectA.Scripts.Controllers;
using _ProjectA.Status.Active;
using _ProjectA.Status.StatusEffectData;
using UnityEngine;

namespace _ProjectA.Status.IncreaseDamageFromSource
{
    public class IncreaseNumberFromSourceStatus : BaseStatus
    {
        public IncreaseNumberFromSourceData IncreaseNumberFromSourceData => (IncreaseNumberFromSourceData)_data;
        
    }
}
