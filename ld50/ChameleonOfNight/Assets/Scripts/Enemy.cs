using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private GameObject _flyTarget;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetTarget (GameObject target) {
        _flyTarget = target;
        Vector3 targetDirection = transform.position - _flyTarget.transform.position;

        transform.DOMove(_flyTarget.transform.position, 3f);
    }
}
