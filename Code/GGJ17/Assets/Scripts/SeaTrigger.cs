using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Character>() == null) return;

        c.GetComponent<Character>().swimming = true;
    }

    void OnTriggerExit(Collider c)
    {
        if (c.GetComponent<Character>() == null) return;

        c.GetComponent<Character>().swimming = false;
    }
}
