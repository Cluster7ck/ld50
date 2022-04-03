using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [SerializeField] private GameObject levelUpScreen;

    [SerializeField] private Button leftButton;
    [SerializeField] private Image leftButtonImage;
    [SerializeField] private TMPro.TMP_Text leftButtonText;

    [SerializeField] private Button rightButton;
    [SerializeField] private Image rightButtonImage;
    [SerializeField] private TMPro.TMP_Text rightButtonText;

    [SerializeField] private GameObject skillLearnedPrefab;
    [SerializeField] private RectTransform skillLearnedParent;

    [SerializeField] private float buttonStartY;
    [SerializeField] private float buttonEndY;
    [SerializeField] private float buttonAnimTime;

    public OnHighscore OnHighscore = new OnHighscore();

    private int playerLevel = 1;
    private int currentXp = 0;
    private int lastXp = 0;

    private void Awake()
    {
        instance = this;
        Reset();
    }

    public void Reset()
    {
        playerLevel = 1;
        currentXp = 0;
        lastXp = 0;
        SetXp(currentXp);
        SetPlayerLevel(playerLevel);
        levelUpScreen.SetActive(false);
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
            OpenLevelScreen(options);

            currentXp = 0;
            SetXp(currentXp);
            SetPlayerLevel(playerLevel + 1);
        }
    }

    private void OpenLevelScreen(List<UpgradeOption> options)
    {
        Time.timeScale = 0.005f;
        levelUpScreen.gameObject.SetActive(true);

        SetupButton(leftButton, leftButtonImage, leftButtonText, options[0]);
        SetupButton(rightButton, rightButtonImage, rightButtonText, options[1]);
    }

    private void SetupButton(Button button, Image image, TMPro.TMP_Text text, UpgradeOption option)
    {
        ButtonAnimation(button.gameObject);
        image.sprite = option.sprite;
        text.text = option.text;
        button.onClick.AddListener(() => OnButtonClicked(option));
        button.onClick.AddListener(CloseLevelScreen);
    }

    private void ButtonAnimation(GameObject button)
    {
        var p = button.transform.position;
        button.transform.position = new Vector3(p.x, buttonStartY, p.z);

        button.transform.DOLocalMoveY(buttonEndY, buttonAnimTime)
            .SetEase(Ease.InBounce)
            .SetUpdate(true);
    }

    private void OnButtonClicked(UpgradeOption option)
    {
        option.onSelected();
        var icon = Instantiate(skillLearnedPrefab);
        icon.GetComponent<UpgradeIcon>().SetSprite(option.spriteSecondary);
        icon.transform.SetParent(skillLearnedParent);
    }

    private void CloseLevelScreen()
    {
        Time.timeScale = 1;
        levelUpScreen.gameObject.SetActive(false);

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
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
