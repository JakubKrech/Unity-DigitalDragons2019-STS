using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
   // [SerializeField]
    public List<Ability> abilities;

    // Update is called once per frame
    void Awake()
    {
        BuildAbilitiesDatabase();
    }

    void BuildAbilitiesDatabase()
    {
        abilities = new List<Ability>
        {
            // abilityName, manaCost, healthCost, cooldown, baseDamage,
            // strengthMultiplier, powerMultiplier, agilityMultiplier, range

            new Ability("Melee Basic Attack", 0, 0, 1, 10, 3, 0, 0, false),
            new Ability("Shield Bash", 10, 0, 3, 15, 5, 0, 0, false)


        };
    }
}
