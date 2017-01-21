using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoCollector : MonoBehaviour {
    public Ship ship;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Pickup") == false) return;
        ship.CollectCargo(c.gameObject);
    }
}
