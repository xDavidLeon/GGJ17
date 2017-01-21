using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour {
    public Ship ship;
    public Transform clothMesh;
    public bool extended = false;
    public float extendSpeed = 0.5f;
    public float extendProgress = 0.0f;

    public float sailForce = 1000;

	// Use this for initialization
	void Start () {
        if (extended) Extend();
        else Retract();
	}

    void Update()
    {
        if (extended)
        {
            extendProgress += Time.deltaTime * extendSpeed;
            extendProgress = Mathf.Clamp(extendProgress, 0.1f, 1.0f);
        }
        else
        {
            extendProgress -= Time.deltaTime * extendSpeed;
            extendProgress = Mathf.Clamp(extendProgress, 0.1f, 1.0f);
        }

        clothMesh.localScale = new Vector3(1, extendProgress, 1);
    }
	
    public void Extend()
    {
        Debug.Log("Extending sail " + gameObject.name);
        extended = true;
    }

    public void Retract()
    {
        Debug.Log("Retracting sail " + gameObject.name);
        extended = false;
    }

    public void Activate()
    {
        if (extended) Retract();
        else Extend();
    }
}
