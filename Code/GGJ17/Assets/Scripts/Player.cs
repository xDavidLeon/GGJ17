using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public enum PLAYER_N
    {
        PLAYER_1 = 1,
        PLAYER_2 = 2
    };

    public int playerN = 1;

    [Header("Movement")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 10.0f;

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

    protected override void Init()
    {
        base.Init();
        m_MouseLook.Init(this.transform, m_Camera.transform);
    }

    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        RotateView();
        if (GameManager.Instance.camMode == GameManager.CAMERA_MODE.AERIAL) return;
        UpdateMovement();

        if (grabbedObject != null)
        {
            Vector3 desiredObjectPosition = m_Camera.transform.position + m_Camera.transform.forward * grabDistance;
            grabbedObject.transform.position = (Vector3.Lerp(grabbedObject.GetComponent<Rigidbody>().position, desiredObjectPosition, grabLerpTime));

            if (Input.GetButtonDown("Action_" + (int)playerN)) ThrowObject();
            //else if (Input.GetMouseButtonDown(1)) ThrowObject();
        }
        else
        {
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, grabDistance, (1 << LayerMask.NameToLayer("Pickup"))))
            {
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.green);
                if (Input.GetButtonDown("Action_" + (int)playerN)) GrabObject(hit.transform.gameObject);
            }
            else
            {
                Debug.DrawLine(m_Camera.transform.position, m_Camera.transform.position + m_Camera.transform.forward * grabDistance, Color.red);

                if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, grabDistance))
                {
                    if (currentShip != null)
                    {
                        if (Input.GetButton("Action_" + (int)playerN))
                        {
                            if (hit.collider == currentShip.controlWheel.left) currentShip.RotateLeft();
                            else if (hit.collider == currentShip.controlWheel.right) currentShip.RotateRight();
                        }
                        if (Input.GetButtonDown("Action_" + (int)playerN))
                        {
                            if (hit.collider.GetComponent<Sail>()) hit.collider.GetComponent<Sail>().Activate();
                        }
                    }
                }
            }
        }

        if (controlledCannon != null)
        {
            if (Input.GetButtonDown("Action_" + (int)playerN))
            {
                controlledCannon.Shoot();
            }
            //if (Input.GetKey(KeyCode.Q))
            //{
            //    controlledCannon.currentAngle += 10.0f * Time.deltaTime;
            //}
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    controlledCannon.currentAngle -= 10.0f * Time.deltaTime;
            //}
        }
    }

    public void GrabObject(GameObject g)
    {
        if (g.GetComponent<Rigidbody>() == null) return;
        //if (g.GetComponent<Rigidbody>().CompareTag("Pickup") == false) return;

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
        grabbedObject.GetComponent<Rigidbody>().AddForce(m_Camera.transform.forward * throwForce * grabbedObject.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        //grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
    }

    void UpdateMovement()
    {
        horizontal = Input.GetAxis("Horizontal_" + (int) playerN) * Time.deltaTime;
        vertical = Input.GetAxis("Vertical_" + (int)playerN) * Time.deltaTime;

        Vector3 newPosition = transform.position;
        float speed = walkSpeed;
        if (Input.GetButton("Sprint_" + (int)playerN)) speed = runSpeed;


        newPosition += transform.right * horizontal * speed;
        newPosition += transform.forward * vertical * speed;

        transform.position = newPosition;

        if (Input.GetButtonDown("Jump_" + (int)playerN) && (swimming || Physics.Raycast(transform.position, -transform.up, 0.5f)))
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
}
