using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTheGame : MonoBehaviour
{
    [SerializeField] private FlySpawner spawner;
    [SerializeField] private StartKill startKill;
    [SerializeField] private SleepyBoy sleepyBoy;
    [SerializeField] private GameObject levelXpUi;
    [SerializeField] private Credits credits;
    [SerializeField] private Button exitButton;

    public void Awake()
    {
        startKill.SetStartAction(StartIt);
        sleepyBoy.SetLoseAction(Reset);

        exitButton.onClick.AddListener(() => Application.Quit());
    }

    public void Reset()
    {
        // TODO SHOW YOU LOST OR SOMETHING

        spawner.Reset();
        Highscore.Instance.Reset();
        Upgrades.Instance.Reset();
        levelXpUi.SetActive(false);

        startKill.gameObject.SetActive(true);
        credits.Appear();
    }

    void StartIt()
    {
        spawner.StartSpawning();
        levelXpUi.SetActive(true);
        credits.Disappear();
        sleepyBoy.Reset();
    }
}
