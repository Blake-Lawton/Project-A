using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] private bool _stunned;
        [SerializeField] private bool _rooted;
        //for now
        public int DamageAmp = 1;
    }
}
