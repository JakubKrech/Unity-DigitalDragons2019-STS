using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    //public enum ButtonStateMachine{ACTIVATED, DEACTIVATED};
    //ButtonStateMachine currentButtonState = ButtonStateMachine.DEACTIVATED;
    public MouseStateManager MSM;

    public void clickedAbilityButton1()
    {
        Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[0];
        clickAbilityButtonAction(chosenAbility);
    }
    public void clickedAbilityButton2()
    {
        Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[1];
        clickAbilityButtonAction(chosenAbility);
    }
    public void clickedAbilityButton3()
    {
        Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[2];
        clickAbilityButtonAction(chosenAbility);
    }
    public void clickedAbilityButton4()
    {
        Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[3];
        clickAbilityButtonAction(chosenAbility);
    }
    public void clickedAbilityButton5()
    {
        Ability chosenAbility = MSM.BattlefieldSM.CharactersByInitiative[0].abilities[4];
        clickAbilityButtonAction(chosenAbility);
    }

    void clickAbilityButtonAction(Ability a)
    {
        if(MSM.currentMouseState == MouseStateManager.MouseStateMachine.CHOOSEACTION)
        {
            if(!checkLocked(a) && checkCooldown(a) && checkAP(a) && checkMana(a))
            {
                Debug.Log("ABILITY " + a.name + " CLICKED SUCCESFULLY");

                MSM.clickedAbility = a;
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
