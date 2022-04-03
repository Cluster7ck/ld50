using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    private RectTransform thisRect;

    private void Start () {
        thisRect = GetComponent<RectTransform>();
    }
    public void Appear() {
        thisRect.DOAnchorPosX(thisRect.anchoredPosition.x - 600, 1f);
    }

    public void Disappear() {
        thisRect.DOAnchorPosX(thisRect.anchoredPosition.x + 600, 1f);
    }
}
