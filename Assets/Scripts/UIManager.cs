using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public Text CharacterName;
    public Image CharacterAvatar;
    public Slider HealthBar;
    public Text HealthBarText;
    public Slider ManaBar;
    public Text ManaBarText;
    public Image[] OrderQueue;
    public Image[] AbilityBar;

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
        for(int i = 0; i < character.abilities.Count; i++)
        {
            AbilityBar[i].sprite = character.abilities[i].icon;
        }
    }
}
