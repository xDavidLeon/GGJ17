using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player") == false) return;

        c.GetComponent<Character>().climbing = true;
        c.GetComponent<Rigidbody>().isKinematic = false;
        c.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        c.transform.SetParent(this.transform);
    }

    void OnTriggerStay(Collider c)
    {
        if (c.CompareTag("Player") == false) return;

        c.GetComponent<Character>().climbing = true;
        c.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        c.GetComponent<Rigidbody>().freezeRotation = true;

        c.transform.SetParent(this.transform);
    }


    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player") == false) return;

        c.GetComponent<Character>().climbing = false;
        c.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        c.transform.SetParent(null);
    }
}
