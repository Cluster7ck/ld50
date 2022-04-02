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
    private bool currentlyExtending;

    public void Click(Vector3 worldPos)
    {
        if(currentlyExtending) return;
        activeTongue = Instantiate(tonguePrefab);
        activeTongue.ExtendFrom(tongueOrigin.position, worldPos, TongueUpType.Star, 4, OnExtendCompleted, tonguePrefab);
        currentlyExtending = true;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        //foreach(var enemy in enemies)
        //{
        //    enemy.RealDestroy();
        //}
        currentlyExtending = false;
        Destroy(activeTongue.gameObject);
    }
}
