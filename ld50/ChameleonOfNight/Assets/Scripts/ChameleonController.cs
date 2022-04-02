using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum TongueUpType
{
    Nothing,
    Chain,
    Star
}

public class ChameleonController : MonoBehaviour
{
    private static ChameleonController instance;
    public static ChameleonController Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private Tongue tonguePrefab;
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private int depth;

    public TongueUpType type;
    private float restTimePowerUp;
    private Tongue activeTongue;
    private bool currentlyExtending;

    public void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            type = TongueUpType.Nothing;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPowerUp(TongueUpType.Chain, 10);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPowerUp(TongueUpType.Star, 10);
        }

        if(type != TongueUpType.Nothing)
        {
            restTimePowerUp -= Time.deltaTime;
            if(restTimePowerUp <= 0)
            {
                type = TongueUpType.Nothing;
            }
        }
    }

    public void SetPowerUp(TongueUpType type, float duration = 20)
    {
        this.type = type;
        restTimePowerUp = duration;
    }

    public void Click(Vector3 worldPos)
    {
        if(currentlyExtending) return;
        activeTongue = Instantiate(tonguePrefab);
        activeTongue.ExtendFrom(tongueOrigin.position, worldPos, type, depth, OnExtendCompleted, tonguePrefab);
        currentlyExtending = true;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        if(type == TongueUpType.Nothing) {
            foreach(var enemy in enemies) {
                if(enemy.GetComponent<Enemy>().HeldTongueUp != TongueUpType.Nothing) {
                    SetPowerUp(enemy.GetComponent<Enemy>().HeldTongueUp);
                    break;
                }
            }
        }
        
        currentlyExtending = false;
        Destroy(activeTongue.gameObject);
    }
}
