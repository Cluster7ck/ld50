using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private int _enemyPotential;
    [SerializeField] private float _enemySpawnRate;
    [SerializeField] private float _chanceToCarryTongueUp;
    [SerializeField] private List<SpawnZone> spawnZones;

    private float _timeSinceLastSpawn;
    private int _enemiesSpawned;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemiesSpawned < _enemyPotential) {
            if(_timeSinceLastSpawn >= _enemySpawnRate) {
                SpawnEnemy();
            }
            _timeSinceLastSpawn += Time.deltaTime;
        }          
    }

    private void SpawnEnemy() {
        GameObject newEnemy = Instantiate(enemyPrefab, SpawnPosition(), Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.SetTarget(enemyTarget);
        
        if(Random.Range(0f, 1f) < _chanceToCarryTongueUp) {
            enemy.SetTongueUp(TongueUpType.Chain);
        } else {
            enemy.SetTongueUp(TongueUpType.Nothing);
        }
        _enemiesSpawned++;
        _timeSinceLastSpawn = 0;
    }

    private Vector3 SpawnPosition() {
        Vector3 pos;
        pos = spawnZones[Random.Range(0, spawnZones.Count)].GetSpawnPosition();
        return pos;
    }
}
