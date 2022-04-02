using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChameleonController : MonoBehaviour
{
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private LineRenderer tongue;
    [SerializeField] private Transform tongueCollider;

    [SerializeField] private float tongueFlickExtendTime = 0.2f;
    [SerializeField] private Ease tongueFlickeExtendEase;
    [SerializeField] private float tongueFlickRetreatTime = 0.2f;
    [SerializeField] private Ease tongueFlickeRetreatEase;
    private Sequence sequence;

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
