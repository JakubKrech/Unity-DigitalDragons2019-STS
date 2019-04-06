﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class Character : MonoBehaviour {
    [Header ("Character Info")]
    public bool playerControlled;
    public bool alive = true;
    public enum CharacterStateMachine { WAIT, CHOOSEACTION, AFTERACTION, ENDTURN }
    public CharacterStateMachine characterState = CharacterStateMachine.WAIT;

    [Header ("Character Stats")]
    public string charName;
    public int level, currentEXP;
    public int maxHP, currentHP;
    public int maxMana, currentMana, regenMana;
    public int maxActionPoints, currentActionPoints, actionPointsRegen;
    public int strength; // bonus maxHP + damage
    public int agility; // bonus critChance + maxActionPoints
    public int power; // bonus mana + damage
    public int initiative; // starts turn faster + ??
    //public int armor; // block incoming damage, 20 armor = -20% dmg taken
    public int critChance;
    public List<Ability> abilities;

    [Header ("Character Components")]
    public HexCell hexCell;
    //Material material;
    public Sprite characterAvatar;
    public BattlefieldStateManager BSM;
    public List<Ability> abilitiesPrefabs;
    public SpriteRenderer characterSprite;
    public DragonBones.UnityArmatureComponent characterArmature;

    /// Awake is called when the script instance is being loaded.
    void Awake () {
       // material = gameObject.GetComponentInChildren<SpriteRenderer> ().material;
       // material.renderQueue = 3006; // without it hexes dissapear while moving the camera

        InitializeAbilities ();
    }

    // Start is called before the first frame update
    void Start () {
        BSM = GameObject.Find ("BattlefieldStateManager").GetComponent<BattlefieldStateManager> ();
    }

    // Update is called once per frame
    void Update () {
        switch (characterState) {
            case (CharacterStateMachine.WAIT):
                // idle
                break;

            case (CharacterStateMachine.CHOOSEACTION):
                Debug.Log ("Turn of character: " + this.charName);
                ActivateMovableTiles ();
                //handle player input regarding actions to be taken by character
                BSM.MSM.currentMouseState = MouseStateManager.MouseStateMachine.CHOOSEACTION;

                characterState = CharacterStateMachine.WAIT;
                break;

            case (CharacterStateMachine.AFTERACTION):
                if (currentActionPoints == 0) {
                    Debug.Log ("Character " + charName + " ran out of Action Points! Turn ended!");
                    characterState = CharacterStateMachine.ENDTURN;
                } else characterState = CharacterStateMachine.CHOOSEACTION;
                break;

            case (CharacterStateMachine.ENDTURN):
                DeActivateNeighboringTiles ();
                Debug.Log (charName + " has ended his turn!");
                characterState = CharacterStateMachine.WAIT;
                BSM.CharactersByInitiative.RemoveAt (0);
                BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
                break;
        }
    }

    public void performAITurn () {
        StartCoroutine (SleepForSecondsAITurn (5));
    }

    IEnumerator SleepForSecondsAITurn (int s) {
        Debug.Log ("AI Controlled Character " + this.charName + " completes his turn now");
        yield return new WaitForSeconds (2);
        BSM.CharactersByInitiative.RemoveAt (0);
        BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
    }

    void InitializeAbilities () {
        GameObject abilitiesParentObject = new GameObject ();
        abilitiesParentObject.transform.parent = this.transform;
        abilitiesParentObject.name = "Abilities";

        abilities = new List<Ability> ();

        for (int i = 0; i < abilitiesPrefabs.Count; i++) {
            Ability ab = (Ability) Instantiate (abilitiesPrefabs[i], new Vector3 (0, 0, 0), Quaternion.identity, abilitiesParentObject.transform);
            ab.name = ab.abilityName;
            abilities.Add (ab);
        }
    }

    public void ActivateMovableTiles () {
        foreach (HexCell neighbor in hexCell.neighbors) {
            if (neighbor != null && !neighbor.occupied) {
                neighbor.HexBorder.color = Color.blue;
                neighbor.HexBorder.enabled = true;
                neighbor.active = true;

            } else if (neighbor != null && neighbor.occupiedBy.playerControlled) {
                neighbor.HexBorder.color = Color.green;
                neighbor.HexBorder.enabled = true;
            } else if (neighbor != null && !neighbor.occupiedBy.playerControlled) {
                neighbor.HexBorder.color = Color.red;
                neighbor.HexBorder.enabled = true;
            }
        }
    }
    public void DeActivateNeighboringTiles () {
        foreach (HexCell neighbor in hexCell.neighbors) {
            if (neighbor != null) {
                neighbor.HexBorder.color = Color.black;
                neighbor.HexBorder.enabled = false;
                neighbor.active = false;
            }
        }
    }

    public void ActivateAllEnemyTiles () {
        foreach (Character character in BSM.CharactersByInitiative) {
            if (!character.playerControlled) {
                character.hexCell.HexBorder.color = Color.red;
                character.hexCell.HexBorder.enabled = true;
                character.hexCell.active = true;
            }
        }
    }

    public void ActivateAvailableTilesForMelee () {
        foreach (HexCell neighbour in this.hexCell.neighbors) {
            if (neighbour.occupied) {
                if (neighbour.occupiedBy.playerControlled == true)
                    neighbour.HexBorder.color = Color.green;
                else if (neighbour.occupiedBy.playerControlled == false)
                    neighbour.HexBorder.color = Color.red;
                neighbour.HexBorder.enabled = true;
                neighbour.active = true;
            }
        }
    }
    public void ActivateAvailableTilesForRange () {
        foreach (Character character in BSM.battlefield.characters) {
            if(character.alive){
                if (character.playerControlled == true)
                    character.hexCell.HexBorder.color = Color.green;
                else if (character.playerControlled == false)
                    character.hexCell.HexBorder.color = Color.red;
                character.hexCell.HexBorder.enabled = true;
                character.hexCell.active = true;
            }
        }
    }
    public void DeactivateAvailableTilesForRange () {
        foreach (Character character in BSM.battlefield.characters) {
            character.hexCell.HexBorder.color = Color.black;
            character.hexCell.HexBorder.enabled = false;
            character.hexCell.active = false;
        }
    }

    // public void DeactivateAllTiles() {
    //     foreach (HexCell hexCell in BSM.battlefield.hexGrid.cells) {
    //         hexCell.HexBorder.color = Color.black;
    //         hexCell.HexBorder.enabled = false;
    //         hexCell.active = false;
    //     }
    // }

    public void MoveCharacter (HexCell destination) {
        DeActivateNeighboringTiles ();

        hexCell.occupied = false;
        hexCell.occupiedBy = null;

        hexCell = destination;

        hexCell.occupied = true;
        hexCell.occupiedBy = this;

        this.transform.position = hexCell.transform.position;

        this.characterSprite.sortingLayerName = "Row" + destination.y;
        //this.characterArmature.sortingLayerName = "Row" + destination.y;
        //characterArmature.animation.Stop(); //works!
        //characterArmature.animationName = "idle"; // should work

        // moving consumes one Action Point
        currentActionPoints--;

        //ActivateMovableTiles();
    }

    public void characterDies () {
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = 270;
        this.transform.rotation = Quaternion.Euler (rotationVector);

        BSM.CharactersByInitiative.Remove(this);
        BSM.UIM.updateOrderQueue(BSM.CharactersByInitiative);

        if ((BSM.checkIfAlliesLeft (BSM.battlefield.characters) && BSM.checkIfEnemiesLeft (BSM.battlefield.characters)) == false) {
            this.characterState = CharacterStateMachine.ENDTURN;
        }

    }
}