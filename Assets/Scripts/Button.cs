using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    //public enum ButtonStateMachine{ACTIVATED, DEACTIVATED};
    //ButtonStateMachine currentButtonState = ButtonStateMachine.DEACTIVATED;
    public MouseStateManager MSM;

    public void clickedAbilityIcon()
    {
        if(MSM.currentMouseState == MouseStateManager.MouseStateMachine.CHOOSEACTION)
        {
            Debug.Log("ABILITY ICON CLICKED SUCCESFULLY");
            // TO DO
        }
        else
        {
            Debug.Log("YOU CANT CLICK ABILITY ICON RIGHT NOW!");
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
