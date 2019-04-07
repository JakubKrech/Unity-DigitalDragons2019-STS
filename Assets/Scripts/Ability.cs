using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public bool locked = false; // indicates if ability is unlocked yet
    [Header("Ability Characteristics")]
    public string abilityName;
    public string description;
    public int manaCost = 0;
    public int healthCost = 0;
    public int actionPointsCost = 0;
    public int cooldown;
    public int currentCooldown = 0;
    public int baseDamage = 0;
    public float strengthMultiplier = 0;
    public float powerMultiplier = 0;
    public float agilityMultiplier = 0;
    //public int range;
    public bool range;
    public int stunLength;
    public bool removeStun = false;
    public int immobilizeLength;
    public bool removeImmobilize = false;
    public int tauntLength;
    public bool removeTaunt = false;

    [Header("Elements")]
    public Sprite icon;

    public Ability(string n, int mC, int hC, int cd, int bD, int sM, int pM, int aM, bool r)
    {
        this.abilityName = n;
        this.manaCost = mC;
        this.healthCost = hC;
        this.cooldown = cd;
        this.baseDamage = bD;
        this.strengthMultiplier = sM;
        this.powerMultiplier = pM;
        this.agilityMultiplier = aM;
        this.range = r;
    }

    public int calculateDamage(Character attacker)//, Character attacked)
    {
        float critRoll = Random.Range(.0f, 1.0f);
        int critCheck = 0;
        if(critRoll >= attacker.critChance) critCheck = 1;

        float damage = ((baseDamage + strengthMultiplier * attacker.strength + 
            powerMultiplier * attacker.power + agilityMultiplier * attacker.agility) * 
            critCheck * 1.5f);// * (1 - attacked.armor);

        return Mathf.RoundToInt(damage);
    }

    public void dealDamage(Character attacker, Character attacked)
    {
        int damageDealt = calculateDamage(attacker);//, attacked);

        string debugMessage = attacker.charName + " has used " + this.name + " on " + attacked.charName + "\n" +
            attacker.charName + " -" + manaCost + "mana, -" + healthCost + "HP, -" + actionPointsCost + "AP\n" +
            attacked.charName + " " + (-damageDealt) + "HP";

        Debug.Log(debugMessage);
        StartCoroutine(attacker.BSM.UIM.ShowFadeingMessage(debugMessage));

        if(attacked.currentHP - damageDealt <= 0) 
        {
            attacked.currentHP = 0;
            attacked.alive = false;
            // death animation
            attacked.characterDies();
            Debug.Log(attacked.charName + " DIES!!!");
        }
        else attacked.currentHP -= damageDealt;

        if(attacked.currentHP > attacked.maxHP) attacked.currentHP = attacked.maxHP;
        if(attacked.currentMana > attacked.maxMana) attacked.currentMana = attacked.maxMana;

        // healthCost
        if(attacker.currentHP - healthCost <= 0) 
        {
            attacker.currentHP = 0;
            attacker.alive = false;
            // death animation
            attacker.characterDies();
            Debug.Log(attacker.charName + " DIES!!!");
        }
        else attacker.currentHP -= healthCost;
        if(healthCost != 0){
            StartCoroutine(showDmgIndicator(attacker, healthCost, attacker.BSM.UIM.currentIndicatorText));
            if(attacker.BSM.UIM.currentIndicatorText + 1 >= attacker.BSM.UIM.damageIndicatorTexts.Count) attacker.BSM.UIM.currentIndicatorText = 0;
            else ++attacker.BSM.UIM.currentIndicatorText;
        } 

        if(attacker.currentHP > attacker.maxHP) attacker.currentHP = attacker.maxHP;
        if(attacker.currentMana > attacker.maxMana) attacker.currentMana = attacker.maxMana;

        // manaCost
        if(attacker.currentMana - manaCost < 0) attacker.currentMana = 0;
        else attacker.currentMana -= manaCost;

        // APCost
        attacker.currentActionPoints -= actionPointsCost;

        // Effects
        if(stunLength > 0){ 
            attacked.stunnedFor += this.stunLength; 
            if(attacked.stunnedFor > this.stunLength) attacked.stunnedFor = this.stunLength;
        }
        if(immobilizeLength > 0){
            attacked.immobilizedFor += this.immobilizeLength;
            if(attacked.immobilizedFor > this.immobilizeLength) attacked.immobilizedFor = this.immobilizeLength;
        }

        if(tauntLength > 0){
            attacked.tauntedFor += this.tauntLength;
            if(attacked.tauntedFor > this.tauntLength) attacked.tauntedFor = this.tauntLength;

            attacked.tauntTarget = attacker;
        }

        if(removeStun) attacked.stunnedFor = 0;
        if(removeImmobilize) attacked.immobilizedFor = 0;
        if(removeTaunt) attacked.tauntedFor = 0;

        // ability cd
        currentCooldown = cooldown;

        // show dmg indicator
        StartCoroutine(showDmgIndicator(attacked, damageDealt, attacker.BSM.UIM.currentIndicatorText));
        if(attacker.BSM.UIM.currentIndicatorText + 1 >= attacker.BSM.UIM.damageIndicatorTexts.Count) attacker.BSM.UIM.currentIndicatorText = 0;
        else ++attacker.BSM.UIM.currentIndicatorText;
    }

    IEnumerator showDmgIndicator(Character target, int dmg, int ind)
    {
        int fadeSpeed = 3;

        // int index = target.BSM.UIM.currentIndicatorText;
        // if(target.BSM.UIM.currentIndicatorText >= target.BSM.UIM.damageIndicatorTexts.Count) target.BSM.UIM.currentIndicatorText = 0;
        // else ++target.BSM.UIM.currentIndicatorText;
        int index = ind;
        Text text = target.BSM.UIM.damageIndicatorTexts[index];
        RectTransform rectTransform = text.GetComponent<RectTransform>();

        if(dmg >= 0) {
            text.text = "-" + dmg.ToString();
            text.color = new Color32(255, 12, 0 , 0);
        }
        else { 
            text.text = "+" + (-dmg).ToString();
            text.color = new Color32(31, 217, 0 , 0);
        }

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(target.transform.position);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;


        while(text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g,
                text.color.b, text.color.a + (Time.deltaTime * fadeSpeed));

            yield return null;
        }
        //text.transform

        yield return new WaitForSeconds (2);

        while(text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g,
                text.color.b, text.color.a - (Time.deltaTime * fadeSpeed));
            
            yield return null;
        }
    }
}
