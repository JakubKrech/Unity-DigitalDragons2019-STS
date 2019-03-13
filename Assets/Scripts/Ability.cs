﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("Ability Characteristics")]
    public string abilityName;
    public int manaCost = 0;
    public int healthCost = 0;
    public int cooldown;
    public int baseDamage = 0;
    public float strengthMultiplier = 0;
    public float powerMultiplier = 0;
    public float agilityMultiplier = 0;
    public int range;

    [Header("Elements")]
    public Sprite icon;

    public Ability(string n, int mC, int hC, int cd, int bD, int sM, int pM, int aM, int r)
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

    int calculateDamage(Character attacker, Character attacked)
    {
        float critRoll = Random.Range(.0f, 1.0f);
        int critCheck = 0;
        if(critRoll >= attacker.critChance) critCheck = 1;

        float damage = ((baseDamage + strengthMultiplier * attacker.strength + 
            powerMultiplier * attacker.power + agilityMultiplier * attacker.agility) * 
            critCheck * 1.5f) * (1 - attacked.armor);

        return Mathf.RoundToInt(damage);
    }

    void dealDamage(Character attacker, Character attacked)
    {
        int damageDealt = calculateDamage(attacker, attacked);

        if(attacked.currentHP - damageDealt < 0) 
        {
            attacked.currentHP = 0;
            attacked.alive = false;
        }
        else attacked.currentHP -= damageDealt;

        // healthCost
        if(attacker.currentHP - healthCost < 0) attacker.currentHP = 0;
        else attacker.currentHP -= healthCost;

        // manaCost
        if(attacker.currentMana - manaCost < 0) attacker.currentMana = 0;
        else attacker.currentMana -= manaCost;
    }
}
