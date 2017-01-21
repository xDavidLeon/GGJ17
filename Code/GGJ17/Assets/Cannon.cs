using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    public GameObject cannonballPrefab;
    public Transform launchVector;
    public float explosiveForce;
    public float cooldown = 3.0f;
    public bool autoShoot = false;
    public bool aimPlayer = false;
    private float timer = 0.0f;

    public float currentAngle = 0.0f;
    public float minAngle = -20.0f;
    public float maxAngle = 0.0f;

    public bool playerControlling = false;
    public bool playerInRange = false;

	// Use this for initialization
	void Start () {
        if (autoShoot) InvokeRepeating("Shoot", cooldown + Random.Range(0, cooldown), cooldown);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, 10.0f);

        playerInRange = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 75.0f;
        if (playerControlling == false && aimPlayer && playerInRange)
        {
            Quaternion lookat = Quaternion.LookRotation((GameManager.Instance.player.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-2.0f, 2.0f), Random.Range(-5.0f, 5.0f)) - transform.position).normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.time * 0.1f);
        }
        else if (playerControlling)
        {
            //currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            //Vector3 euler = transform.localEulerAngles;
            //euler.x = currentAngle;
            //transform.localRotation = Quaternion.Euler(euler);
        }
    }

    public void Shoot()
    {
        if (timer > 0) return;
        if (playerControlling == false && !playerInRange) return;
        timer = 3.0f;

        GameObject g = GameObject.Instantiate(cannonballPrefab, launchVector.position, launchVector.rotation) as GameObject;
        g.GetComponent<Rigidbody>().AddForce(launchVector.forward * explosiveForce, ForceMode.Impulse);
    }

    void OnTriggerStay(Collider c)
    {
        //if (playerControlling) return;
        if (c.CompareTag("Player"))
        {
            transform.LookAt(GameManager.Instance.player.m_Camera.transform.position + GameManager.Instance.player.m_Camera.transform.forward * 1000.0f);
            c.GetComponent<Character>().controlledCannon = this;
            playerControlling = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            c.GetComponent<Character>().controlledCannon = null;
            playerControlling = false;
        }
    }
}
