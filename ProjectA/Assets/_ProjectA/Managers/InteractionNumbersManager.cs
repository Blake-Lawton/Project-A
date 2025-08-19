using DamageNumbersPro;
using UnityEngine;

namespace _BazookaBrawl.Scripts.Manager
{
    public class InteractionNumbersManager : MonoBehaviour
    {
        [SerializeField]private DamageNumber _standardDamageNumber;

        public void SpawnStandardDamage(int damage, Vector3 position)
        {
            DamageNumber damageNumber = _standardDamageNumber.Spawn(position, damage);
        }
    }
}
