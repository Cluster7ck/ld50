using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepyBoy : MonoBehaviour
{
    private static SleepyBoy instance;
    public static SleepyBoy Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private float drainSpeed;
    [SerializeField] private float regenSpeed;
    [SerializeField] private Animator sleepy;
    [SerializeField] private Animator twitchy;
    [SerializeField] private Animator awake;

    public float maxHitPoints;
    public float hitPoints;
    private int suckers;
    private Action loseAction;

    public void Awake()
    {
        instance = this;
        hitPoints = maxHitPoints;
    }

    public void SetLoseAction(Action action)
    {
        loseAction =  action;
    }

    public void Reset()
    {
        suckers = 0;
        hitPoints = maxHitPoints;
        AtRest();
    }

    private void Update() 
    {
        if(suckers > 0)
        {
            hitPoints -= drainSpeed * suckers * Time.deltaTime;

        }
        else if(hitPoints < maxHitPoints)
        {
            hitPoints += regenSpeed * Time.deltaTime;
        }
        hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints);

        if(hitPoints <= 0)
        {
            WakeUp();
            loseAction();
        }
    }

    public void Suck()
    {
        suckers++;

        Twitchy();
    }

    public void DeSuck()
    {
        suckers--;
        suckers = Mathf.Clamp(suckers,0, 100);
        
        if(suckers == 0)
        {
            AtRest();
        }
    }
    
    private void AtRest()
    {
        sleepy.gameObject.SetActive(true);
        twitchy.gameObject.SetActive(false);
        awake.gameObject.SetActive(false);
    }

    private void Twitchy()
    {
        sleepy.gameObject.SetActive(false);
        twitchy.gameObject.SetActive(true);
        awake.gameObject.SetActive(false);
    }

    private void WakeUp()
    {
        sleepy.gameObject.SetActive(false);
        twitchy.gameObject.SetActive(false);
        awake.gameObject.SetActive(true);
    }
}
