using _ProjectA.Scripts.Networking;
using Vector3 = UnityEngine.Vector3;

namespace Data.Types
{
    
    public struct ServerStateSnapShot
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float TimeStamp;
    }
    
    
    public struct CspState
    {
        public uint Tick;
        public Vector3 Position;
    }

    // ReSharper disable once DefaultStructEqualityIsUsed.Global
    public struct InputPayLoad
    {
        public uint Tick;
        public Vector3 Movement;
        public Vector3 Rotation;
        public bool Jump;
        
        public InputPayLoad(Client client)
        {
            Tick = client.Csp.CurrentTick;
            Movement = client.Movement.MoveDirection;
            Rotation = client.Movement.Rotation;
            Jump = client.Movement.InputJump;
     
        }
    }
}
