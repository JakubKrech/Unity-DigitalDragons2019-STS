using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattlefieldStateManager : MonoBehaviour
{
    public Battlefield battlefield;
    public MouseStateManager MSM;
    public UIManager UIM;
    public enum BattlefieldStateMachine{ PRESTART, PERFORMTURN, WAIT, ENDTURN, RESULTSCREEN }
    public BattlefieldStateMachine currentState = BattlefieldStateMachine.WAIT;
    public List<Character> CharactersByInitiative; //CharactersByInitiative[0] is currently active character
    //int activeCharacterNumber = 0;
    //public Character activeCharacter;
    int currentTurn = 1;
    public Character lastActiveCharacter;

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
                    if(CharactersByInitiative.Any() && checkIfEnemiesLeft(battlefield.characters) && checkIfAlliesLeft(battlefield.characters)){
                        battlefield.UIManager.updateOrderQueue(CharactersByInitiative);
                        battlefield.UIManager.updateUIToChosenCharacter(CharactersByInitiative[0]);
                        lastActiveCharacter = CharactersByInitiative[0];
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
                    if(checkIfEnemiesLeft(battlefield.characters) && checkIfAlliesLeft(battlefield.characters)){
                        Debug.Log("<color=green>-----NEW TURN HAS STARTED-----</color>");
                        UIM.updateTurnCounter(++currentTurn);
                        UpdateCharactersStats();
                        FillActiveCharsByInitiative();
                        currentState = BattlefieldStateMachine.PERFORMTURN;
                    }
                    else currentState = BattlefieldStateMachine.RESULTSCREEN;
                break;

            case(BattlefieldStateMachine.RESULTSCREEN):
                    MSM.currentMouseState = MouseStateManager.MouseStateMachine.IDLE;
                    Debug.Log("GAME FINISHED");
                    
                    if(!checkIfEnemiesLeft(battlefield.characters)){
                        UIM.battleOutcome.text = "VICTORY";
                        UIM.battleOutcomeDescription.text = "All enemies has been defeated!";
                    }
                    else{
                        UIM.battleOutcome.text = "LOSE";
                        UIM.battleOutcomeDescription.text = "You have been defeated!";
                    }
                    UIM.ResultScreenWindow.SetActive(true);

                    currentState = BattlefieldStateMachine.WAIT;
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

        CharactersByInitiative.Sort((x, y) => x.initiative.CompareTo(y.initiative));
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

            // Update Mana
            c.currentMana += c.regenMana + c.power;
            if(c.currentMana > c.maxMana) c.currentMana = c.maxMana;

            // Update skills cooldowns
            foreach (Ability ability in c.abilities)
            {
                ability.currentCooldown -= 1;
            }
        }
    }

    public bool checkIfEnemiesLeft(List<Character> list)
    {
        foreach (Character character in list)
        {
            if(character.playerControlled == false && character.alive) return true;
        }
        return false;
    }

    public bool checkIfAlliesLeft(List<Character> list)
    {
        foreach (Character character in list)
        {
            if(character.playerControlled == true && character.alive) return true;
        }
        return false;
    }
}
