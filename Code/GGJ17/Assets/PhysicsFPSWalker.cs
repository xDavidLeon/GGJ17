using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PhysicsFPSWalker : MonoBehaviour
{
    [SerializeField]
    private MouseLook m_MouseLook;
    private Camera m_Camera;

    private Rigidbody rb;

    // These variables are for adjusting in the inspector how the object behaves 
    public float maxSpeed = 7;
    public float force = 8;
    public float jumpSpeed = 5;

    // These variables are there for use by the script and don't need to be edited
    private int state = 0;
    private bool grounded = false;
    private float jumpLimit = 0;

    public float gravityFactor = 1.0f;

    // Don't let the Physics Engine rotate this physics object so it doesn't fall over when running
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        m_Camera = Camera.main;
    }

    void Start()
    {
        m_MouseLook.Init(transform, m_Camera.transform);
    }

    void Update()
    {
        RotateView();

        Debug.Log(rb.velocity.magnitude);
    }

    // This part detects whether or not the object is grounded and stores it in a variable
    void OnCollisionEnter()
    {
        state++;
        if (state > 0)
        {
            grounded = true;
        }
    }

    void OnCollisionExit()
    {
        state--;
        if (state < 1)
        {
            grounded = false;
            state = 0;
        }
    }

    public virtual bool jump
    {
        get
        {
            return Input.GetButtonDown("Jump");
        }
    }

    public virtual float horizontal
    {
        get
        {
            return Input.GetAxis("Horizontal") * force;
        }
    }

    public virtual float vertical
    {
        get
        {
            return Input.GetAxis("Vertical") * force;
        }
    }
   
    // This is called every physics frame
    void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        vel.y = 0.0f;

        // If the object is grounded and isn't moving at the max speed or higher apply force to move it
        if (vel.magnitude < maxSpeed && grounded == true)
        {
            rb.AddForce(transform.rotation * Vector3.forward * vertical);
            rb.AddForce(transform.rotation * Vector3.right * horizontal);
        }

        // This part is for jumping. I only let jump force be applied every 10 physics frames so
        // the player can't somehow get a huge velocity due to multiple jumps in a very short time
        if (jumpLimit < 10) jumpLimit++;

        if (jump && grounded && jumpLimit >= 10)
        {
            rb.velocity = rb.velocity + (Vector3.up * jumpSpeed);
            jumpLimit = 0;
        }

        rb.AddForce(Vector3.up * Physics.gravity.y * (gravityFactor - 1));
        m_MouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }
}