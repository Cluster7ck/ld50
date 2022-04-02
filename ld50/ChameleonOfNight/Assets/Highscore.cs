using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHighscore : UnityEvent<int> {}
public class Highscore : MonoBehaviour
{
    private static Highscore instance;
    public static Highscore Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private TMPro.TMP_Text highScoreText;

    public OnHighscore OnHighscore = new OnHighscore();
    private int currentHighscore = 0;
    private int lastHighScore = 0;

    private void Awake()
    {
        instance = this;
        highScoreText.text = currentHighscore.ToString();
    }

    private void Update()
    {
        if(currentHighscore != lastHighScore)
        {
            highScoreText.text = currentHighscore.ToString();
            lastHighScore = currentHighscore;
            OnHighscore.Invoke(currentHighscore);
        }
    }

    public void AddScore(int score)
    {
        currentHighscore += score;
    }
}
