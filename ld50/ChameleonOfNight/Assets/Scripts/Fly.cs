using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Fly : Enemy
{
    private Vector3 _flyTarget;
    [SerializeField] private float _flightTime = 10;
    [SerializeField] private int _flightDirectionChanges = 10;
    [SerializeField] private float pullToTarget = 1;
    [SerializeField] private float step;

    bool reached = false;
    bool sucking = true;
    Vector3 lastPos;
    TweenerCore<Vector3, Vector3, VectorOptions> currentMover;

    //TongueUp
    private TongueUpType _heldTongueUp;
    private FlySpawner flySpawner;

    public void Init (GameObject target, FlySpawner flySpawner) {
        _flyTarget = target.transform.position;
        lastPos = transform.position;
    }

    public override void Hit()
    {
        if(isHit) return;
        // TODO DEATH ANIMATION

        if(sucking)
            SleepyBoy.Instance.DeSuck();

        OnEnemyKill.Invoke();
        isHit = true;
    }

    public override void RealDestroy()
    {
        flySpawner.liveEnemies.Remove(this);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(reached) return;

        if(Vector3.Distance(transform.position, lastPos) <= Mathf.Epsilon )
        {
            if(Random.value < pullToTarget )
            {
                var dir = (_flyTarget - lastPos).normalized * step;
                var newTarget = lastPos + dir;
                lastPos = newTarget;
            }
            else
            {
                var dir2d = Random.insideUnitCircle;
                var dir = new Vector3(dir2d.x, dir2d.y, 0) * step;

                var newTarget = lastPos + dir;
                lastPos = newTarget;
            }

            if(Vector3.Distance(_flyTarget, lastPos) <= step )
            {
                var dir = (_flyTarget - lastPos).normalized * step;
                var newTarget = lastPos + dir;
                lastPos = newTarget;
                reached = true;

                currentMover?.Kill();
                currentMover = transform.DOMove(lastPos, step/_flightTime).OnComplete(() => {
                    SleepyBoy.Instance.Suck();
                });
            }
            else{
                currentMover?.Kill();
                currentMover = transform.DOMove(lastPos, step/_flightTime);
            }
        }
    }

    private Vector3[] JitterPath(Vector3 headTarget)
    {
        var _path = new List<Vector3>();

        Vector3 lastPos = transform.position;
        bool reached = false;
        while(!reached && _path.Count < 200)
        {
            if(Random.value < pullToTarget )
            {
                var dir = (headTarget - lastPos).normalized * step;
                var newTarget = lastPos + dir;
                _path.Add(newTarget);
                lastPos = newTarget;
            }
            else
            {
                var dir2d = Random.insideUnitCircle;
                var dir = new Vector3(dir2d.x, dir2d.y, 0) * step;

                var newTarget = lastPos + dir;
                _path.Add(newTarget);
                lastPos = newTarget;
            }

            if(Vector3.Distance(headTarget, lastPos) <= step )
            {
                var dir = (headTarget - lastPos).normalized * step;
                var newTarget = lastPos + dir;
                _path.Add(newTarget);
                lastPos = newTarget;
                reached = true;
            }
        }
        Debug.Log(_path.Count);
        return _path.ToArray();
    }

    public TongueUpType HeldTongueUp {
        get { return _heldTongueUp; }

    }

    public void SetTongueUp(TongueUpType type) {
        _heldTongueUp = type;
        switch (_heldTongueUp) {
            case TongueUpType.Nothing:
                break;
            case TongueUpType.Chain:
                transform.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
                break;
            case TongueUpType.Star:
                transform.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 1, 0);
                break;
        }
    }
}
