using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int currentHighscore = 0;
    private int lastHighScore = 0;

    private void Awake()
    {
        instance = this;
    }
}
