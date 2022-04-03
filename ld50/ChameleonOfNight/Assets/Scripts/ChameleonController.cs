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
        activeTongue.ExtendFrom(tongueOrigin.position, worldPos, TongueUpType.Chain, Upgrades.Instance.ChainLevel, OnExtendCompleted, tonguePrefab);
        currentlyExtending = true;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        currentlyExtending = false;
        Destroy(activeTongue.gameObject);
    }
}
