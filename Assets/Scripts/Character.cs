using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Info")]
    public bool playerControlled;
    public bool alive = true;
    public enum CharacterStateMachine{ WAIT, CHOOSEACTION, AFTERACTION, ENDTURN }
    public CharacterStateMachine characterState = CharacterStateMachine.WAIT;

    [Header("Character Stats")]
    public string charName;
    public int level, currentEXP;
    public int maxHP, currentHP;
    public int maxMana, currentMana;
    public int maxActionPoints, currentActionPoints, actionPointsRegen;
    public int strength; // bonus maxHP + damage
    public int agility; // bonus critChance + maxActionPoints
    public int power; // bonus mana + damage
    public int initiative; // starts turn faster + ??
    public int armor; // block incoming damage, 20 armor = -20% dmg taken
    public int critChance;
    public List<Ability> abilities;

    [Header("Character Components")]
    public HexCell hexCell;
    Material material;
    public Sprite characterAvatar;
    BattlefieldStateManager BSM;
    public List<Ability> abilitiesPrefabs;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
        material.renderQueue = 3006; // without it hexes dissapear while moving the camera

        InitializeAbilities();
    }

    // Start is called before the first frame update
    void Start()
    {
        BSM = GameObject.Find("BattlefieldStateManager").GetComponent<BattlefieldStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(characterState)
        {
            case(CharacterStateMachine.WAIT):
                    // idle
                break;

            case(CharacterStateMachine.CHOOSEACTION):
                    Debug.Log("Turn of character: " + this.charName);
                    ActivateMovableTiles();
                    //handle player input regarding actions to be taken by character
                    BSM.MSM.currentMouseState = MouseStateManager.MouseStateMachine.CHOOSEACTION;

                    characterState = CharacterStateMachine.WAIT;
                break;

            case(CharacterStateMachine.AFTERACTION):
                    if(currentActionPoints == 0) 
                    {
                        Debug.Log("Character " + charName + " ran out of Action Points! Turn ended!");
                        characterState = CharacterStateMachine.ENDTURN;
                    }
                    else characterState = CharacterStateMachine.CHOOSEACTION;
                break;

            case(CharacterStateMachine.ENDTURN):
                    DeActivateMovableTiles();
                    Debug.Log(charName + " has ended his turn!");
                    characterState = CharacterStateMachine.WAIT;
                    BSM.CharactersByInitiative.RemoveAt(0);
                    BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
                break;
        }
    }


    public void performAITurn()
    {
        StartCoroutine(SleepForSecondsAITurn(5));
    }

    IEnumerator SleepForSecondsAITurn(int s)
    {
        Debug.Log("AI Controlled Character " + this.charName + " completes his turn now");
        yield return new WaitForSeconds(2);
        BSM.CharactersByInitiative.RemoveAt(0);
        BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
    }

    void InitializeAbilities()
    {
         GameObject abilitiesParentObject = new GameObject();
        abilitiesParentObject.transform.parent = this.transform;
        abilitiesParentObject.name = "Abilities";

        abilities = new List<Ability>();

        for(int i = 0; i < abilitiesPrefabs.Count; i++){
            Ability ab = (Ability)Instantiate(abilitiesPrefabs[i], new Vector3(0,0,0), Quaternion.identity, abilitiesParentObject.transform);
            ab.name = ab.abilityName;
            abilities.Add(ab);
        }
    }

    void ActivateMovableTiles()
    {
        foreach (HexCell neighbor in hexCell.neighbors)
        {
            if(neighbor != null && !neighbor.occupied) 
            {
                neighbor.HexBorder.color = Color.green;
                neighbor.HexBorder.enabled = true;
                neighbor.active = true;
                
            }
            else if(neighbor != null && neighbor.occupiedBy.playerControlled){
                neighbor.HexBorder.color = Color.blue;
                neighbor.HexBorder.enabled = true;
            }
            else if(neighbor != null && !neighbor.occupiedBy.playerControlled){
                neighbor.HexBorder.color = Color.red;
                neighbor.HexBorder.enabled = true;
            }
        }
    }
    void DeActivateMovableTiles()
    {
        foreach (HexCell neighbor in hexCell.neighbors)
        {
            if(neighbor != null) 
            {
                neighbor.HexBorder.color = Color.black;
                neighbor.HexBorder.enabled = false;
                neighbor.active = false;
            }
        }
    }
    public void MoveCharacter(HexCell destination)
    {
        DeActivateMovableTiles();

        hexCell.occupied = false;
        hexCell.occupiedBy = null;

        hexCell = destination;
        
        hexCell.occupied = true;
        hexCell.occupiedBy = this;

        this.transform.position = hexCell.transform.position;

        // moving consumes one Action Point
        currentActionPoints--;

        //ActivateMovableTiles();
    }
}
