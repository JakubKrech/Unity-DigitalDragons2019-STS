using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattlefieldStateManager : MonoBehaviour
{
    public Battlefield battlefield;
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
                        setUIToActiveCharacter();
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
        foreach (var c in battlefield.characters)
        {
            if (c.alive) CharactersByInitiative.Add(c);
        }

        CharactersByInitiative.OrderBy(characters=>characters.initiative);
        CharactersByInitiative.Reverse();

        foreach (var c in CharactersByInitiative)
        {
            Debug.Log(c.charName + ", initiative: " + c.initiative);
        }
    }

    public void setUIToActiveCharacter()
    {
        battlefield.UIManager.CharacterName.text = CharactersByInitiative[0].charName;
        battlefield.UIManager.CharacterAvatar.sprite = CharactersByInitiative[0].characterAvatar;
        battlefield.UIManager.updateHealthBar(CharactersByInitiative[0]);
        battlefield.UIManager.updateManaBar(CharactersByInitiative[0]);
        battlefield.UIManager.updateOrderQueue(CharactersByInitiative);
        battlefield.UIManager.updateAbilityBar(CharactersByInitiative[0]);
    }
}
