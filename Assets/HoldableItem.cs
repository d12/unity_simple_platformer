using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    bool isHeld;
    public GameObject player;
    public float reachLength = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
    }

    void Update()
    {
        if(isHeld) {
            
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
        return Camera.main.transform.forward * reachLength;
    }

    float distanceBetweenPlayerAndObject() {
        return (transform.position - player.transform.position).magnitude;
    }
}
