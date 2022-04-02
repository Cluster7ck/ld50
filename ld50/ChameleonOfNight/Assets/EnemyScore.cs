using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyScore : MonoBehaviour
{
    [SerializeField] private float riseHeight;
    [SerializeField] private TMPro.TMP_Text text;

    int score;
    private Enemy enemy;
    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        score = enemy.scoreValue;
        text.text = score.ToString();
        enemy.OnEnemyKill.AddListener(Show);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        enemy.OnEnemyKill.RemoveListener(Show);
    }

    public void Show()
    {
        transform.parent = null;
        gameObject.SetActive(true);

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y + riseHeight, 1f).SetEase(Ease.OutSine));
        //sequence.Join(transform)
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
