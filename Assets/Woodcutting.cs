using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutting : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnedLogPrefab;

    float _chopDistance = 5.0f;

    float _secondsBetweenChops = 1.0f;
    float _chopCooldown;

    Transform _playerTransform;



    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = player.transform;
    }

    // Update is called once per frame
    // void Update()
    // {
    //
    // }

    void OnMouseDown() {
        if(isAllowedToChop()) {
            SpawnLog();
            _chopCooldown = Time.time + _secondsBetweenChops;
        }
    }

    void SpawnLog() {
        GameObject log = Instantiate(spawnedLogPrefab, transform.position + new Vector3(0.5f, 1f, 0.5f), Quaternion.identity);
        HoldableItem script = log.GetComponent<HoldableItem>();
        script.player = player;
    }

    bool isAllowedToChop() {
        float distance = (_playerTransform.position - transform.position).magnitude;

        if(distance > _chopDistance) {
            // Too far to chop
            return false;
        }

        if(_chopCooldown == null) {
            // First chop so no cooldown exists yet, we're good
            return true;
        }

        if(Time.time > _chopCooldown) {
            // The cooldown period is in the past, all good
            return true;
        } else {
            // The cooldown period is in the future, must wait!
            return false;
        }
    }
}
