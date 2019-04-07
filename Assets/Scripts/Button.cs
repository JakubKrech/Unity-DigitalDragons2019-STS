using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    //public enum ButtonStateMachine{ACTIVATED, DEACTIVATED};
    //ButtonStateMachine currentButtonState = ButtonStateMachine.DEACTIVATED;
    public MouseStateManager MSM;
    public UIManager UIM;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) clickAbilityButtonAction(0);
        if(Input.GetKeyDown(KeyCode.Alpha2)) clickAbilityButtonAction(1);
        if(Input.GetKeyDown(KeyCode.Alpha3)) clickAbilityButtonAction(2);
        if(Input.GetKeyDown(KeyCode.Alpha4)) clickAbilityButtonAction(3);
        if(Input.GetKeyDown(KeyCode.Alpha5)) clickAbilityButtonAction(4);
    }

    // On mouse click
    public void clickedAbilityButton1()
    {
        clickAbilityButtonAction(0);
    }
    public void clickedAbilityButton2()
    {
        clickAbilityButtonAction(1);
    }
    public void clickedAbilityButton3()
    {
        clickAbilityButtonAction(2);
    }
    public void clickedAbilityButton4()
    {
        clickAbilityButtonAction(3);
    }
    public void clickedAbilityButton5()
    {
        clickAbilityButtonAction(4);
    }
    // On mouse enter
    public void onMouseEnterButton1()
    {
        onPointerEnterAction(0);
    }
    public void onMouseEnterButton2()
    {
        onPointerEnterAction(1);
    }
    public void onMouseEnterButton3()
    {
        onPointerEnterAction(2);
    }
    public void onMouseEnterButton4()
    {
        onPointerEnterAction(3);
    }
    public void onMouseEnterButton5()
    {
        onPointerEnterAction(4);
    }

    // On mouse exit
    public void onMouseExitButton1()
    {
        onPointerExitAction(0);
    }
    public void onMouseExitButton2()
    {
        onPointerExitAction(1);
    }
    public void onMouseExitButton3()
    {
        onPointerExitAction(2);
    }
    public void onMouseExitButton4()
    {
        onPointerExitAction(3);
    }
    public void onMouseExitButton5()
    {
        onPointerExitAction(4);
    }

    void clickAbilityButtonAction(int abilityIndex)
    {
        if(MSM.currentMouseState == MouseStateManager.MouseStateMachine.CHOOSEACTION)
        {
            Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[abilityIndex];

            Character attacker = MSM.BattlefieldSM.CharactersByInitiative[0];

            if(!checkLocked(chosenAbility) && checkCooldown(chosenAbility) 
                && checkAP(chosenAbility) && checkMana(chosenAbility))
            {
                attacker.DeActivateNeighboringTiles();
                Debug.Log("ABILITY " + chosenAbility.name + " CLICKED SUCCESFULLY");

                Color32 borderColor = new Color32(0, 255, 0, 255);
                UIM.AbilityBarBorders[abilityIndex].color = borderColor;
                MSM.clickedAbility = chosenAbility;
                MSM.clickedAbilityIndex = abilityIndex;
                MSM.clickedAbilityBorder = UIM.AbilityBarBorders[abilityIndex];

                if(chosenAbility.range)
                    attacker.ActivateAvailableTilesForRange();
                else
                    attacker.ActivateAvailableTilesForMelee();

                MSM.currentMouseState = MouseStateManager.MouseStateMachine.CHOOSETARGET;
            }
        }
        else
        {
            Debug.Log("YOU CANT CLICK ABILITY ICON RIGHT NOW!");
        }
    }

    void onPointerEnterAction(int abilityIndex)
    {
        updateAbilityDescriptionPanel(abilityIndex);
        UIM.ADP.SetActive(true);
    }

    void onPointerExitAction(int abilityIndex)
    {
        UIM.ADP.SetActive(false);
    }
    
    bool checkLocked(Ability a)
    {
        if(a.locked) {
            Debug.Log("This ability is still locked");
            StartCoroutine(UIM.ShowFadeingMessage("This ability is still locked"));
            return true;
        }
        else return false;
    }

    bool checkCooldown(Ability a)
    {
        if(a.currentCooldown == 0) return true;
        else {
            Debug.Log("Ability still on cooldown!");
            StartCoroutine(UIM.ShowFadeingMessage("Ability still on cooldown!"));
            return false;
        }
    }

    bool checkMana(Ability a)
    {
        if(MSM.BattlefieldSM.CharactersByInitiative[0].currentMana >= a.manaCost) return true;
        else {
            Debug.Log("Not enough Mana!");
            StartCoroutine(UIM.ShowFadeingMessage("Not enough Mana!"));
            return false;
        }
    }

    bool checkAP(Ability a)
    {
        if(MSM.BattlefieldSM.CharactersByInitiative[0].currentActionPoints >= a.actionPointsCost) return true;
        else {
            Debug.Log("Not enough AP!");
            StartCoroutine(UIM.ShowFadeingMessage("Not enough AP!"));
            return false;
        }
    }

    public void clickedEndTurnButton()
    {
        if(MSM.currentMouseState == MouseStateManager.MouseStateMachine.CHOOSEACTION)
        {
            //Debug.Log("END TURN BUTTON PRESSED");
            MSM.BattlefieldSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.ENDTURN;
            MSM.currentMouseState = MouseStateManager.MouseStateMachine.IDLE;
        }
        else
        {
            Debug.Log("YOU CANT END TURN RIGHT NOW!");
            StartCoroutine(UIM.ShowFadeingMessage("YOU CANT END TURN RIGHT NOW!"));
        }
    }

    void updateAbilityDescriptionPanel(int abilityIndex)
    {
        Character activeCharacter = MSM.BattlefieldSM.CharactersByInitiative[0];

        UIM.ADPAbilityName.text = activeCharacter.abilities[abilityIndex].name;
        UIM.ADPDescription.text = activeCharacter.abilities[abilityIndex].description;
        UIM.ADPAPCost.text = "AP: " + activeCharacter.abilities[abilityIndex].actionPointsCost.ToString();
        
        UIM.ADPSkillCostValue.text = "";
        if(activeCharacter.abilities[abilityIndex].manaCost > 0) UIM.ADPSkillCostValue.text += activeCharacter.abilities[abilityIndex].manaCost.ToString() + " MP ";
        if(activeCharacter.abilities[abilityIndex].healthCost > 0) UIM.ADPSkillCostValue.text += activeCharacter.abilities[abilityIndex].healthCost.ToString() + " HP";

        int damageDealt = activeCharacter.abilities[abilityIndex].calculateDamage(activeCharacter);
        if(damageDealt > 0) UIM.ADPSkillDamageValue.text = "Damage:  " + damageDealt;
        else UIM.ADPSkillDamageValue.text = "Heal:  " + (-damageDealt);


        UIM.ADPSkillCooldown.text = "Cooldown: " + activeCharacter.abilities[abilityIndex].cooldown.ToString();
        if(activeCharacter.abilities[abilityIndex].range)
            UIM.ADPRangedMelee.text = "RANGED";
        else
            UIM.ADPRangedMelee.text = "MELEE";

        UIM.ADPSkillEffects.text = "Effects:  ";
        if(activeCharacter.abilities[abilityIndex].stunLength > 0) UIM.ADPSkillEffects.text += "STUN (" + activeCharacter.abilities[abilityIndex].stunLength + ") ";
        if(activeCharacter.abilities[abilityIndex].immobilizeLength > 0) UIM.ADPSkillEffects.text += "IMMOBILIZE (" + activeCharacter.abilities[abilityIndex].immobilizeLength + ")";
        if(activeCharacter.abilities[abilityIndex].tauntLength > 0) UIM.ADPSkillEffects.text += "TAUNT (" + activeCharacter.abilities[abilityIndex].tauntLength + ")";
    }

    public void EndScreenButton()
    {
        SceneManager.LoadScene("EndScreen");
    }

    // Help Screen/Menu window
    public void returnToMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void hideMenuHelpScreenButton()
    {
        UIM.helpScreen.SetActive(false);
    }

    public void skipBattleButton()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
