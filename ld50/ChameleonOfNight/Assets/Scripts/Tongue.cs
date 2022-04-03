using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Tongue : MonoBehaviour
{
    [SerializeField] private LineRenderer tongue;
    [SerializeField] private Transform tongueCollider;
    [SerializeField] private float tongueColliderRadius;
    [SerializeField] private LayerMask insectMask;

    [SerializeField] private float tongueExtendSpeedPowerUp;
    [SerializeField] private float tongueRetreatSpeedPowerUp;

    [SerializeField] private float tongueExtendSpeed;
    [SerializeField] private float tongueRetreatSpeed;
    [SerializeField] private Ease tongueFlickeExtendEase;
    [SerializeField] private Ease tongueFlickeRetreatEase;

    [SerializeField] private GameObject splashParticleSystemPrefab;
    [SerializeField] private GameObject splashAudioSourcePrefab;

    private TweenerCore<Vector3, Vector3, VectorOptions> extendTween;
    private Sequence sequence;
    private Action<List<Enemy>> onExtendCompleted;

    private Tongue tonguePrefab;
    private Vector3 origin;
    private List<Enemy> enemies = new List<Enemy>();
    private int depth;
    private TongueUpType type;
    private int startedExtraExtend = 0;

    void Update()
    {
        if(startedExtraExtend > 0) return;
        if(enemies.Count > 0) return;
        var hitInsects = Physics.OverlapSphere(tongueCollider.transform.position, tongueColliderRadius, insectMask);
        if(hitInsects.Length > 0)
        {
            foreach(var insectCollider in hitInsects)
            {
                var enemy = insectCollider.GetComponent<Enemy>();
                // hit enemy
                if(enemy && !enemy.isHit)
                {
                    enemies.Add(enemy);
                    enemy.Hit();
                    enemy.transform.SetParent(tongueCollider);
                    Instantiate(splashParticleSystemPrefab, tongueCollider.transform.position, Quaternion.identity);
                    AudioSource audioClip = Instantiate(splashAudioSourcePrefab, tongueCollider.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                    audioClip.pitch += depth/10;

                    if(depth > 0)
                    {
                        switch (type) {
                            case TongueUpType.Chain:
                                DoExtendChain();
                                break;
                            case TongueUpType.Star:
                                DoExtendStar();
                                break;
                            default:
                                break;
                        }
                    }
                    else if(depth == 0 && Upgrades.Instance.StarPointsLevel > 0)
                    {
                        DoExtendStar();
                    }
                    else
                    {
                        ReturnToOrigin();
                    }
                    break;
                }
            }
        }
    }

    private void ReturnToOrigin()
    {
        sequence.Kill();
        var retreatTime = CalcAnimTime(origin, tongueCollider.position, GetRetreatSpeed());
        DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), origin, retreatTime)
            .SetEase(tongueFlickeRetreatEase)
            .OnComplete(() => {
                onExtendCompleted(enemies);
                Destroy(gameObject);
            });
    }

    private void DoExtendStar()
    {
        int starPoints = Upgrades.Instance.StarPointsLevel;
        if(starPoints <= 0) return;

        sequence.Kill();
        float step = (Mathf.PI * 2.0f) / starPoints;

        var newDepth = depth - 1;
        var radius = Upgrades.Instance.StarRadius;
        for(int i = 0; i < starPoints; i++)
        {
            var a = step * i;
            var x = radius * Mathf.Cos(a + Mathf.PI/2);
            var y = radius * Mathf.Sin(a + Mathf.PI/2);
            var target = tongueCollider.position + new Vector3(x, y, 0);

            var tongue = Instantiate(tonguePrefab);
            tongue.ExtendFrom(tongueCollider.position, target, TongueUpType.Star, newDepth, OnExtendCompleted, tonguePrefab);
        }
        startedExtraExtend = starPoints;
    }

    private void DoExtendChain() {

        var newDepth = depth - 1;
        var enemiesInRange = Physics.OverlapSphere(tongueCollider.transform.position, Upgrades.Instance.ChainRange, insectMask);
        
        float closestEnemeyDistance = float.MaxValue;
        GameObject closestEnemy = null;

        foreach(Collider checkEnemy in enemiesInRange) {
            if(!checkEnemy.gameObject.GetComponent<Fly>().isHit)
            {
                if(closestEnemy == null) {
                    closestEnemy = checkEnemy.gameObject;
                }
                else
                {
                    float distance = Vector3.Distance(checkEnemy.transform.position, tongueCollider.transform.position);

                    if(distance < closestEnemeyDistance) {
                        closestEnemy = checkEnemy.gameObject;
                        closestEnemeyDistance = distance;
                    }
                }
            }
        }

        if(closestEnemy != null)
        {
            sequence.Kill();

            var tongue = Instantiate(tonguePrefab);
            tongue.ExtendFrom(tongueCollider.position, closestEnemy.transform.position, type, newDepth, OnExtendCompleted, tonguePrefab);
            startedExtraExtend = 1;
        }
        else if(Upgrades.Instance.StarPointsLevel > 0)
        {
            DoExtendStar();
        }
        else
        {
            ReturnToOrigin();
        }
    }

    public void ExtendFrom(Vector3 origin, Vector3 target, TongueUpType type, int depth, Action<List<Enemy>> onExtendCompleted, Tongue tonguePrefab)
    {
        ExtendFrom(origin, () => target, type, depth, onExtendCompleted, tonguePrefab);
    }

    public void ExtendFrom(Vector3 origin, Func<Vector3> target, TongueUpType type, int depth, Action<List<Enemy>> onExtendCompleted, Tongue tonguePrefab)
    {
        this.tonguePrefab = tonguePrefab;
        this.origin = origin;
        this.depth = depth;
        this.type = type;
        this.onExtendCompleted = onExtendCompleted;
        tongue.SetPositions(new Vector3[]{origin, origin});
        
        sequence = DOTween.Sequence();

        var extendTime = CalcAnimTime(origin, target(), GetExtendSpeed());
        var retreatTime = CalcAnimTime(origin, target(), GetRetreatSpeed());
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), target(), extendTime).SetEase(tongueFlickeExtendEase));
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), origin, retreatTime).SetEase(tongueFlickeRetreatEase));
        sequence.OnComplete(() => {
            onExtendCompleted(enemies);
            Destroy(gameObject);
        });
        sequence.Play();
    }

    private float GetExtendSpeed()
    {
        return tongueExtendSpeed + Upgrades.Instance.ExtraSpeed;
    }

    private float GetRetreatSpeed()
    {
        return tongueRetreatSpeed + Upgrades.Instance.ExtraSpeed;
    }

    private float CalcAnimTime(Vector3 origin, Vector3 target, float speed)
    {
        var dist = Vector3.Distance(origin, target);
        var time = dist/speed;
        Debug.Log("AnimTime: "+time);
        var remapped = time.Remap(0,0.25f,0.08f,0.25f);
        return remapped;
    }

    private void SetTongueTip(Vector3 target)
    {
        tongue.SetPosition(1, target);
        tongueCollider.position = target;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        startedExtraExtend -= 1;
        foreach(var enemy in enemies)
        {
            enemy.transform.SetParent(tongueCollider);
            this.enemies.Add(enemy);
        }

        if(startedExtraExtend == 0)
        {
            // RÜCKWEG
            ReturnToOrigin();
        }
    }
}
