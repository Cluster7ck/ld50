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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChainLevel = 0;
            ChainRangeLevel = 0;
            StarRadiusLevel = 0;
            StarPointsLevel = 0;
            SpeedLevel = 0;
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

    public List<UpgradeOptions> GetUpgradeOptions()
    {
        var options = new List<UpgradeOptions>();
        AddSpeedOption(options);
        AddStarPointsOption(options);
        AddStarRadiusOption(options);
        AddChainOption(options);
        AddChainRangeOption(options);

        Helper.Shuffle(options);
        return options.Take(2).ToList();
    }

    private void AddSpeedOption(List<UpgradeOptions> options)
    {
        options.Add(new UpgradeOptions{
            sprite = SpeedUpgradeSprite,
            onSelected = () => SpeedLevel += 1,
            currentLevel = SpeedLevel,
        });
    }

    private void AddStarPointsOption(List<UpgradeOptions> options)
    {
        options.Add(new UpgradeOptions{
            sprite = StarPointsUpgradeSprite,
            onSelected = () => StarPointsLevel += 1,
            currentLevel = StarPointsLevel,
        });
    }

    private void AddStarRadiusOption(List<UpgradeOptions> options)
    {
        if(StarPointsLevel > 0)
        {
            options.Add(new UpgradeOptions{
                sprite = StarRadiusUpgradeSprite,
                onSelected = () => StarRadiusLevel += 1,
                currentLevel = StarRadiusLevel,
            });
        }
    }

    private void AddChainOption(List<UpgradeOptions> options)
    {
        options.Add(new UpgradeOptions{
            sprite = ChainUpgradeSprite,
            onSelected = () => ChainLevel += 1,
            currentLevel = ChainLevel,
        });
    }

    private void AddChainRangeOption(List<UpgradeOptions> options)
    {
        if(ChainLevel > 0)
        {
            options.Add(new UpgradeOptions{
                sprite = ChainRangeUpgradeSprite,
                onSelected = () => ChainRangeLevel += 1,
                currentLevel = ChainRangeLevel,
            });
        }
    }

    public class UpgradeOptions
    {
        public Sprite sprite;
        public Action onSelected;
        public int currentLevel;
    }
}
