using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CardSelectionUI : MonoBehaviour
{
    [System.Serializable]
    public class AbilityCardUI
    {
        public AbilityType abilityType;
        public Sprite cardImage;
    }

    public GameObject cardPanel;
    public Button[] cardButtons;
    public Sprite[] abilitySprites;

    private List<AbilityCardUI> allCards;
    private List<AbilityCardUI> offeredCards = new List<AbilityCardUI>();

    void Start()
    {
        BuildCardPool();
        ShowRandomCards();
    }

    void BuildCardPool()
    {
        allCards = new List<AbilityCardUI>
        {
            new AbilityCardUI { abilityType = AbilityType.HealthBoost,    cardImage = abilitySprites[0] },
            new AbilityCardUI { abilityType = AbilityType.JumpBoost,      cardImage = abilitySprites[1] },
            new AbilityCardUI { abilityType = AbilityType.HairballAttack, cardImage = abilitySprites[2] },
            new AbilityCardUI { abilityType = AbilityType.StarAttack,     cardImage = abilitySprites[3] },
            new AbilityCardUI { abilityType = AbilityType.FreezeEnemy,    cardImage = abilitySprites[4] }
        };
    }

    void ShowRandomCards()
    {
        offeredCards.Clear();
        List<AbilityCardUI> pool = new List<AbilityCardUI>(allCards);

        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            offeredCards.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i < offeredCards.Count)
            {
                cardButtons[i].gameObject.SetActive(true);

                Image img = cardButtons[i].GetComponentInChildren<Image>();
                if (img != null && offeredCards[i].cardImage != null)
                    img.sprite = offeredCards[i].cardImage;

                cardButtons[i].onClick.RemoveAllListeners();
                int captured = i;
                cardButtons[i].onClick.AddListener(() => OnCardSelected(captured));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false);
            }
        }

        cardPanel.SetActive(true);
    }

    void OnCardSelected(int index)
    {
        AbilityCardUI chosen = offeredCards[index];
        Debug.Log("Player chose: " + chosen.abilityType);

        if (PlayerAbilities.instance != null)
            PlayerAbilities.instance.ApplyAbility(chosen.abilityType);

        cardPanel.SetActive(false);

        
        string nextLevel = PlayerPrefs.GetString("NextLevel", "Level 1");
        Debug.Log("Loading next level: " + nextLevel);
        SceneManager.LoadScene(nextLevel);
    }
}