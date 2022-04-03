using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private static Upgrades instance;
    public static Upgrades Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private float BaseSpeedMultiplier;
    [SerializeField] private float ChainBaseRange;
    [SerializeField] private float ChainPerLevelRange;
    [SerializeField] private float StarBaseRadius;
    [SerializeField] private float StarPerLevelRadius;

    [SerializeField] private Sprite SpeedUpgradeSprite;
    [SerializeField] private Sprite ChainUpgradeSprite;
    [SerializeField] private Sprite ChainRangeUpgradeSprite;
    [SerializeField] private Sprite StarPointsUpgradeSprite;
    [SerializeField] private Sprite StarRadiusUpgradeSprite;

    [SerializeField] private Sprite SpeedUpgradeSpriteSecondary;
    [SerializeField] private Sprite ChainUpgradeSpriteSecondary;
    [SerializeField] private Sprite ChainRangeUpgradeSpriteSecondary;
    [SerializeField] private Sprite StarPointsUpgradeSpriteSecondary;
    [SerializeField] private Sprite StarRadiusUpgradeSpriteSecondary;

    public float ExtraSpeed => SpeedLevel * BaseSpeedMultiplier;
    public float StarRadius => StarRadiusLevel * StarPerLevelRadius + StarBaseRadius;
    public float ChainRange => ChainRangeLevel * ChainPerLevelRange + ChainBaseRange;

    public int ChainLevel;
    public int ChainRangeLevel;
    public int StarPointsLevel;
    public int StarRadiusLevel;
    public int SpeedLevel;
    // TODO moar upgrades
    // more tongues?
    // Charm explosion: turns flies onto other flies 

    public void Reset()
    {
        ChainLevel = 0;
        ChainRangeLevel = 0;
        StarRadiusLevel = 0;
        StarPointsLevel = 0;
        SpeedLevel = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Reset();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChainLevel++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChainRangeLevel++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            StarRadiusLevel++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(StarPointsLevel == 0)
            {
                StarPointsLevel = 2;
            }
            else
            {
            StarPointsLevel++;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpeedLevel++;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }

    void Awake()
    {
        instance = this;   
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        var options = new List<UpgradeOption>();
        AddSpeedOption(options);
        AddStarPointsOption(options);
        AddStarRadiusOption(options);
        AddChainOption(options);
        AddChainRangeOption(options);

        Helper.Shuffle(options);
        return options.Take(2).ToList();
    }

    private void AddSpeedOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = SpeedUpgradeSprite,
            spriteSecondary = SpeedUpgradeSpriteSecondary,
            onSelected = () => SpeedLevel += 1,
            currentLevel = SpeedLevel,
            text = "Tongue speed"
        });
    }

    private void AddStarPointsOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = StarPointsUpgradeSprite,
            spriteSecondary=StarPointsUpgradeSpriteSecondary,
            onSelected = () => {
                if(StarPointsLevel == 0)
                {
                    StarPointsLevel += 2;
                }
                else
                {
                    StarPointsLevel++;
                }
            },
            currentLevel = StarPointsLevel,
            text = "Tongue splits"
        });
    }

    private void AddStarRadiusOption(List<UpgradeOption> options)
    {
        if(StarPointsLevel > 0)
        {
            options.Add(new UpgradeOption{
                sprite = StarRadiusUpgradeSprite,
                spriteSecondary=StarRadiusUpgradeSpriteSecondary,
                onSelected = () => StarRadiusLevel += 1,
                currentLevel = StarRadiusLevel,
                text = "Split radius"
            });
        }
    }

    private void AddChainOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = ChainUpgradeSprite,
            spriteSecondary = ChainUpgradeSpriteSecondary,
            onSelected = () => ChainLevel += 1,
            currentLevel = ChainLevel,
            text = "Tongue bounces"
        });
    }

    private void AddChainRangeOption(List<UpgradeOption> options)
    {
        if(ChainLevel > 0)
        {
            options.Add(new UpgradeOption{
                sprite = ChainRangeUpgradeSprite,
                spriteSecondary=ChainRangeUpgradeSpriteSecondary,
                onSelected = () => ChainRangeLevel += 1,
                currentLevel = ChainRangeLevel,
                text = "Increase Bounce Range"
            });
        }
    }

}

public class UpgradeOption
{
    public Sprite sprite;
    public Sprite spriteSecondary;
    public Action onSelected;
    public int currentLevel;
    public string text;
}