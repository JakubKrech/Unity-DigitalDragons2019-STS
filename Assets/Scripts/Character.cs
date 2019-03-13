using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Info")]
    public bool playerControlled;
    public bool alive = true;
    public enum CharacterStateMachine{ WAIT, CHOOSEACTION, PERFORMACTION, ENDTURN }
    public CharacterStateMachine characterState = CharacterStateMachine.WAIT;

    [Header("Character Stats")]
    public string charName;
    public int level, currentEXP;
    public int maxHP, currentHP;
    public int maxMana, currentMana;
    public int maxActionPoints, currentActionPoints;
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
                    //handle player input regarding actions to be taken by character

                    characterState = CharacterStateMachine.WAIT;
                break;

            case(CharacterStateMachine.PERFORMACTION):
                    
                break;

            case(CharacterStateMachine.ENDTURN):
                    
                break;
        }
    }


    // OnMouseDown is called when the user has pressed the mouse button while
    // over the GUIElement or Collider.
    void OnMouseDown()
    {

    }

    public void performAITurn()
    {
        // Debug.Log("AI Controlled Character " + this.charName + " completes his turn now");
        // BSM.CharactersByInitiative.RemoveAt(0);
        // BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
        StartCoroutine(SleepForSeconds(5));
    }

    IEnumerator SleepForSeconds(int s)
    {
        Debug.Log("AI Controlled Character " + this.charName + " completes his turn now");
        yield return new WaitForSeconds(5);
        BSM.CharactersByInitiative.RemoveAt(0);
        BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
    }

    void InitializeAbilities()
    {
        abilities = new List<Ability>();

        for(int i = 0; i < abilitiesPrefabs.Count; i++){
            Ability ab = (Ability)Instantiate(abilitiesPrefabs[i], new Vector3(0,0,0), Quaternion.identity, this.transform);
            ab.name = ab.abilityName;
            abilities.Add(ab);
        }
    }
}
