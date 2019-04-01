using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody ball;
    public Transform cam;
    public ParticleSystem particles;
    public float forceMultiplier = 500f;
    public float jumpForceMultiplier = 30000f;

    public Vector3 startPos;
    public Vector3 initialVelocity;

    // Start is called before the first frame update
    void Start()
    {
        ball.position = startPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Death
        if (ball.position.y < -5)
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
        if (System.Math.Abs(ball.velocity.y) < 0.001 && Input.GetKey("space"))
        {
            ball.AddForce(0, jumpForceMultiplier * Time.deltaTime, 0);
        }
    }

    private void killPlayer() {
        ball.position = startPos;
        ball.velocity = initialVelocity;
        ball.angularVelocity = initialVelocity;
        particles.Stop();
    }

    private Vector3 Rotate90(Vector3 aDir)
     {
         return new Vector3(aDir.z, 0, -aDir.x);
     }
}
