using _BazookaBrawl.Data;
using _BazookaBrawl.Data.ChracterData;
using _BazookaBrawl.Data.PlayerData;
using _ProjectA.Scripts.Networking;
using _ProjectA.Scripts.UI;
using _ProjectA.Scripts.Util;
using Data.Types;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _ProjectA.Scripts.Controllers
{
    [RequireComponent(typeof(MovementController), typeof(Client), typeof(NetworkIdentity))]
    public class PlayerBrain : NetworkBehaviour
    {
      

        [SerializeField] private CharacterData _characterData;
        [SerializeField] private NamePlate _namePlatePrefab;
        private NamePlate _nameplate;
        private PlayerData _playerData;
        private MovementController _movement;
        private AnimationController _animation;
        private Client _client;
        private HealthController _health;
        private AbilityController _ability;
        private StatusController _status;
        private JointFinder _jointFinder;
        public CharacterData CharacterData => _characterData;
        public PlayerData PlayerData => _playerData;
        public MovementController Movement => _movement;
        public Client client => _client;
        public NetworkIdentity NetworkIdentity => netIdentity;
        public HealthController Health => _health;
        public AbilityController Ability => _ability;
        public StatusController Status => _status;
        public NamePlate NamePlate => _nameplate;
        public JointFinder JointFinder => _jointFinder;
        public AnimationController Animation => _animation;
        [Header("Debug for now")] 
        [SerializeField, SyncVar(hook = nameof(OnTeamChange))] private Team _team;
        
        public Team Team => _team;

        [SerializeField] private bool _isDummy;

        [SyncVar(hook = nameof(OnNameChange))] private string _playerName;
        private void Awake()
        {
            _status = GetComponent<StatusController>();
            _health = GetComponent<HealthController>();
            _movement = GetComponent<MovementController>();
            _animation = GetComponent<AnimationController>();
            _client = GetComponent<Client>();
            _ability = GetComponent<AbilityController>();
            _jointFinder = GetComponentInChildren<JointFinder>();
        }

        private void Start()
        {
            if(!_isDummy)
                NetworkManagerA.Instance.AddPlayer(this);
            
            NamePlate namePlate = Instantiate(_namePlatePrefab, ScreenSpaceCanvas.Instance.transform);
            _nameplate = namePlate;
            namePlate.SetUp(transform);
            _health.SetUp(namePlate, _characterData);
            _ability.SetUp(namePlate);
        }

        private void OnDestroy()
        {
            if(_nameplate  !=null)
                Destroy(_nameplate.gameObject);
            NetworkManagerA.Instance.RemovePlayer(this);
        }


        public PlayerBrain SetUp(Team team)
        {
            
            _team = team;
            if(isLocalPlayer)
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
            else
                gameObject.layer = LayerMask.NameToLayer(Team.ToString());
            
            
            if (!isServer) return this;
            //for sure debug
            string pName = Random.Range(0, 100).ToString();
            _playerName = pName + " " + _team;
            name = _playerName;
            return this;
        }
       
        // all debug
        private void OnNameChange(string oldName, string newName)
        {
            name = _playerName;
        }

        private void OnTeamChange(Team oldTeam, Team newTeam)
        {
            Debug.Log(" Team Update to  " + newTeam);
            _team = newTeam;
            gameObject.layer = LayerMask.NameToLayer(_team.ToString());
        }
        
        //Debug ^^^^^
        private void Update()
        { 
          if(_health.IsDead)
              return;
          
          if (isLocalPlayer )
          {
              _movement.RotationInput();
              _movement.Handle();
              
              _animation.Handle();
          }
          
          _status.Handle();
          _ability.Handle();
          _health.Handle();
          _client.Handle();
        }
        
        [Server]
        public void Reset()
        {
            Health.Reset();
            ResetRPC();
        }

        [ClientRpc]
        private void ResetRPC()
        {
            _health.Reset();
        }

        public bool OnSameTeam(PlayerBrain target)
        {
            return _team == target.Team;
        }
    }
}
