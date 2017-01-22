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
    public float AIrange = 100.0f;
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

        foreach(Player player in GameManager.Instance.players)
        {
            playerInRange = Vector3.Distance(transform.position, player.transform.position) < AIrange;
            if (playerControlling == false && aimPlayer && playerInRange)
            {
                Quaternion lookat = Quaternion.LookRotation((player.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f)) - transform.position).normalized, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.time * 0.1f);
                break;
            }
        }

    }

    public void Shoot()
    {
        if (timer > 0) return;
        if (playerControlling == false && !playerInRange) return;
        timer = cooldown;
        GetComponent<AudioSource>().Play();

        GameObject g = GameObject.Instantiate(cannonballPrefab, launchVector.position, launchVector.rotation) as GameObject;
        g.GetComponent<Rigidbody>().AddForce(launchVector.forward * explosiveForce, ForceMode.Impulse);
    }

    void OnTriggerStay(Collider c)
    {
        //if (playerControlling) return;
        if (c.CompareTag("Player"))
        {
            transform.LookAt(c.GetComponent<Player>().m_Camera.transform.position + c.GetComponent<Player>().m_Camera.transform.forward * 1000.0f);
            c.GetComponent<Player>().controlledCannon = this;
            playerControlling = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            c.GetComponent<Player>().controlledCannon = null;
            playerControlling = false;
        }
    }
}
