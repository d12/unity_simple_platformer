﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject ballObject;
    public Rigidbody ball;
    public Transform cam;
    public ParticleSystem particles;
    public float forceMultiplier = 500f;
    public float jumpForceMultiplier = 30000f;
    public float maximumVelocity = 10f;
    public float jumpHeaviness = 1f;

    public Vector3 startPos;
    public Vector3 initialVelocity;
    public SphereCollider ballCollider;

    private bool jumpOnLastFrame = false;
    private float ballRadius;

    // Start is called before the first frame update
    void Start()
    {
        ball.position = startPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Death
        if (ball.position.y < -3)
        {
            killPlayer();
            return;
        }

        Vector3 forwardVector = cam.forward;

        // Looking up or down shouldn't make us jump higher or lower,
        // so zero out the y vector component
        forwardVector.y = 0;

        // Then normalize the vector (convert to a unit vector) since zero'ing
        // the y component can change the magnitude.
        forwardVector = Vector3.Normalize(forwardVector);

        // The vector perpendicular to the forward vector used when moving left and right
        Vector3 perpendicularVector = Rotate90(forwardVector);

        bool onGround = isOnGround();

        // Left
        if (Input.GetKey("a"))
        {
            ball.AddForce(-forceMultiplier * perpendicularVector * Time.deltaTime);
        }

        // Right
        if (Input.GetKey("d"))
        {
            ball.AddForce(forceMultiplier * perpendicularVector * Time.deltaTime);
        }

        // Down
        if (Input.GetKey("s"))
        {
            ball.AddForce(-forceMultiplier * forwardVector * Time.deltaTime);
        }

        // Up
        if (Input.GetKey("w"))
        {
            ball.AddForce(forceMultiplier * forwardVector * Time.deltaTime);
        }

        // Space
        if (Input.GetKey("space"))
        {
            if (onGround && !jumpOnLastFrame) // For some reason, Epsilon breaks this part sometimes.
            {
                Vector3 velocity = ball.velocity;
                velocity.y = 0;
                ball.velocity = velocity;

                ball.AddForce(0, jumpForceMultiplier * Time.deltaTime, 0);
            }

            jumpOnLastFrame = true;
        } else {
            jumpOnLastFrame = false;
        }

        if (!onGround) {
            ball.AddForce(0, -1 * jumpHeaviness * Time.deltaTime, 0);
        }

        ApplyVelocityCeiling();
    }

    private void killPlayer() {
        ball.position = startPos;
        ball.velocity = initialVelocity;
        ball.angularVelocity = initialVelocity;
        particles.Stop();
    }

    private Vector3 Rotate90(Vector3 aDir) {
         return new Vector3(aDir.z, 0, -aDir.x);
    }

    // Apply a cap to the horiztontal components of the velocity vector (ignores y axis)
    private void ApplyVelocityCeiling()
    {
        Vector3 horiztonalVelocityVector = ball.velocity;
        horiztonalVelocityVector.y = 0;

        float horiztonalVelocityMagnitude = horiztonalVelocityVector.magnitude;

        if (horiztonalVelocityMagnitude > maximumVelocity)
        {
            // If we're > the allowed maximum velocity, we need to reassign the velocity vector
            // to be within the allowable x/z magnitude without changing the
            // x/z magnitude ratio.
            //
            // Do this by saving the y magnitude, normalizing the current velocity
            // vector with y zeroed out, multiply by the allowed x/z velocity magnitude,
            // then reassign the previous y magnitude.

            float oldYVelocityMagnitude = ball.velocity.y;
            Vector3 scaledHorizontalVelocityVector = horiztonalVelocityVector.normalized * maximumVelocity;
            Vector3 newVelocity = Vector3.zero; // I don't know how to initialize a new Vector3, and I'm on a plane so i can't google it. -_-
            newVelocity.Set(scaledHorizontalVelocityVector.x, oldYVelocityMagnitude, scaledHorizontalVelocityVector.z);

            ball.velocity = newVelocity;
        }
    }

    private bool isOnGround()
    {
        // On a slope the bottom of the ball may not be touching the floor.
        // So we give some buffer for how far above the ground an object can be.
        float leniency = 0.2f;

        if (Physics.Raycast(bottomOfBall(), Vector3.down, leniency)){
            return true;
        } else {
            return false;
        }
    }

    private Vector3 bottomOfBall()
    {
        // Raise the bottom of the ball by a small amount otherwise the raycast goes right through the floor.
        return ball.position - (new Vector3(0, ballRadius - 0.01f, 0));
    }
}
