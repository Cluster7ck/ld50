using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private float _enemySpawnRate;
    [SerializeField] private float _chanceToCarryTongueUp;
    [SerializeField] private List<SpawnZone> spawnZones;

    private float nextEnemyTime;
    private float _timeSinceLastSpawn;
    private int _enemiesSpawned;

    public bool shouldSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Highscore.Instance.OnHighscore.AddListener(OnHighscore);
        nextEnemyTime = GetSpawnRate();
    }


    public void OnHighscore(int highScore)
    {
        // remap highscore to higher spawn rate and stuff
    }

    // Update is called once per frame
    void Update()
    {
        if(!shouldSpawn) return;
        if(_timeSinceLastSpawn >= nextEnemyTime) {
            SpawnEnemy();
        }
        _timeSinceLastSpawn += Time.deltaTime;
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
        nextEnemyTime = GetSpawnRate();
    }

    private float GetSpawnRate()
    {
        var spawnRate = 1.0f / (Mathf.Clamp(Mathf.RoundToInt(Time.timeSinceLevelLoad/60), 1, int.MaxValue));
        return Helper.RandomGaussian(spawnRate * 0.8f, spawnRate * 1.2f) * _enemySpawnRate;
    }

    private Vector3 SpawnPosition() {
        Vector3 pos;
        pos = spawnZones[Random.Range(0, spawnZones.Count)].GetSpawnPosition();
        return pos;
    }
}
