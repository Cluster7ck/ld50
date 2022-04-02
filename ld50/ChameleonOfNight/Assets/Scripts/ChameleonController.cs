using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChameleonController : MonoBehaviour
{
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private LineRenderer tongue;
    [SerializeField] private Transform tongueCollider;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private LayerMask insectMask;

    [SerializeField] private float tongueFlickExtendTime = 0.2f;
    [SerializeField] private Ease tongueFlickeExtendEase;
    [SerializeField] private float tongueFlickRetreatTime = 0.2f;
    [SerializeField] private Ease tongueFlickeRetreatEase;

    private Sequence sequence;

    void Update()
    {
        var hitInsects = Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, insectMask);
        if(hitInsects.Length > 0)
        {
            foreach(var insectCollider in hitInsects)
            {
                var enemy = insectCollider.GetComponent<Enemy>();
                // hit enemy
                if(enemy)
                    enemy.Hit(sphereCollider.transform, 0.4f);
            }
        }
    }

    public void Click(Vector3 worldPos)
    {
        Debug.Log("Chameleon click");
        if(sequence?.IsActive() ?? false) return;

        tongue.SetPositions(new Vector3[]{tongueOrigin.position, tongueOrigin.position});
        sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), worldPos, tongueFlickExtendTime).SetEase(tongueFlickeExtendEase));
        sequence.Append(DOTween.To(() => tongue.GetPosition(1), x => SetTongueTip(x), tongueOrigin.position, tongueFlickRetreatTime).SetEase(tongueFlickeRetreatEase));
        sequence.Play();
    }

    private void SetTongueTip(Vector3 target)
    {
        tongue.SetPosition(1, target);
        tongueCollider.position = target;
    }
}
