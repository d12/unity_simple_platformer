using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public ParticleSystem particles;

    public void OnTriggerEnter(Collider collidingObject)
    {
        Debug.Log("WIN!");
        particles.Play();
    }
}
