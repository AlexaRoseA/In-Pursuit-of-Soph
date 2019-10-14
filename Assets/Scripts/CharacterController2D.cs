using UnityEngine;
using UnityEngine.UI;


public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    private BoxCollider2D boxCollider;

    [SerializeField]
    private Slider healthbar;
    HealthBarController healthscript;

    private Vector2 velocity;

    private bool switched = false;
    private bool flight = false;

    private Animator animator;

    [SerializeField]
    public bool dirRight = true;

    [SerializeField]
    public bool BATTLETIME = false;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool grounded;

    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        healthscript = GetComponent<HealthBarController>();
    }

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float moveInput = Input.GetAxisRaw("Horizontal"); 

        if (grounded)
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                // Calculate the velocity required to achieve the target jump height.
                velocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                // Calculate the velocity required to achieve the target jump height.
                flight = true;
                animator.SetBool("flying", true);
                velocity.y = Mathf.Sqrt(20 * jumpHeight);
            }
        }


        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        if(!flight)
        {
            velocity.y += Physics2D.gravity.y * Time.deltaTime;
        } else
        {
            velocity.y += Physics2D.gravity.y * (Time.deltaTime/1.5f);
        }

        transform.Translate(velocity * Time.deltaTime);

        if(velocity.x != 0 && !Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }

        grounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                    flight = false;
                    animator.SetBool("flying", false);
                }
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            // flip the sprite
            transform.localScale = new Vector3(-1, 1, 1);
            dirRight = false;
        } else if (Input.GetKey(KeyCode.D))
        {
            dirRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

        if(BATTLETIME)
        {
            BossAttack script; //creates that script data type
            GameObject boss;

            boss = GameObject.Find("Boss");

            script = boss.GetComponent<BossAttack>();

            script.enabled = true;
        }
    }
}
