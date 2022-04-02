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
    public float maxHitPoints;
    public float hitPoints;
    private int suckers;

    public void Awake()
    {
        instance = this;
        hitPoints = maxHitPoints;
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
            // TODO LOSE
        }
    }

    public void Suck()
    {
        suckers++;
    }
    public void DeSuck()
    {
        suckers--;
    }

}
