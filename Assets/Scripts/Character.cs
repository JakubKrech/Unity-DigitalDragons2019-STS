using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool playerControlled;
    public bool alive = true;
    public string charName;
    public int level, currentEXP;
    public int maxHP, currentHP;
    public int maxMana, currentMana;
    public int maxActionPoints, currentActionPoints;
    public int strength, agility, power, initiative;
    Material material;
    public HexCell hexCell;
    public Sprite characterAvatar;
    public enum CharacterStateMachine{ WAIT, CHOOSEACTION, PERFORMACTION, ENDTURN }
    public CharacterStateMachine characterState = CharacterStateMachine.WAIT;
    BattlefieldStateManager BSM;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
        material.renderQueue = 3006; // without it hexes dissapear while moving the camera
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
}
