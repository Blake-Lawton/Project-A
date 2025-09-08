using UnityEngine;

namespace _ProjectA.Scripts.Controllers
{
   public class AnimationController : MonoBehaviour
   {
      private Animator _animator;
      private MovementController _movement;

      public Animator Animator => _animator;
      private void Awake()
      {
         _animator = GetComponentInChildren<Animator>();
         _movement = GetComponent<MovementController>();
      }

      public void Handle()
      {
         MovementAnims();
      }


      private void MovementAnims()
      {
         float velocityZ = Vector3.Dot(_movement.MoveDirection.normalized, transform.forward);
         float velocityX = Vector3.Dot(_movement.MoveDirection.normalized, transform.right);

         _animator.SetFloat("Horizontal", velocityX, 1f, Time.deltaTime * 10f);
         _animator.SetFloat("Vertical", velocityZ, 1f, Time.deltaTime * 10f);
         
         _animator.SetBool("IsMoving",_movement.MoveDirection.magnitude > 0.1f);
      }
   }
}