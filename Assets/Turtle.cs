using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float baseSpeed = 100f;
    public float maxVelocity = 3f;
    public float recalculatePathFrequency = 10f;
    public float restFrequency = 0.1f;
    bool isWalking;

    Transform transform;
    Rigidbody rb;
    Animator anim;

    public GameObject target1;
    Transform target1Trans;

    public GameObject target2;
    Transform target2Trans;

    Transform currentTarget;

    float lastCalculatedWalkDirection;
    Vector3 walkDirection;

    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        target1Trans = target1.transform;
        target2Trans = target2.transform;

        if (Random.value < .5) {
            currentTarget = target1Trans;
        } else {
            currentTarget = target2Trans;
        }

        lastCalculatedWalkDirection = Time.time;

        SetIsWalkingState();
        SetLookDirection();
    }

    void FixedUpdate()
    {
        if(ShouldRecalculateMovementInfo()){
            SetIsWalkingState();
            if(isWalking) {
                SetLookDirection();
            }

            lastCalculatedWalkDirection = Time.time + (Random.value * 2);
        }

        if(isWalking) {
            rb.AddForce(transform.forward * Time.deltaTime * baseSpeed * rb.mass);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    void RecalculateTarget() {
        float distanceToTarget = Mathf.Abs((transform.position - currentTarget.position).magnitude);
        if(distanceToTarget < 3f) {
            if(currentTarget == target1Trans) {
                currentTarget = target2Trans;
            } else {
                currentTarget = target1Trans;
            }
        }
    }

    Vector3 CalculateWalkDirection() {
        Vector3 walkDirection = Vector3.Normalize(currentTarget.position - transform.position);
        Vector3 randomness = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);

        return walkDirection + randomness;
    }

    // Rotate the character to look towards the walkDirection
    void SetLookDirection() {
        // If we reach the target, RecalculateTarget will assign a new target.
        RecalculateTarget();

        Vector3 direction = CalculateWalkDirection();
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    void SetIsWalkingState() {
        isWalking = (restFrequency < Random.value);
        anim.SetBool("walking", isWalking);
    }

    bool ShouldRecalculateMovementInfo() {
        float timeSinceLastRecalculate = Time.time - lastCalculatedWalkDirection;

        return (timeSinceLastRecalculate > recalculatePathFrequency);
    }
}
