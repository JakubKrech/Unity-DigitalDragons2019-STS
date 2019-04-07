using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Character Window")]
    public Text CharacterName;
    public Image CharacterAvatar;
    public Text CharacterLevel;
    public Slider HealthBar;
    public Text HealthBarText;
    public Slider ManaBar;
    public Text ManaBarText, ManaRegenText;
    public Text CharStrength, CharAgility, CharPower, CharInitiative;
    public Image StunIndicator, ImmobilizeIndicator, TauntIndicator;
    public Text StunIndicatorText, ImmobilizeIndicatorText, TauntIndicatorText;
    public Image Rune;

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
    [Header("Damage Indicator")]
    public List<Text> damageIndicatorTexts;
    public int currentIndicatorText = 0;

    [Header("Menu/Help Screen")]
    public GameObject helpScreen;

    [Header("Other")]
    public Text turnCounterText;
    public Text OnScreenTextMessage;
    public Image OnScreenTextPanel;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.H))
        {
            if(helpScreen.activeSelf) helpScreen.SetActive(false);
            else helpScreen.SetActive(true);
        }
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
        ManaRegenText.text = "+" + (character.regenMana + character.power);
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
                AbilityBarCooldownCovers[i].fillAmount = 1.0f * character.abilities[i].currentCooldown / character.abilities[i].cooldown;
                AbilityBarCooldownCoversText[i].text = character.abilities[i].currentCooldown.ToString();
            }
            else
            {
                AbilityBarCooldownCovers[i].transform.gameObject.SetActive(false);
            }
        }
    }

    public void updateEffectIndicators(Character character)
    {
        if(character.stunnedFor > 0 || character.immobilizedFor > 0 || character.tauntedFor > 0){
            if(character.stunnedFor > 0){
                StunIndicatorText.text = "Stun(" + character.stunnedFor + ")";
                StunIndicator.gameObject.SetActive(true);
            }
            if(character.immobilizedFor > 0){
                ImmobilizeIndicatorText.text = "Stuck(" + character.immobilizedFor + ")";
                ImmobilizeIndicator.gameObject.SetActive(true);
            }
            if(character.tauntedFor > 0){
                TauntIndicatorText.text = "Taunted(" + character.tauntedFor + ")";
                TauntIndicator.sprite = character.tauntTarget.characterAvatar;
                TauntIndicator.gameObject.SetActive(true);
            }
            Rune.gameObject.SetActive(false);
        }
        else {
            StunIndicator.gameObject.SetActive(false);
            ImmobilizeIndicator.gameObject.SetActive(false);
            TauntIndicator.gameObject.SetActive(false);
            Rune.gameObject.SetActive(true);
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
        updateEffectIndicators(character);

        CharStrength.text = character.strength.ToString();
        CharAgility.text = character.agility.ToString();
        CharPower.text = character.power.ToString();
        CharInitiative.text = character.initiative.ToString();
    }

    public void updateTurnCounter(int currentTurnNumber)
    {
        turnCounterText.text = "Turn " + currentTurnNumber;
        StartCoroutine(ShowFadeingMessage("Turn " + currentTurnNumber));
    }

    public IEnumerator ShowFadeingMessage(string message)
    {
        int fadeSpeed = 2;

        OnScreenTextMessage.text = message;
        
        while(OnScreenTextMessage.color.a < 1.0f)
        {
            OnScreenTextMessage.color = new Color(OnScreenTextMessage.color.r, OnScreenTextMessage.color.g,
                OnScreenTextMessage.color.b, OnScreenTextMessage.color.a + (Time.deltaTime * fadeSpeed));

            OnScreenTextPanel.color = new Color(OnScreenTextPanel.color.r, OnScreenTextPanel.color.g,
                OnScreenTextPanel.color.b, OnScreenTextPanel.color.a + (Time.deltaTime * fadeSpeed));
            
            yield return null;
        }


        yield return new WaitForSeconds (1);

        while(OnScreenTextMessage.color.a > 0.0f)
        {
            OnScreenTextMessage.color = new Color(OnScreenTextMessage.color.r, OnScreenTextMessage.color.g,
                OnScreenTextMessage.color.b, OnScreenTextMessage.color.a - (Time.deltaTime * fadeSpeed));

            OnScreenTextPanel.color = new Color(OnScreenTextPanel.color.r, OnScreenTextPanel.color.g,
                OnScreenTextPanel.color.b, OnScreenTextPanel.color.a - (Time.deltaTime * fadeSpeed));
            
            yield return null;
        }

    }
}
