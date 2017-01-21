using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 10.0f;
    public bool swimming = false;
    public bool climbing = false;

    private float horizontal, vertical;

    [Header("Grab")]
    public float grabDistance = 2.0f;
    public GameObject grabbedObject = null;
    public float grabLerpTime = 0.1f;
    public float throwForce = 1.0f;

    private RaycastHit hit;

    [Header("Ship")]
    public Ship currentShip;
    public Cannon controlledCannon;

    [SerializeField]
    private MouseLook m_MouseLook;
    public Camera m_Camera;

    void Awake()
    {
    }

    void Start()
    {
        m_MouseLook.Init(transform, m_Camera.transform);
    }

    void Update()
    {
        if (GameManager.Instance.camMode == GameManager.CAMERA_MODE.AERIAL) return;
        RotateView();
        UpdateMovement();

        if (grabbedObject != null)
        {
            Vector3 desiredObjectPosition = m_Camera.transform.position + m_Camera.transform.forward * grabDistance;
            grabbedObject.transform.position = (Vector3.Lerp(grabbedObject.GetComponent<Rigidbody>().position, desiredObjectPosition, grabLerpTime));

            if (Input.GetMouseButtonDown(0)) ReleaseObject();
            else if (Input.GetMouseButtonDown(1)) ThrowObject();
        }
        else
        {
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, grabDistance, (1 << LayerMask.NameToLayer("Pickup"))))
            {
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.green);
                Debug.Log(hit.transform.name);
                if (Input.GetMouseButtonDown(0)) GrabObject(hit.transform.gameObject);
            }
            else
            {
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.red);

                if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, grabDistance))
                {
                    if ( currentShip != null )
                    {
                        if (Input.GetMouseButton(0))
                        {
                            if (hit.collider == currentShip.controlWheel.left) currentShip.RotateLeft();
                            else if (hit.collider == currentShip.controlWheel.right) currentShip.RotateRight();
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (hit.collider.GetComponent<Sail>()) hit.collider.GetComponent<Sail>().Activate();
                        }
                    }
                }
            }
        }

        if (controlledCannon != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                controlledCannon.Shoot();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                controlledCannon.currentAngle += 10.0f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                controlledCannon.currentAngle -= 10.0f * Time.deltaTime;
            }
        }
    }

    public void GrabObject(GameObject g)
    {
        if (g.GetComponent<Rigidbody>() == null) return;
        if (g.GetComponent<Rigidbody>().CompareTag("Pickup") == false) return;

        grabbedObject = g;
        grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        //grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void ReleaseObject()
    {
        if (grabbedObject == null) return;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * 1.0f, ForceMode.Impulse);
        //grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
    }

    public void ThrowObject()
    {
        if (grabbedObject == null) return;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * throwForce, ForceMode.Impulse);
        //grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
    }

    void UpdateMovement()
    {
        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 newPosition = transform.position;
        float speed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed = runSpeed;

        if (climbing)
        {
            newPosition += m_Camera.transform.right * horizontal * speed;
            newPosition += m_Camera.transform.forward * vertical * speed;
        }
        else
        {
            newPosition += transform.right * horizontal * speed;
            newPosition += transform.forward * vertical * speed;
        }

        transform.position = newPosition;

        if (Input.GetButtonDown("Jump") && (swimming || Physics.Raycast(transform.position, -transform.up, 0.5f)))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    void OnDrawGizmos()
    {

    }

}
