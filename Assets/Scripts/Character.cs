using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool playerControlled;
    public bool alive = true;
    public string charName;
    public int level, maxHP, currentHP, maxMana, currentMana, actionPoints;
    public int strength, agility, power, initiative;
    Material material;
    public HexCell hexCell;
    public enum CharacterStateMachine{ WAIT, CHOOSEACTION, PERFORMACTION, ENDTURN }
    public CharacterStateMachine characterState = CharacterStateMachine.WAIT;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
        material.renderQueue = 3006; // without it hexes dissapear while moving the camera
    }

    // Start is called before the first frame update
    void Start()
    {

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

    }
}
