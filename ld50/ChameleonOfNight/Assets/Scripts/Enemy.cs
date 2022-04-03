using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnEnemyKill : UnityEvent {}

public class Enemy : MonoBehaviour
{
    [SerializeField] public int scoreValue;
    public bool isHit { get; protected set;} = false;

    public OnEnemyKill OnEnemyKill { get; } = new OnEnemyKill();

    public virtual void Hit()
    {

    }

    public virtual void RealDestroy()
    {
    }
}
