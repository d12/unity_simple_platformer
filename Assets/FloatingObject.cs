using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingObject : MonoBehaviour
{
    Rigidbody rb;

    // Linearly scales the force of buoyancy
    // This has to be tuned based on the number of buoyancy points you define,
    // the mass of your object, and the force of gravity.
    public float buoyancyForce = 1000f;

    // A value between 0 and 1, higher dampenForce causes rotational energy
    // to dampen faster. 0.01 seems good in my experience. Not needed if your water
    // applies a force to your floating objects.
    public float dampenForce = 0.01f;

    // The height of your water.
    // TODO: Be able to find the height of the water below the point
    // This is easy with static water but very difficult with wavy water and no mesh collider on the water.
    public float waterHeight = -1.8f;

    public bool cube = true;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        Vector3[] objectCorners = ObjectCornersWorldSpace();

        ApplyBuoyancyForces(objectCorners);
    }

    void ApplyBuoyancyForces(Vector3[] corners) {
        foreach (Vector3 corner in corners)
        {
            float depthInWaterForceFactor = DepthInWater(corner.y);
            Vector3 force = Vector3.up * (depthInWaterForceFactor * buoyancyForce * Time.deltaTime);

            rb.AddForceAtPosition(force, corner);
        }
    }

    float DepthInWater(float objHeight){
        return Mathf.Abs(waterHeight - objHeight);
    }

    void ApplyVelocityDapening() {
        Vector3 velocity = rb.velocity;
        velocity = velocity * (1 - dampenForce);
        rb.velocity = velocity;
    }

    // This returns the four bottom corners of a cube in world space.
    // To use this script with a non-cube, return at least 3 evenly spaced points
    // on the bottom of your object.
    Vector3[] ObjectCornersWorldSpace() {
        if(cube) {
            Vector3[] vertices = new Vector3[4];

            vertices[0] = transform.TransformPoint(new Vector3(.5f, -.5f, -.5f));
            vertices[1] = transform.TransformPoint(new Vector3(.5f, -.5f, .5f));
            vertices[2] = transform.TransformPoint(new Vector3(-.5f, -.5f, -.5f));
            vertices[3] = transform.TransformPoint(new Vector3(-.5f, -.5f, .5f));

            return vertices;
        } else {
            Vector3[] vertices = new Vector3[1];
            vertices[0] = transform.position;
            Debug.Log(vertices[0]);

            return vertices;
        }
    }
}
