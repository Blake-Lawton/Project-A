using _ProjectA.Scripts.Controllers;
using Data.Interaction;

namespace _ProjectA.Scripts.Interface
{
    public interface IDamageable 
    {
        public PlayerBrain Brain { get; set; }
        public void TakeDamage(InteractionData data);
        public void Heal(int heal);
    }
}
