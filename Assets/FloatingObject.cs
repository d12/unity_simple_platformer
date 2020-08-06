using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingObject : MonoBehaviour
{
    Rigidbody rb;

    public float buoyancyForce = 1f;
    public float dampenForce = 0.01f;

    private float waterHeight = -1.8f;
    private float radius;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        radius = GetComponent<Renderer>().bounds.size.y / 2;
    }

    void FixedUpdate() {
        Vector3[] objectCorners = objectCornersWorldSpace();

        applyBuoyancyForces(objectCorners);
    }

    float percentageInWater(float objHeight){
        if(waterHeight < objHeight) {
            return 0;
        } else {
            return Mathf.Pow(waterHeight - objHeight, 2);
        }
    }

    void applyBuoyancyForces(Vector3[] corners) {
        foreach (Vector3 corner in corners)
        {
            float percentageInWaterFloat = percentageInWater(corner.y);
            Vector3 force = percentageInWaterFloat * buoyancyForce * Vector3.up * Time.deltaTime;
            rb.AddForceAtPosition(force, corner);
        }
    }

    void applyVelocityDapening() {
        Vector3 velocity = rb.velocity;
        velocity = velocity * (1 - dampenForce);
        rb.velocity = velocity;
    }

    Vector3[] objectCornersWorldSpace() {
        Vector3[] vertices = new Vector3[4];

        vertices[0] = transform.TransformPoint(new Vector3(.5f, -.5f, -.5f));
        vertices[1] = transform.TransformPoint(new Vector3(.5f, -.5f, .5f));
        vertices[2] = transform.TransformPoint(new Vector3(-.5f, -.5f, -.5f));
        vertices[3] = transform.TransformPoint(new Vector3(-.5f, -.5f, .5f));

        return vertices;
    }
}
