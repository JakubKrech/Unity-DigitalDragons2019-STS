using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int applyStun;
    public bool removeStun = false;
    public int applyImmobilize;
    public bool removeImmobilize = false;

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

        Debug.Log(attacker.charName + " has attacked " + attacked.charName + " using " + this.name);
        Debug.Log(attacker.charName + " -" + manaCost + "mana, -" + healthCost + "HP, -" + actionPointsCost + "AP");
        Debug.Log(attacked.charName + " -" + damageDealt + "HP");

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

        // manaCost
        if(attacker.currentMana - manaCost < 0) attacker.currentMana = 0;
        else attacker.currentMana -= manaCost;

        // APCost
        attacker.currentActionPoints -= actionPointsCost;
    }
}
