using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public GameObject skeleton;
    Rigidbody[] subRigidbodies;
    Collider[] subColliders;

    // Start is called before the first frame update
    void Start()
    {
        subRigidbodies = skeleton.GetComponentsInChildren<Rigidbody>();
        subColliders = skeleton.GetComponentsInChildren<Collider>();

        Debug.Log("Rigid Bodies: " + subRigidbodies.Length);
        Debug.Log("Colliders: " + subColliders.Length);

        disableRigidbodies();
        //disableColliders();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void disableRigidbodies(){
        foreach(Rigidbody r in subRigidbodies) {
            r.isKinematic = true;
        }
    }

    void disableColliders(){
        foreach(Collider c in subColliders) {
            c.enabled = false;
        }
    }
}
