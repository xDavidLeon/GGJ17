using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {
    public float explosionForce = 100.0f;
    public float explosionRadius = 3;
    public GameObject particleExplosion;

    public float aliveTime = 10.0f;

    void Start()
    {
        Destroy(this.gameObject, aliveTime);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude < 2.0f) return;
        GameObject.Instantiate(particleExplosion, transform.position, Quaternion.identity);

        Debug.Log("BOOM_" + c.relativeVelocity.magnitude);
        foreach (Collider col in Physics.OverlapSphere(transform.position, explosionRadius))
        {
            if (col.CompareTag("Destructible"))
            {
                Destroy(col.gameObject);
                continue;
            }
            if (col.GetComponent<Rigidbody>() == null) continue;
            col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 3.0f);
        }

        GetComponent<AudioSource>().Play();
        GameObject.Destroy(this.gameObject);
    }



}
