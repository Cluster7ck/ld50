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

    [SerializeField] private float tongueExtendSpeed;
    [SerializeField] private float tongueRetreatSpeed;
    [SerializeField] private Ease tongueFlickeExtendEase;
    [SerializeField] private Ease tongueFlickeRetreatEase;

    private TweenerCore<Vector3, Vector3, VectorOptions> extendTween;
    private Sequence sequence;
    private Action<List<Enemy>> onExtendCompleted;

    private Tongue tonguePrefab;
    private Vector3 origin;
    private List<Enemy> enemies = new List<Enemy>();
    private int depth;
    private TongueUpType type;
    private bool startedExtraExtend;

    [Header("ChainTongue")]
    [SerializeField] private float chainRange;


    void Update()
    {
        if(startedExtraExtend) return;
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

                    if(!startedExtraExtend && type != TongueUpType.Nothing && depth > 0)
                    {
                        sequence.Kill();
                        //ExtendFrom(tongueCollider.transform.position, Vector3.left);
                        switch (type) {
                            case TongueUpType.Chain:
                                var tongue = Instantiate(tonguePrefab);

                                var newDepth = depth - 1;
                                Debug.Log("NewDepth: " + newDepth);
                                var enemiesInRange = Physics.OverlapSphere(tongueCollider.transform.position, chainRange, insectMask);
                                Debug.Log("EnemiesInRange: " + enemiesInRange.Length);
                                if(enemiesInRange.Length > 0) {
                                    GameObject closestEnemy = null;
                                    foreach(Collider checkEnemy in enemiesInRange) {
                                        if(!checkEnemy.gameObject.GetComponent<Enemy>().isHit) {
                                            if(closestEnemy == null) {
                                                closestEnemy = checkEnemy.gameObject;
                                            } else { 
                                                if(Vector3.Distance(checkEnemy.transform.position, tongueCollider.transform.position) < Vector3.Distance(closestEnemy.transform.position, tongueCollider.transform.position)) {
                                                    closestEnemy = checkEnemy.gameObject;
                                                    Debug.Log("New closest enemy");
                                                }
                                            }
                                        }
                                    }
                                    if(closestEnemy != null) {
                                        Debug.Log("Closest enemy: " + closestEnemy);
                                        tongue.ExtendFrom(tongueCollider.position, closestEnemy.transform.position, type, newDepth, OnExtendCompleted, tonguePrefab);
                                        startedExtraExtend = true;
                                    }
                                }
                                break;

                            case TongueUpType.Star:

                                break;
                            default:

                                break;
                        }

                        // chain
                        // overlap sphere
                        // take the first one that is not in hitInsects or myself OR enemies
                        // then extend to that insect
                    }
                    else
                    {
                        //
                    }
                }
            }
        }
    }

    public void ExtendFrom(Vector3 origin, Vector3 target, TongueUpType type, int depth, Action<List<Enemy>> onExtendCompleted, Tongue tonguePrefab)
    {
        ExtendFrom(origin, () => target, type, depth, onExtendCompleted, tonguePrefab);
    }

    public void ExtendFrom(Vector3 origin, Func<Vector3> target, TongueUpType type, int depth, Action<List<Enemy>> onExtendCompleted, Tongue tonguePrefab)
    {
        //Debug.Log($"New extend from: {origin} to: {target()}");
        this.tonguePrefab = tonguePrefab;
        this.origin = origin;
        this.depth = depth;
        this.type = type;
        this.onExtendCompleted = onExtendCompleted;
        tongue.SetPositions(new Vector3[]{origin, origin});
        
        sequence = DOTween.Sequence();

        var extendTime = CalcAnimTime(origin, target(), tongueExtendSpeed);
        var retreatTime = CalcAnimTime(origin, target(), tongueRetreatSpeed);
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), target(), extendTime).SetEase(tongueFlickeExtendEase));
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), origin, retreatTime).SetEase(tongueFlickeRetreatEase));
        sequence.OnComplete(() => {
            onExtendCompleted(enemies);
            Destroy(gameObject);
        });
        sequence.Play();
    }

    private float CalcAnimTime(Vector3 origin, Vector3 target, float speed)
    {
        var dist = Vector3.Distance(origin, target);
        return dist/speed;
    }

    private void SetTongueTip(Vector3 target)
    {
        tongue.SetPosition(1, target);
        tongueCollider.position = target;
    }

    private void OnExtendCompleted(List<Enemy> enemies)
    {
        foreach(var enemy in enemies)
        {
            enemy.transform.SetParent(tongueCollider);
            this.enemies.Add(enemy);
        }

        // RÜCKWEG

        var retreatTime = CalcAnimTime(tongueCollider.position, origin, tongueRetreatSpeed);
        DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), origin, retreatTime).SetEase(tongueFlickeRetreatEase).OnComplete(() => {
            onExtendCompleted(enemies);
            Destroy(gameObject);
        });

    }
}
