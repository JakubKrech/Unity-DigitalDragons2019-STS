using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattlefieldStateManager : MonoBehaviour
{
    public Battlefield battlefield;
    public MouseStateManager MSM;
    public enum BattlefieldStateMachine{ PRESTART, PERFORMTURN, WAIT, ENDTURN, RESULTSCREEN }
    public BattlefieldStateMachine currentState = BattlefieldStateMachine.WAIT;
    public List<Character> CharactersByInitiative; //CharactersByInitiative[0] is currently active character
    //int activeCharacterNumber = 0;
    //public Character activeCharacter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case(BattlefieldStateMachine.WAIT):
                    // idle
                break;

            case(BattlefieldStateMachine.PRESTART):
                    FillActiveCharsByInitiative();
                    currentState = BattlefieldStateMachine.PERFORMTURN;
                break;

            case(BattlefieldStateMachine.PERFORMTURN):
                    if(CharactersByInitiative.Any()){
                        battlefield.UIManager.updateOrderQueue(CharactersByInitiative);
                        battlefield.UIManager.updateUIToChosenCharacter(CharactersByInitiative[0]);
                        currentState = BattlefieldStateMachine.WAIT;
                        
                        if(CharactersByInitiative[0].playerControlled){
                            CharactersByInitiative[0].characterState = Character.CharacterStateMachine.CHOOSEACTION;
                        }
                        else{
                            CharactersByInitiative[0].performAITurn();
                        }
                        
                    }
                    else currentState = BattlefieldStateMachine.ENDTURN;
                break;

            case(BattlefieldStateMachine.ENDTURN):
                    Debug.Log("-----NEW TURN HAS STARTED-----");
                    UpdateCharactersStats();
                    FillActiveCharsByInitiative();
                    // wypisanie czegos typu "TURN 5" na srodku ekranu?
                    currentState = BattlefieldStateMachine.PERFORMTURN;
                break;

            case(BattlefieldStateMachine.RESULTSCREEN):

                break;
        }
    }

    public void StartTurnLogic()
    {
        currentState = BattlefieldStateMachine.PRESTART;
    }

    void FillActiveCharsByInitiative()
    {
        foreach (Character c in battlefield.characters)
        {
            if (c.alive) CharactersByInitiative.Add(c);
        }

        CharactersByInitiative.OrderBy(characters=>characters.initiative);
        CharactersByInitiative.Reverse();

        foreach (Character c in CharactersByInitiative)
        {
            Debug.Log(c.charName + ", initiative: " + c.initiative);
        }
    }

    void UpdateCharactersStats()
    {
        foreach (Character c in battlefield.characters)
        {
            // Update Action Points
            c.currentActionPoints += c.actionPointsRegen;
            if(c.currentActionPoints > c.maxActionPoints) c.currentActionPoints = c.maxActionPoints;
        }
    }

}
