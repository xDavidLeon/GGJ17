using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour {
    public Vector2 waterLevelVariance = Vector2.zero;
    public float waterLevelSpeed = 1.0f;
    public float waterLevel = 0.0f;

    public float floatHeight = 2.0f;
    public float bounceDamp = 0.0f;

    public bool generateBuoyancyPoints = false;
    public Vector3[] buoyancyPoints;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        if (buoyancyPoints.Length == 0 && generateBuoyancyPoints)
        {
            buoyancyPoints = new Vector3[4];
            buoyancyPoints[0] = new Vector3(0.5f, 0, 0.5f);
            buoyancyPoints[1] = new Vector3(0.5f, 0, -0.5f);
            buoyancyPoints[2] = new Vector3(-0.5f, 0, 0.5f);
            buoyancyPoints[3] = new Vector3(-0.5f, 0, -0.5f);
        }
    }

    void Update()
    {
        waterLevel = Mathf.Lerp(waterLevelVariance.x, waterLevelVariance.y, (1 + Mathf.Sin(Time.timeSinceLevelLoad * waterLevelSpeed))/2.0f);
    }
	
    void FixedUpdate()
    {
        foreach (Vector3 point in buoyancyPoints) ApplyBuoyancyForce(point);
    }

    void ApplyBuoyancyForce(Vector3 point)
    {
        Vector3 actionPoint = transform.position + transform.TransformDirection(point);
        float forceFactor = 1.0f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0.0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
            rb.AddForceAtPosition(uplift * rb.mass, actionPoint);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.75F);
        if (buoyancyPoints.Length == 0) return;
        foreach (Vector3 point in buoyancyPoints)
        {
            Vector3 actionPoint = transform.position + transform.TransformDirection(point);
            Gizmos.DrawCube(actionPoint, new Vector3(0.2f, 0.2f,0.2f));
        }
    }
}
