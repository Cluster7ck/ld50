using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTheGame : MonoBehaviour
{
    [SerializeField] private FlySpawner spawner;
    [SerializeField] private StartKill startKill;
    [SerializeField] private SleepyBoy sleepyBoy;
    [SerializeField] private GameObject levelXpUi;

    public void Awake()
    {
        startKill.SetStartAction(StartIt);
        sleepyBoy.SetLoseAction(Reset);
    }

    public void Reset()
    {
        // TODO SHOW YOU LOST OR SOMETHING

        spawner.Reset();
        Highscore.Instance.Reset();
        Upgrades.Instance.Reset();
        sleepyBoy.Reset();
        levelXpUi.SetActive(false);

        startKill.gameObject.SetActive(true);
    }

    void StartIt()
    {
        spawner.StartSpawning();
        levelXpUi.SetActive(true);
    }
}
