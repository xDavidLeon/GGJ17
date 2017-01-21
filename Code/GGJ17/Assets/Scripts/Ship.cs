using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public float currentSpeed = 0.0f;

    public float minSpeed = 1.0f;
    public float maxSpeed = 100.0f;
    public float forceFirstSail = 5.0f;
    public float forceSecondSail = 5.0f;
    public float forceThirdSail = 5.0f;

    public float rotationSpeed = 1.0f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {




        //Vector3 rot = transform.eulerAngles;
        //if (transform.eulerAngles.x > maxAnglePitch) rot.x = maxAnglePitch;
        //else if (transform.eulerAngles.x < 360 -maxAnglePitch) rot.x = -maxAnglePitch;
        //if (transform.eulerAngles.z > maxAngleRoll) rot.z = maxAngleRoll;
        //else if (transform.eulerAngles.z < -maxAngleRoll) rot.z = -maxAngleRoll;
        //transform.eulerAngles = rot;
        currentSpeed = rb.velocity.magnitude;
	}

    void FixedUpdate()
    {
        rb.AddForceAtPosition(transform.forward * forceFirstSail, transform.position + transform.forward * 5.0f);
        rb.AddForceAtPosition(transform.forward * forceSecondSail, transform.position);
        rb.AddForceAtPosition(transform.forward * forceThirdSail, transform.position - transform.forward * 5.0f);

        if (Input.GetKey(KeyCode.Q))
        {
            RotateLeft(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            RotateRight(rotationSpeed);
        }


        if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;


        //Vector3 actionPoint = transform.position + transform.TransformDirection(new Vector3(Random.Range(-1,1), 0, Random.Range(-1,1)));
        //float forceFactor = 1.0f - ((actionPoint.y - waterLevel) / floatHeight);

        //if (forceFactor > 0.0f)
        //{
        //    Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
        //    rb.AddForceAtPosition(uplift, actionPoint);
        //}
    }

    public void RotateLeft(float force)
    {
        rb.AddForceAtPosition(transform.right * -force, transform.position + transform.forward * 10);

        //transform.Rotate(0, force * Time.deltaTime, 0);
    }

    public void RotateRight(float force)
    {
        rb.AddForceAtPosition(transform.right * force, transform.position + transform.forward *10);

        //transform.Rotate(0, -force * Time.deltaTime, 0);
    }
}
