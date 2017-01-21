using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingTrigger : MonoBehaviour {
    public Ship ship;

	// Use this for initialization
	void Start () {
		
	}
	
    void OnTriggerEnter(Collider c)
    {
        //if (c.GetComponent<Buoyancy>() != null) c.GetComponent<Buoyancy>().enabled = false;
        if (c.GetComponent<Character>() != null)
        {
            c.GetComponent<Character>().currentShip = ship;
        }

        if (c.CompareTag("Pickup"))
        {
            ship.AddCargo(c.gameObject);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Pickup"))
        {
            ship.RemoveCargo(c.gameObject);
        }
    }

    //void OnTriggerEnter(Collider c)
    //{
    //    if (c.GetComponent<Buoyancy>() == null) return;

    //    c.GetComponent<Buoyancy>().enabled = true;
    //}

    //void OnTriggerExit(Collider c)
    //{
    //    if (c.GetComponent<Buoyancy>() == null) return;

    //    c.GetComponent<Buoyancy>().enabled = false;
    //}
}
