using _ProjectA.Scripts.Interface;

namespace Data.Interaction
{
    
    public class InteractionData
    {
        private IDamager _perp;
        private IDamageable _victim;
        public IDamager Perp => _perp;
        public IDamageable Victim => _victim;
       
        public InteractionData()
        {
           
            
        }
        
    }
}
