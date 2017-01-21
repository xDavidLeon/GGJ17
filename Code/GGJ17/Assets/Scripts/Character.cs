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

    [SerializeField]
    private MouseLook m_MouseLook;
    private Camera m_Camera;

    void Awake()
    {
    }

    void Start()
    {
        m_Camera = Camera.main;
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
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, grabDistance))
            {
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.green);
                if (Input.GetMouseButtonDown(0)) GrabObject(hit.transform.gameObject);
            }
            else
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.red);
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
