using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private GameObject _flyTarget;
    [SerializeField] private float _flightTime = 10;
    [SerializeField] private int _flightDirectionChanges = 10;

    public void SetTarget (GameObject target) {
        _flyTarget = target;

        transform.DOPath(flightPath(), _flightTime);
    }

    private Vector3[] flightPath() {
        Vector3[] _path = new Vector3[_flightDirectionChanges];
        float distanceToTarget = (transform.position - _flyTarget.transform.position).magnitude;
        _path[0] = transform.position;
        for(int i = 1; i < _flightDirectionChanges; i++) {
            Vector3 targetDirection = _path[i-1] - _flyTarget.transform.position;
            Vector3 newWaypoint = targetDirection * (distanceToTarget/(_flightDirectionChanges*i)); // waypoint calculated, so that distance covered is relative to distance to target left
            newWaypoint = new Vector3(newWaypoint.x, 0.1f, newWaypoint.z);
            _path[i] = newWaypoint;
        }
        return _path;
    }
}
