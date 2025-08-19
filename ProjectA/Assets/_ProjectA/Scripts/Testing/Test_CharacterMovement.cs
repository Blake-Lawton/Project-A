using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Test_CharacterMovement : MonoBehaviour
{
     public float moveSpeed = 7f;
    public float acceleration = 30f;
    public float deceleration = 30f;
    public float skin = 0.02f;           // small gap from walls
    public int maxSlides = 3;            // handle corners
    public LayerMask collideMask = ~0;   // what blocks movement

    Rigidbody rb;
    CapsuleCollider cap;
    Vector3 input, vel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cap = GetComponent<CapsuleCollider>();

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; // helps with overlap recovery
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        input = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 target = input * moveSpeed;
        float accel = (input.sqrMagnitude > 0f) ? acceleration : deceleration;
        vel = Vector3.MoveTowards(vel, target, accel * Time.fixedDeltaTime);

        MoveWithCollisions(vel * Time.fixedDeltaTime);
        FaceMovement();
    }

    void MoveWithCollisions(Vector3 delta)
    {
        if (delta.sqrMagnitude < 1e-6f) return;

        for (int i = 0; i < maxSlides && delta.sqrMagnitude > 1e-6f; i++)
        {
            Vector3 p1, p2; float radius;
            GetCapsuleWorld(out p1, out p2, out radius);

            if (Physics.CapsuleCast(p1, p2, radius, delta.normalized, out RaycastHit hit, delta.magnitude + skin, collideMask, QueryTriggerInteraction.Ignore))
            {
                // move up to the wall
                float moveDist = Mathf.Max(hit.distance - skin, 0f);
                rb.MovePosition(rb.position + delta.normalized * moveDist);

                // slide along the hit plane with remaining movement
                Vector3 remaining = delta - delta.normalized * moveDist;
                delta = Vector3.ProjectOnPlane(remaining, hit.normal);
            }
            else
            {
                rb.MovePosition(rb.position + delta);
                break;
            }
        }
    }

    void FaceMovement()
    {
        Vector3 flatVel = new Vector3(vel.x, 0f, vel.z);
        if (flatVel.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatVel);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 0.2f));
        }
    }

    void GetCapsuleWorld(out Vector3 p1, out Vector3 p2, out float radius)
    {
        // assumes capsule aligned to Y
        float height = Mathf.Max(cap.height * Mathf.Abs(transform.lossyScale.y), cap.radius * 2f);
        radius = cap.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        Vector3 center = transform.TransformPoint(cap.center);

        float half = Mathf.Max(0f, (height * 0.5f) - radius);
        p1 = center + Vector3.up * half;
        p2 = center - Vector3.up * half;
    } 
}
