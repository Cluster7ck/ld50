using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{ 
    public Vector3 GetSpawnPosition () {
        Vector3 spawnPosition = new Vector3(
            Random.insideUnitSphere.x * transform.localScale.x + transform.position.x, 
            Random.insideUnitSphere.y * transform.localScale.y + transform.position.y, 
            Random.insideUnitSphere.z * transform.localScale.z + transform.position.z);
        Debug.Log("Spawning new Enemy at: " + spawnPosition);
        return spawnPosition;
    }
}