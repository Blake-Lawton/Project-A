using UnityEngine;
using Joint = Data.Types.Joint;

namespace _ProjectA.Scripts.Helpers
{
    public class JointHelper : MonoBehaviour
    {
        [SerializeField] private Joint _joint;

        public Joint Joint => _joint;
    }
}
