using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{ 
    public Vector3 GetSpawnPosition () {
        var dir2d = Random.insideUnitCircle * 0.5f;
        var spawnPosition = new Vector3(dir2d.x, dir2d.y, 0) * transform.localScale.x + transform.position;
        return spawnPosition;
    }
}