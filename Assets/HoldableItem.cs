using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    public GameObject player;

    Rigidbody _rb;
    float dampenForce = 0.9f;

    // How far can we reach things?
    float reachLength = 6.0f;

    // How far infront of ourselves do we hold objects?
    float holdLength = 4.0f;

    bool isHeld;

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(isHeld) {
            _rb.AddForce((desiredItemHoldPosition() - transform.position) * Time.fixedDeltaTime * 10000f);

            _rb.velocity = _rb.velocity * dampenForce;
            _rb.AddTorque(_rb.angularVelocity * -1.0f * (1 - dampenForce), ForceMode.Force);
        }
    }

    void OnMouseDown() {
        Debug.Log(distanceBetweenPlayerAndObject());

        if(distanceBetweenPlayerAndObject() < reachLength){
            isHeld = true;
        }
    }

    void OnMouseUp() {
        if(isHeld){
            isHeld = false;
        }
    }

    // What pos do we want to hold the object in?
    // We may not be able to move the object exactly here in case of collisions.
    Vector3 desiredItemHoldPosition() {
        // Camera.main.transform.forward returns a normalized world space vector
        // representing the direction the camera is facing.
        return Camera.main.transform.position + (Camera.main.transform.forward * holdLength);
    }

    float distanceBetweenPlayerAndObject() {
        return (transform.position - player.transform.position).magnitude;
    }
}
