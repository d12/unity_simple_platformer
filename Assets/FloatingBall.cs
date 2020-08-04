using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ArchimedsLab; //Include the DLL provided

public class FloatingBall : MonoBehaviour
{
    /* These 4 arrays are cache array, preventing some operations to be done each frame. */
    tri[] _triangles;
    tri[] worldBuffer;
    tri[] wetTris;
    tri[] dryTris;

    //These two variables will store the number of valid triangles in each cache arrays. They are different from array.Length !
    uint nbrWet, nbrDry;
    Rigidbody rb; //Get the rigidbody
    Mesh m; //Mesh used for the simulation

    public static float waterHeight = -2f;

    WaterSurface.GetWaterHeight myWaterHeight = delegate(Vector3 a) {
        return waterHeight;
    };

    void Awake() {
        rb = GetComponent<Rigidbody>();
        m = GetComponent<MeshFilter>().mesh;
        //Setting up the cache for the game. Here we use variables with a game-long lifetime
        WaterCutter.CookCache(m, ref _triangles, ref worldBuffer, ref wetTris, ref dryTris);
    }

    void FixedUpdate(){
        /* It's strongly advised to call these in the FixedUpdate function to prevent some weird behaviors */
        //This will prepare static cache, modifying vertices using rotation and position offset.
        WaterCutter.CookMesh(transform.position, transform.rotation, ref _triangles, ref worldBuffer);
                /*
                    Now mesh ae reprensented in World position, we can split the mesh, and split tris
        that are partially submerged.
        Here I use a very simple water model, already implemented in the DLL. You can give your own. See the example in Examples/CustomWater.
        */
        WaterCutter.SplitMesh(worldBuffer, ref wetTris, ref dryTris, out nbrWet, out nbrDry, myWaterHeight);
        //This function will compute the forces depending on the triangles generated before.
        Archimeds.ComputeAllForces(wetTris, dryTris, nbrWet, nbrDry, rb.velocity, rb);
    }
}
