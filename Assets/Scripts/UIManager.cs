using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text CharacterName;
    public Slider HealthBar;
    public Text HealthBarText;
    public Slider ManaBar;
    public Text ManaBarText;

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
        HealthBarText.text = character.currentHP + "/" + character.maxHP;
    }

    public void updateManaBar(Character character)
    {
        ManaBar.maxValue = character.maxMana;
        ManaBar.value = character.currentMana;
        ManaBarText.text = character.currentMana + "/" + character.maxMana;
    }
}
