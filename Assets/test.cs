using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    public Transform part;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void OnMouseDown(){
        Debug.Log("Mouse Down!");
        part.position = part.position + new Vector3(1, 0, 0);
    }

    public void OnMouseUp(){
        Debug.Log("Mouse Up!");
    }
}
