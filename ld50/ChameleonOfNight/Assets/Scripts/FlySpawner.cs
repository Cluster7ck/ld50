using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> normalEnemyPrefabs;
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private GameObject illuminatedEnemy;
    [SerializeField] private float illuminatedEnemyBaseSpawnChance;
    [SerializeField] private float illuminatedEnemySpawnChanceIncrease;

    [SerializeField] private GameObject enemyTarget;
    [SerializeField] private float chanceToCarryExtra;
    [SerializeField] private List<SpawnZone> spawnZones;

    public List<Fly> liveEnemies = new List<Fly>();

    private float spawnerTime;
    private float nextEnemyTime;
    private float _timeSinceLastSpawn;
    private int _enemiesSpawned;

    public bool shouldSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Highscore.Instance.OnHighscore.AddListener(OnHighscore);
    }

    public void StartSpawning()
    {
        shouldSpawn = true;
        spawnerTime = 0;
        nextEnemyTime = GetSpawnRate();
    }

    public void Reset()
    {
        shouldSpawn = false;
        for(int i = liveEnemies.Count - 1; i >= 0; i--)
        {
            var e = liveEnemies[i];
            Destroy(e.gameObject);
        }
        liveEnemies.Clear();
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
        spawnerTime += Time.deltaTime;
    }

    private void SpawnEnemy() {
        GameObject newEnemy;
        if(Random.Range(0f, 1f) < illuminatedEnemyBaseSpawnChance) {
            newEnemy = Instantiate(illuminatedEnemy, SpawnPosition(), Quaternion.identity);
        } else {
            int rng = Random.Range(0, normalEnemyPrefabs.Count);
            newEnemy = Instantiate(normalEnemyPrefabs[rng], SpawnPosition(), Quaternion.identity);
        }
        Fly enemy = newEnemy.GetComponent<Fly>();
        enemy.Init(enemyTarget, this);

        liveEnemies.Add(enemy);
        
        if(Random.Range(0f, 1f) < chanceToCarryExtra) {
            Debug.Log("Extra should be spawned");
        }
        _enemiesSpawned++;
        _timeSinceLastSpawn = 0;
        nextEnemyTime = GetSpawnRate();
    }

    private float GetSpawnRate()
    {
        var spawnRate = 1.0f / (Mathf.Clamp(Mathf.RoundToInt(Time.timeSinceLevelLoad/60), 1, int.MaxValue));
        return Helper.RandomGaussian(spawnRate * 0.8f, spawnRate * 1.2f) * enemySpawnRate;
    }

    private Vector3 SpawnPosition() {
        Vector3 pos;
        pos = spawnZones[Random.Range(0, spawnZones.Count)].GetSpawnPosition();
        return pos;
    }
}
