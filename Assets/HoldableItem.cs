using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    public GameObject player;

    Rigidbody _rb;
    float velocityDampenForce = 0.2f;
    float angularDampenForce = 0.05f;

    // How far can we reach things?
    float reachLength = 4.5f;

    // How far infront of ourselves do we hold objects?
    float holdLength = 3.0f;

    bool isHeld;
    bool isMousedOver;

    Outline outline;
    Camera camera;

    Quaternion originalItemRotation;
    Quaternion originalCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
        _rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        camera = Camera.main;
    }

    void FixedUpdate()
    {
        if(isHeld) {
            // Pull object towards cursor
            _rb.AddForce((desiredItemHoldPosition() - transform.position) * Time.fixedDeltaTime * 10000f);

            // Dampen velocity and angular velocity so the item sticks to the cursor instead of bouncing around
            _rb.velocity = _rb.velocity * (1 - velocityDampenForce);
            _rb.AddTorque(_rb.angularVelocity * -1.0f * (1 - angularDampenForce), ForceMode.Force);

            // Rotate the object correctly
            Quaternion deltaRotation = camera.transform.rotation * Quaternion.Inverse(originalCameraRotation);
            _rb.rotation = originalItemRotation * deltaRotation;
        }
    }

    void OnMouseEnter() {
        if(canHoldObject() && !Helpers.instance.getPlayerState().isHoldingSomething) {
            isMousedOver = true;
            outline.enabled = true;
        }
    }

    void OnMouseDown() {
        if(canHoldObject()){
            isHeld = true;
            outline.enabled = false;
            Helpers.instance.getPlayerState().isHoldingSomething = true;
            originalItemRotation = _rb.rotation;
            originalCameraRotation = camera.transform.rotation;
        }
    }

    void OnMouseExit() {
        isMousedOver = false;
        outline.enabled = false;
    }

    void OnMouseUp() {
        if(isHeld){
            isHeld = false;
            Helpers.instance.getPlayerState().isHoldingSomething = false;
        }

        if(isMousedOver){
            outline.enabled = true;
        }
    }

    bool canHoldObject() {
        return (distanceBetweenPlayerAndObject() < reachLength);
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
