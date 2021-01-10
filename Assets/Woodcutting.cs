using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutting : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnedLogPrefab;

    float _chopDistance = 6.0f;

    float _secondsBetweenChops = .25f;
    float _chopCooldown;

    Transform _playerTransform;

    int maxHealth = 5;
    int health;

    Vector3 initialScale;

    float nextObjectRespawn;
    float respawnRate = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = player.transform;
        initialScale = transform.localScale;

        SpawnObject();
    }

    void Update()
    {
        if(health == 0 && nextObjectRespawn < Time.time){
            SpawnObject();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Axe" && IsAllowedToChop()) {
            ChopLog();
            _chopCooldown = Time.time + _secondsBetweenChops;
        }
    }

    void ChopLog() {
        GameObject log = Instantiate(spawnedLogPrefab, transform.position + new Vector3(0.5f, 1f, 0.5f), Quaternion.identity);
        HoldableItem script = log.GetComponent<HoldableItem>();
        script.player = player;

        health -= 1;

        transform.localScale = initialScale * ((float)health / maxHealth);

        if(health == 0){
            SetRespawnTimer();
        }
    }

    bool IsAllowedToChop() {
        float distance = (_playerTransform.position - transform.position).magnitude;

        if(distance > _chopDistance) {
            // Too far to chop
            return false;
        }

        if(float.IsNaN(_chopCooldown)) {
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

    void SpawnObject() {
        health = maxHealth;
        transform.localScale = initialScale;
    }

    void SetRespawnTimer() {
        nextObjectRespawn = Time.time + respawnRate;
    }
}
