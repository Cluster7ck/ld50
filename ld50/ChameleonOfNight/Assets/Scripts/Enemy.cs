using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Enemy : MonoBehaviour
{
    private Vector3 _flyTarget;
    [SerializeField] private float _flightTime = 10;
    [SerializeField] private int _flightDirectionChanges = 10;
    [SerializeField] private float pullToTarget = 1;
    [SerializeField] private float step;
    bool reached = false;
    Vector3 lastPos;
    TweenerCore<Vector3, Vector3, VectorOptions> currentMover;


    public void SetTarget (GameObject target) {
        _flyTarget = target.transform.position;
        lastPos = transform.position;

        //transform.DOPath(JitterPath(target.transform.position), _flightTime);
    }

    public void Hit()
    {
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
            }
            currentMover?.Kill();
            currentMover = transform.DOMove(lastPos, step/_flightTime);
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

    //private Vector3[] flightPath() {
    //    Vector3[] _path = new Vector3[_flightDirectionChanges];
    //    float distanceToTarget = (transform.position - _flyTarget.transform.position).magnitude;
    //    _path[0] = transform.position;
    //    for(int i = 1; i < _flightDirectionChanges; i++) {
    //        Vector3 targetDirection = _path[i-1] - _flyTarget.transform.position;
    //        Vector3 newWaypoint = targetDirection * (distanceToTarget/(_flightDirectionChanges*i)); // waypoint calculated, so that distance covered is relative to distance to target left
    //        _path[i] = newWaypoint;
    //    }
    //    return _path;
    //}
}
