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
    [SerializeField] private Tongue tonguePrefab;
    [SerializeField] private Transform tongueOrigin;
    
    private Tongue activeTongue;
    private TongueUpType activeTongueUpType;
    private bool currentlyExtending;

    public void Click(Vector3 worldPos)
    {
        if(currentlyExtending) return;
        activeTongue = Instantiate(tonguePrefab);
        activeTongue.ExtendFrom(tongueOrigin.position, worldPos, activeTongueUpType, 5, OnExtendCompleted, tonguePrefab);
        currentlyExtending = true;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        if(activeTongueUpType == TongueUpType.Nothing) {
            foreach(var enemy in enemies) {
                if(enemy.GetComponent<Enemy>().HeldTongueUp != TongueUpType.Nothing) {
                    activeTongueUpType = enemy.GetComponent<Enemy>().HeldTongueUp;
                }
            }
        }
        
        currentlyExtending = false;
        Destroy(activeTongue.gameObject);
    }
}
