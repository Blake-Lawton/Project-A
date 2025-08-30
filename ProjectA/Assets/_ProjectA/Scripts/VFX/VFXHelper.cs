using UnityEngine;

namespace _ProjectA.Scripts.Util
{
    public class VFXHelper : MonoBehaviour
    {
        
        public void SetUp(VFXSpawner spawner)
        {
            Destroy(gameObject, spawner.DestroyDelay); 
        }
        
    }
}
