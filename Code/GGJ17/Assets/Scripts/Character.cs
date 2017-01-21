using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Character : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;

    private float horizontal, vertical;

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
        RotateView();

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            newPosition += transform.right * horizontal * runSpeed;
            newPosition += transform.forward * vertical * runSpeed;
        }
        else
        {
            newPosition += transform.right * horizontal * walkSpeed;
            newPosition += transform.forward * vertical * walkSpeed;
        }

        transform.position = newPosition;
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
