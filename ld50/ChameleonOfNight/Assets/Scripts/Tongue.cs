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

    [SerializeField] private int starMaxDepth;
    [SerializeField] private float starRadius;
    [SerializeField] private int starPointyNum;

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

                    if(startedExtraExtend == 0 && type != TongueUpType.Nothing && depth > 0)
                    {
                        sequence.Kill();

                        // chain
                        // overlap sphere
                        // take the first one that is not in hitInsects or myself OR enemies
                        // then extend to that insect

                        if(type == TongueUpType.Star)
                        {
                            DoExtendStar();
                        }
                        else
                        {
                            //ExtendFrom(tongueCollider.transform.position, Vector3.left);
                            var tongue = Instantiate(tonguePrefab);

                            var newDepth = depth - 1;
                            tongue.ExtendFrom(tongueCollider.position, tongueCollider.position + Vector3.up * 1f, type, newDepth, OnExtendCompleted, tonguePrefab);
                            startedExtraExtend = 1;
                        }
                    }
                    else
                    {
                        //
                    }
                    break;
                }
            }
        }
    }

    private void DoExtendStar()
    {
        float step = (Mathf.PI * 2.0f) / starPointyNum;

        var newDepth = depth - 1;
        for(int i = 0; i < starPointyNum; i++)
        {
            var a = step * i;
            var x = starRadius/(starMaxDepth - depth) * Mathf.Cos(a);
            var y = starRadius/(starMaxDepth - depth) * Mathf.Sin(a);
            var target = tongueCollider.position + new Vector3(x, y, 0);

            var tongue = Instantiate(tonguePrefab);
            tongue.ExtendFrom(tongueCollider.position, target, TongueUpType.Star, newDepth, OnExtendCompleted, tonguePrefab);
        }
        startedExtraExtend = starPointyNum;
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
        startedExtraExtend -= 1;
        foreach(var enemy in enemies)
        {
            enemy.transform.SetParent(tongueCollider);
            this.enemies.Add(enemy);
        }

        if(startedExtraExtend == 0)
        {
            // RÜCKWEG
            var retreatTime = CalcAnimTime(tongueCollider.position, origin, tongueRetreatSpeed);
            DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), origin, retreatTime).SetEase(tongueFlickeRetreatEase).OnComplete(() => {
                onExtendCompleted(enemies);
                Destroy(gameObject);
            });
        }
    }
}
