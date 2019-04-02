using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void clickAbilityButtonAction(int abilityIndex)
    {
        if(MSM.currentMouseState == MouseStateManager.MouseStateMachine.CHOOSEACTION)
        {
            Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[abilityIndex];

            if(!checkLocked(chosenAbility) && checkCooldown(chosenAbility) 
                && checkAP(chosenAbility) && checkMana(chosenAbility))
            {
                MSM.BattlefieldSM.CharactersByInitiative[0].DeActivateMovableTiles();
                Debug.Log("ABILITY " + chosenAbility.name + " CLICKED SUCCESFULLY");

                Color32 borderColor = new Color32(0, 255, 0, 255);
                UIM.AbilityBarBorders[abilityIndex].color = borderColor;
                MSM.clickedAbility = chosenAbility;
                MSM.clickedAbilityIndex = abilityIndex;
                MSM.clickedAbilityBorder = UIM.AbilityBarBorders[abilityIndex];
                MSM.currentMouseState = MouseStateManager.MouseStateMachine.CHOOSETARGET;
            }
        }
        else
        {
            Debug.Log("YOU CANT CLICK ABILITY ICON RIGHT NOW!");
        }
    }
    
    bool checkLocked(Ability a)
    {
        if(a.locked) {
            Debug.Log("This ability is still locked");
            return true;
        }
        else return false;
    }

    bool checkCooldown(Ability a)
    {
        if(a.currentCooldown == 0) return true;
        else {
            Debug.Log("Ability still on cooldown!");
            return false;
        }
    }

    bool checkMana(Ability a)
    {
        if(MSM.BattlefieldSM.CharactersByInitiative[0].currentMana >= a.manaCost) return true;
        else {
            Debug.Log("Not enough Mana!");
            return false;
        }
    }

    bool checkAP(Ability a)
    {
        if(MSM.BattlefieldSM.CharactersByInitiative[0].currentActionPoints >= a.actionPointsCost) return true;
        else {
            Debug.Log("Not enough AP!");
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
        }
    }

}
