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

    [SerializeField] private TMPro.TMP_Text xpText;
    [SerializeField] private TMPro.TMP_Text levelText;

    public OnHighscore OnHighscore = new OnHighscore();

    private int playerLevel = 1;
    private int currentXp = 0;
    private int lastXp = 0;

    private void Awake()
    {
        instance = this;
        SetXp(currentXp);
        SetPlayerLevel(playerLevel);
    }

    private void Update()
    {
        if(currentXp != lastXp)
        {
            SetXp(currentXp);
        }
    }

    private void SetXp(int current)
    {
        lastXp = current;

        xpText.text = $"XP {current.ToString()}";
        OnHighscore.Invoke(current);

        if(current >= GetXpForLevelUp())
        {
            // Show skill choose menu
            var options = Upgrades.Instance.GetUpgradeOptions();
            foreach(var option in options)
            {
                Debug.Log(option);
            }
            options[1].onSelected();
            SetXp(0);
            SetPlayerLevel(playerLevel + 1);
        }
    }

    private void SetPlayerLevel(int level)
    {
        playerLevel = level;
        levelText.text = $"Level {playerLevel.ToString()}";
    }

    public void AddScore(int score)
    {
        currentXp += score;
    }

    private int GetXpForLevelUp()
    {
        var asIdx = playerLevel -1;
        if(asIdx < XpForLevelUp.Length)
        {
            return XpForLevelUp[asIdx];
        }
        else
        {
            return XpForLevelUp[XpForLevelUp.Length - 1];
        }
    }

    public int[] XpForLevelUp = new int[]{
        100,
        150,
        250,
        300,
        400,
        500,
        700,
        900,
        1100,
        1500,
        2000
    };
}
