using System;
using UnityEngine;

public class StartKill : Enemy
{
    private Action startAction;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }
    
    public void SetStartAction(Action action)
    {
        startAction =  action;
    }

    public override void Hit()
    {
        if(isHit) return;
        isHit = true;

        startAction();
    }

    public override void RealDestroy()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);

        transform.position = startPos;
        transform.rotation = startRot;
    }
}