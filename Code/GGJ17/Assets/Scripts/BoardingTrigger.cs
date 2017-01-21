using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        //if (c.GetComponent<Buoyancy>() != null) c.GetComponent<Buoyancy>().enabled = false;
        if (c.GetComponent<Character>() == null) return;

        Debug.Log("Character " + c.name + " attached");
        c.transform.SetParent(transform.parent);
    }

    void OnTriggerExit(Collider c)
    {
        //if (c.GetComponent<Buoyancy>() != null) c.GetComponent<Buoyancy>().enabled = true;
        if (c.GetComponent<Character>() == null) return;
        Debug.Log("Character " + c.name + " detached");
        c.transform.SetParent(null);
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
