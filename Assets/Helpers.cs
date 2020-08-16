using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public static Helpers instance;
    public GameObject player;

    // Start is called before the first frame update
    // void Start()
    // {
    //
    // }

    void Awake() {
        if(instance != null) {
            GameObject.Destroy(instance);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public GameObject getPlayer() {
        return player;
    }

    public PlayerState getPlayerState() {
        return player.GetComponent<PlayerState>();
    }
}
