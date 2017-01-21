using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
    public Ship shipToControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider c)
    {
        if (c.CompareTag("Player") == false) return;
        if (Input.GetKey(KeyCode.Q))
        {
            shipToControl.RotateLeft();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            shipToControl.RotateRight();
        }
    }
}
