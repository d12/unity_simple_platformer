using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingObject : MonoBehaviour
{
    Rigidbody rb;

    public float buoyancyForce = 1f;

    private float waterHeight = -1.8f;
    private float radius;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        radius = GetComponent<Renderer>().bounds.size.y / 2;
    }

    void FixedUpdate() {
        applyBuoyancyForces(percentageInWater());
    }

    float percentageInWater(){
        Vector3 pos = rb.position;
        float objHeight = pos.y;

        float bottom = waterHeight - radius;
        float top = waterHeight + radius;

        if(objHeight > top) {
            return 0;
        } else if(objHeight < bottom) {
            return 1;
        } else {
            return 1 - ((objHeight - bottom) / (top - bottom));
        }
    }

    void applyBuoyancyForces(float percentageInWaterFloat) {
        rb.AddForce(percentageInWaterFloat * buoyancyForce * Vector3.up * Time.deltaTime);
    }
}
