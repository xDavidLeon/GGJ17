using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {
    public GameObject cratePrefab;
    public GameObject container;
    public int numCrates = 100;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numCrates; i++)
        {
            SpawnCrate().name = "Crate " + i;
        }
	}
	
    public GameObject SpawnCrate()
    {
        Vector3 rndPosWithin;
        rndPosWithin = transform.position + new Vector3(Random.Range(-transform.localScale.x / 2.0f, transform.localScale.x / 2.0f), Random.Range(-transform.localScale.y / 2.0f, transform.localScale.y / 2.0f), Random.Range(-transform.localScale.z / 2.0f, transform.localScale.z / 2.0f));
        GameObject pickup = GameObject.Instantiate(cratePrefab, rndPosWithin, Quaternion.identity);
        pickup.transform.parent = container.transform;
        return pickup;
    }
}
