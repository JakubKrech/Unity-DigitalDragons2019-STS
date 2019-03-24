﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateManager : MonoBehaviour
{
    public enum MouseStateMachine{IDLE, CHOOSEACTION, CHOOSETARGET};
    public MouseStateMachine currentMouseState = MouseStateMachine.IDLE;
    public bool characterBeingLookedUp = false;
    public BattlefieldStateManager BattlefieldSM;
    //public ButtonStateManager ButtonSM;
    public Ability clickedAbility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        if(hit2D.collider != null)
        {
            switch(currentMouseState)
            {
                case(MouseStateMachine.IDLE):
                    //idle
                break;
                case(MouseStateMachine.CHOOSEACTION):
                    //BattlefieldSM.CharactersByInitiative[0].ActivateMovableTiles();
                    ChooseActionState(hit2D);
                break;

                case(MouseStateMachine.CHOOSETARGET):
                    //BattlefieldSM.CharactersByInitiative[0].DeActivateMovableTiles();
                    ChooseTargetState(hit2D);
                break;
            }

            if(characterBeingLookedUp && hit2D.transform.tag != "Character") 
            {
                BattlefieldSM.battlefield.UIManager.updateUIToChosenCharacter(BattlefieldSM.CharactersByInitiative[0]);
                characterBeingLookedUp = false;
            }
        }
    }

    void ChooseActionState(RaycastHit2D hit2D)
    {
        string hit2DTag = hit2D.transform.tag;

        if(hit2DTag.Equals("Hex"))
        {
            if(Input.GetMouseButtonDown(0)){
                HexCell clickedHex = hit2D.collider.transform.parent.gameObject.GetComponent<HexCell>();
                if(clickedHex.active) 
                {
                    BattlefieldSM.CharactersByInitiative[0].MoveCharacter(clickedHex);
                    BattlefieldSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.AFTERACTION;
                    currentMouseState = MouseStateMachine.IDLE;
                }
            }
        }
        else if(hit2DTag.Equals("Character"))
        {
            BattlefieldSM.battlefield.UIManager.updateUIToChosenCharacter(hit2D.collider.transform.parent.gameObject.GetComponent<Character>());
            characterBeingLookedUp = true; 
        }

        // clicking ability icons and end turn button is handled by ButtonManager object (Button script)
        // else if(hit2DTag.Equals("Ability")){ }
        // else if(hit2DTag.Equals("EndTurnButton")){ }
    }

    void ChooseTargetState(RaycastHit2D hit2D)
    {
        string hit2DTag = hit2D.transform.tag;

        if(hit2DTag.Equals("Character"))
        {
            if(Input.GetMouseButtonDown(0))
            {
                Character attacker = BattlefieldSM.CharactersByInitiative[0];
                Character attacked = hit2D.transform.parent.gameObject.GetComponent<Character>();
                Debug.Log(attacker.charName + " has attacked " + attacked.charName);
            }   
        }

        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Ability Cancelled");
            clickedAbility = null;
            BattlefieldSM.CharactersByInitiative[0].ActivateMovableTiles();
            currentMouseState = MouseStateMachine.CHOOSEACTION;
        }
    }
}
