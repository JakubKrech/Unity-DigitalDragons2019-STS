using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [Header("Character Window")]
    public Text CharacterName;
    public Image CharacterAvatar;
    public Text CharacterLevel;
    public Slider HealthBar;
    public Text HealthBarText;
    public Slider ManaBar;
    public Text ManaBarText;
    public Text CharStrength, CharAgility, CharPower, CharInitiative;

    [Header("Queue")]
    public Image[] OrderQueue;
    [Header("Ability Bar")]
    public Text ActionPointsText;
    public Text ActionPointsRegenText;
    public Image[] AbilityBar;
    public Image[] AbilityBarBorders;
    public Image[] AbilityBarCooldownCovers;
    public Text[] AbilityBarCooldownCoversText;
    [Header("Result Screen Window")]
    public GameObject ResultScreenWindow;
    public Text battleOutcome;
    public Text battleOutcomeDescription;
    [Header("Ability Description Panel")]
    public GameObject ADP;
    public Text ADPAbilityName;
    public Text ADPDescription;
    public Text ADPSkillCostValue;
    public Text ADPSkillDamageValue;
    public Text ADPSkillEffects;
    public Text ADPAPCost;
    public Text ADPSkillCooldown;
    public Text ADPRangedMelee;
    

    [Header("Other")]
    public Text turnCounterText;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void updateHealthBar(Character character)
    {
        HealthBar.maxValue = character.maxHP;
        HealthBar.value = character.currentHP;
        HealthBarText.text = character.currentHP + " / " + character.maxHP;
    }

    public void updateManaBar(Character character)
    {
        ManaBar.maxValue = character.maxMana;
        ManaBar.value = character.currentMana;
        ManaBarText.text = character.currentMana + " / " + character.maxMana;
    }

    public void updateOrderQueue(List<Character> charList)
    {
        if(charList.Any())
        {
            for(int i = 0; i < OrderQueue.Length; i++)
            {
                if(i < charList.Count){
                    OrderQueue[i].sprite = charList[i].characterAvatar;
                    OrderQueue[i].enabled = true;
                }
                else OrderQueue[i].enabled = false;
            }
        }
    }

    public void updateAbilityBar(Character character)
    {
        ActionPointsText.text = character.currentActionPoints + " / " + character.maxActionPoints;
        ActionPointsRegenText.text = "+" + character.actionPointsRegen + " PER";

        for(int i = 0; i < character.abilities.Count; i++)
        {
            AbilityBar[i].sprite = character.abilities[i].icon;
            if(character.abilities[i].currentCooldown > 0)
            { 
                AbilityBarCooldownCovers[i].transform.gameObject.SetActive(true);
                AbilityBarCooldownCovers[i].fillAmount = character.abilities[i].currentCooldown / character.abilities[i].cooldown;
                AbilityBarCooldownCoversText[i].text = character.abilities[i].currentCooldown.ToString();
            }
            else
            {
                AbilityBarCooldownCovers[i].transform.gameObject.SetActive(false);
            }
        }
    }

    public void updateUIToChosenCharacter(Character character)
    {
        CharacterName.text = character.charName;
        CharacterAvatar.sprite = character.characterAvatar;
        CharacterLevel.text = "LEVEL " + character.level;
        updateHealthBar(character);
        updateManaBar(character);
        updateAbilityBar(character);

        CharStrength.text = character.strength.ToString();
        CharAgility.text = character.agility.ToString();
        CharPower.text = character.power.ToString();
        CharInitiative.text = character.initiative.ToString();
    }

    public void updateTurnCounter(int currentTurnNumber)
    {
        turnCounterText.text = "Turn " + currentTurnNumber;
    }
}
