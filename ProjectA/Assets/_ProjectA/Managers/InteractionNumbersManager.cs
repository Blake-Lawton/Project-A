using System;
using _ProjectA.Scripts.Controllers;
using _ProjectA.Scripts.UI;
using DamageNumbersPro;
using Data.Interaction;
using Mirror;
using UnityEngine;

namespace _ProjectA.Managers
{
    public class InteractionNumbersManager : NetworkBehaviour
    {
        public static InteractionNumbersManager Instance;
        [SerializeField]private DamageNumber _standardDamageNumber;

   
        private Camera _camera;
        
        private void Awake()
        {
            Instance = this;
            _camera = Camera.main;
        }


        public void DisplayDamage(int damage, InteractionData data)
        {
            SpawnForVictim(data.Victim.NetworkIdentity.connectionToClient, data.Victim.NetworkIdentity, damage);
            SpawnForPerp(data.Perp.NetworkIdentity.connectionToClient, data.Victim.NetworkIdentity, damage);
        }


        
        [TargetRpc]
        private void SpawnForVictim(NetworkConnectionToClient conn, NetworkIdentity victim, int damage)
        {
            DamageNumber dn = _standardDamageNumber.Spawn(victim.transform.position, damage);
            dn.SetAnchoredPosition(victim.GetComponent<PlayerBrain>().NamePlate.GetComponent<RectTransform>(), Vector2.zero);
        }

        [TargetRpc]
        private void SpawnForPerp(NetworkConnectionToClient conn, NetworkIdentity victim, int damage)
        {
            DamageNumber dn = _standardDamageNumber.Spawn(victim.transform.position, damage);
            dn.SetAnchoredPosition(victim.GetComponent<PlayerBrain>().NamePlate.GetComponent<RectTransform>(), Vector2.zero);
        }
    }
}
