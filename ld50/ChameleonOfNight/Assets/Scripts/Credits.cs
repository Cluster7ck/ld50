using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    private RectTransform thisRect;
    [SerializeField] private float endY;
    [SerializeField] private float startY;

    private void Start () {
        thisRect = GetComponent<RectTransform>();
        endY = thisRect.transform.localPosition.y;
    }
    public void Appear() {
        thisRect.transform.DOLocalMoveY(endY, 1f);
    }

    public void Disappear() {
        thisRect.transform.DOLocalMoveY(startY, 1f);
    }
}
