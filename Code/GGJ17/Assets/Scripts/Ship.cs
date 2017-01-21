using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public GameObject controlWheel;
    public List<Sail> sails;
    public List<GameObject> cargo = new List<GameObject>();
    public Transform cargoHolder;

    public float currentSpeed = 0.0f;

    public float maxSpeed = 100.0f;

    public float rotationSpeed = 1.0f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Start () {
		
	}
	
	void Update () {
        currentSpeed = rb.velocity.magnitude;
	}

    void FixedUpdate()
    {
        foreach(Sail s in sails)
        {
            if (s.extended == false) continue;
            rb.AddForceAtPosition(transform.forward * s.sailForce, s.transform.position);
        }

        if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    public void RotateLeft()
    {
        rb.AddForceAtPosition(transform.right * -rotationSpeed, transform.position + transform.forward * 10);

        controlWheel.transform.Rotate(0, 0, 100.0f * Time.fixedDeltaTime, Space.Self);
    }

    public void RotateRight()
    {
        rb.AddForceAtPosition(transform.right * rotationSpeed, transform.position + transform.forward *10);

        controlWheel.transform.Rotate(0, 0, -100.0f * Time.fixedDeltaTime, Space.Self);
    }

    public void CollectCargo(GameObject pickup)
    {
        Debug.Log("Cargo collected: " + pickup.name);
        Vector3 rndPosWithin;
        rndPosWithin = cargoHolder.transform.position + new Vector3(Random.Range(-cargoHolder.localScale.x/2.0f, cargoHolder.localScale.x/2.0f), Random.Range(-cargoHolder.localScale.y / 2.0f, cargoHolder.localScale.y / 2.0f), Random.Range(-cargoHolder.localScale.z / 2.0f, cargoHolder.localScale.z / 2.0f));
        pickup.transform.position = rndPosWithin;
        pickup.transform.rotation = Quaternion.identity;
    }

    public void AddCargo(GameObject pickup)
    {
        if (cargo.Contains(pickup) == false)
            cargo.Add(pickup);
    }

    public void RemoveCargo(GameObject pickup)
    {
        if (cargo.Contains(pickup) == true)
            cargo.Remove(pickup);
    }

    public int CountCargo()
    {
        return cargo.Count;
    }
}
