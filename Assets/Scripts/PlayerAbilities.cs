using UnityEngine;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities instance;

    public List<AbilityType> unlockedAbilities = new List<AbilityType>();
    public int bonusHealth = 0;
    public float jumpBoostAmount = 3f;
    public bool canThrowStars = false;
    public float starCooldown = 3f;      // decreases each repeat
    public float freezeDuration = 3f;    // increases each repeat
    public bool canFreeze = false;
    public bool hasHairball = false;
    public int hairballDamage = 1;       // increases each repeat
    public int starDamage = 1;           // base star damage

    void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }
    else
    {
        Destroy(gameObject);
    }
}

    public void ApplyAbility(AbilityType ability)
    {
        unlockedAbilities.Add(ability); // no duplicate check — stacking allowed

        switch (ability)
        {
            case AbilityType.HealthBoost:
                bonusHealth += 1;
                break;
            case AbilityType.JumpBoost:
                jumpBoostAmount += 1f;
                break;
            case AbilityType.HairballAttack:
                if (!hasHairball) hasHairball = true;
                else hairballDamage += 1;
                break;
            case AbilityType.StarAttack:
                if (!canThrowStars) canThrowStars = true;
                else starCooldown = Mathf.Max(0.5f, starCooldown - 0.5f);
                break;
            case AbilityType.FreezeEnemy:
                if (!canFreeze) canFreeze = true;
                else freezeDuration += 1f;
                break;
        }

        Save();
    }

    public bool HasAbility(AbilityType ability)
    {
        return unlockedAbilities.Contains(ability);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("BonusHealth", bonusHealth);
        PlayerPrefs.SetFloat("JumpBoostAmount", jumpBoostAmount);
        PlayerPrefs.SetInt("HasHairball", hasHairball ? 1 : 0);
        PlayerPrefs.SetInt("HairballDamage", hairballDamage);
        PlayerPrefs.SetInt("CanThrowStars", canThrowStars ? 1 : 0);
        PlayerPrefs.SetFloat("StarCooldown", starCooldown);
        PlayerPrefs.SetInt("CanFreeze", canFreeze ? 1 : 0);
        PlayerPrefs.SetFloat("FreezeDuration", freezeDuration);
        PlayerPrefs.Save();
        Debug.Log("Abilities saved");
    }

    public void Load()
    {
        bonusHealth = PlayerPrefs.GetInt("BonusHealth", 0);
        jumpBoostAmount = PlayerPrefs.GetFloat("JumpBoostAmount", 3f);
        hasHairball = PlayerPrefs.GetInt("HasHairball", 0) == 1;
        hairballDamage = PlayerPrefs.GetInt("HairballDamage", 1);
        canThrowStars = PlayerPrefs.GetInt("CanThrowStars", 0) == 1;
        starCooldown = PlayerPrefs.GetFloat("StarCooldown", 3f);
        canFreeze = PlayerPrefs.GetInt("CanFreeze", 0) == 1;
        freezeDuration = PlayerPrefs.GetFloat("FreezeDuration", 3f);
        Debug.Log("Abilities loaded");
    }

    public void ResetAbilities()
    {
        PlayerPrefs.DeleteKey("BonusHealth");
        PlayerPrefs.DeleteKey("JumpBoostAmount");
        PlayerPrefs.DeleteKey("HasHairball");
        PlayerPrefs.DeleteKey("HairballDamage");
        PlayerPrefs.DeleteKey("CanThrowStars");
        PlayerPrefs.DeleteKey("StarCooldown");
        PlayerPrefs.DeleteKey("CanFreeze");
        PlayerPrefs.DeleteKey("FreezeDuration");
        PlayerPrefs.Save();

        bonusHealth = 0;
        jumpBoostAmount = 3f;
        hasHairball = false;
        hairballDamage = 1;
        canThrowStars = false;
        starCooldown = 3f;
        canFreeze = false;
        freezeDuration = 3f;
        unlockedAbilities.Clear();
    }
}