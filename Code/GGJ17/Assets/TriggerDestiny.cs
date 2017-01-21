using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestiny : MonoBehaviour {
    public bool playerInside = false;
    public bool shipInside = false;

	// Use this for initialization
	void Start () {
        Reset();
	}
	
    void Reset()
    {
        playerInside = false;
        shipInside = false;
    }

	// Update is called once per frame
	void Update () {
        if (shipInside && playerInside)
        {
            GameManager.Instance.GameWin();
            Reset();
        }
	}

    void OnTriggerStay(Collider c)
    {
        if (c.CompareTag("Player")) playerInside = true;
        if (c.CompareTag("Ship")) shipInside = true;
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player")) playerInside = false;
        if (c.CompareTag("Ship")) shipInside = false;
    }
}
