using _ProjectA.Scripts.Controllers;
using DamageNumbersPro;
using Data.Interaction;
using Data.Types;
using Mirror;
using UnityEngine;

namespace _ProjectA.Scripts.Managers
{
    public class InteractionNumbersManager : NetworkBehaviour
    {
        public static InteractionNumbersManager Instance;
        [SerializeField]private DamageNumber _standardDamage;
        [SerializeField] private DamageNumber _heal;
        [SerializeField] private DamageNumber _shield;

   
        private Camera _camera;
        
        private void Awake()
        {
            Instance = this;
            _camera = Camera.main;
        }


        public void DisplayInteraction(int healthChange, InteractionData data)
        {
            SpawnInteractionNumbers(data.Victim.NetworkIdentity.connectionToClient, data.Victim.NetworkIdentity, healthChange, data.Ability.BaseData.Type);
            SpawnInteractionNumbers(data.Perp.NetworkIdentity.connectionToClient, data.Victim.NetworkIdentity, healthChange,data.Ability.BaseData.Type);
        }


        
        [TargetRpc]
        private void SpawnInteractionNumbers(NetworkConnectionToClient conn, NetworkIdentity victim, int damage, AbilityType type)
        {
            DamageNumber dn = _standardDamage;
            switch (type)
            {
                case AbilityType.Damage:
                    dn = _standardDamage.Spawn(victim.transform.position, damage);
                    break;
                case AbilityType.Shield:
                   dn = _shield.Spawn(victim.transform.position, damage);
                    break;
                case AbilityType.Heal:
                    dn = _heal.Spawn(victim.transform.position, damage);
                    break;
            }
         
            dn.SetAnchoredPosition(victim.GetComponent<PlayerBrain>().NamePlate.GetComponent<RectTransform>(), Vector2.zero);
        }

        
    }
}
