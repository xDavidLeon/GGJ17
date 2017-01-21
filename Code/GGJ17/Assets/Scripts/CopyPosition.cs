using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour {
    public Transform target;

    public bool copyX = true;
    public bool copyY = true;
    public bool copyZ = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        if (copyX) newPosition.x = target.position.x;
        if (copyY) newPosition.y = target.position.y;
        if (copyZ) newPosition.z = target.position.z;
        transform.position = newPosition;
    }
}
