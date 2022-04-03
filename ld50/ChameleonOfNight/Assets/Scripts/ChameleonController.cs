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

    [SerializeField] private Transform jawHinge;
    [SerializeField] private Transform headHinge;
    [SerializeField] private float headSpeed;
    [SerializeField] private float headAngle;
    [SerializeField] private float jawAngle;
    [SerializeField] private float jawSpeed;
    [SerializeField] private Ease headUpEase;
    [SerializeField] private Ease jawDownEase;

    [SerializeField] private Transform body;
    [SerializeField] private float bodyBreadthSpeed;
    [SerializeField] private float bodyBreadthHeight;
    [SerializeField] private Ease breadthInEase;
    [SerializeField] private Ease breadthOutEase;

    private Tongue activeTongue;
    private bool currentlyExtending;

    private void Start()
    {
        var seq = DOTween.Sequence();
        seq.Append(body.DOMoveY(body.transform.position.y + bodyBreadthHeight, bodyBreadthSpeed)
            .SetEase(breadthInEase));
        seq.Append(body.DOMoveY(body.transform.position.y, bodyBreadthSpeed * 2)
            .SetEase(breadthOutEase));
        seq.SetLoops(-1);
        seq.Play();
    }

    public void Click(Vector3 worldPos)
    {
        if(currentlyExtending) return;
        activeTongue = Instantiate(tonguePrefab);
        activeTongue.ExtendFrom(tongueOrigin.position, worldPos, TongueUpType.Chain, Upgrades.Instance.ChainLevel, OnExtendCompleted, tonguePrefab);
        DoHeadAnimation();
        currentlyExtending = true;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        currentlyExtending = false;
        jawHinge.DORotate(Vector3.zero, jawSpeed).SetEase(jawDownEase);
        headHinge.DORotate(Vector3.zero, headSpeed).SetEase(jawDownEase);
        Destroy(activeTongue.gameObject);
    }

    private void DoHeadAnimation()
    {
        jawHinge.DORotate(Vector3.forward * jawAngle, jawSpeed).SetEase(jawDownEase);
        headHinge.DORotate(Vector3.forward * headAngle, headSpeed).SetEase(jawDownEase);
    }
}
