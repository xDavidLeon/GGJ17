using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Transform SpawnPoint;

	// Use this for initialization
	void Start () {
        ResetPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) ResetPlayer();
	}

    public void ResetPlayer()
    {
        GameObject.Find("Player").transform.position = SpawnPoint.position;
    }
}
