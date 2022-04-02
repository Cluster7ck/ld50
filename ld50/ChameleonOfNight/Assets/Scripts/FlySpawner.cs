using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int _enemyPotential;
    [SerializeField] private int _enemySpawnRate;

    private double _timeSinceLastSpawn;
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
        Instantiate(enemyPrefab);
        _enemiesSpawned++;
        _timeSinceLastSpawn = 0;
    }
}
