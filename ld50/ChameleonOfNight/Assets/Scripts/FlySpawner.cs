using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;
    
        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);
    
        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
    
        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }

    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private float _enemySpawnRate;
    [SerializeField] private float _chanceToCarryTongueUp;
    [SerializeField] private float spawnRateSlowdowner;
    [SerializeField] private List<SpawnZone> spawnZones;

    private float nextEnemyTime;
    private float _timeSinceLastSpawn;
    private int _enemiesSpawned;


    // Start is called before the first frame update
    void Start()
    {
        Highscore.Instance.OnHighscore.AddListener(OnHighscore);
        nextEnemyTime = _enemySpawnRate;
    }


    public void OnHighscore(int highScore)
    {
        // remap highscore to higher spawn rate and stuff
    }

    // Update is called once per frame
    void Update()
    {
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
        var spawnRate = 1.0f / (Mathf.Clamp(Mathf.RoundToInt(Time.timeSinceLevelLoad/60), 1, int.MaxValue));
        nextEnemyTime = Helper.RandomGaussian(spawnRate * 0.8f, spawnRate * 1.2f);
    }

    private Vector3 SpawnPosition() {
        Vector3 pos;
        pos = spawnZones[Random.Range(0, spawnZones.Count)].GetSpawnPosition();
        return pos;
    }
}
