using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;

public class Character : MonoBehaviour {
    [Header ("Character Info")]
    public bool playerControlled;
    public bool alive = true;
    public enum CharacterStateMachine { WAIT, CHOOSEACTION, AFTERACTION, MOVING, ENDTURN }
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
    public int stunnedFor, immobilizedFor, tauntedFor;
    public Character tauntTarget;
    public List<Ability> abilities;

    [Header ("Character Components")]
    public HexCell hexCell;
    //Material material;
    public Sprite characterAvatar;
    public BattlefieldStateManager BSM;
    public List<Ability> abilitiesPrefabs;
    public SpriteRenderer characterSprite;
    public DragonBones.UnityArmatureComponent characterArmature;
    [Header ("Other")]
    public Coroutine movingCoroutine;

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
                if (stunnedFor > 0) {
                    Debug.Log ("Character " + charName + " is stunned and skips this turn.");
                    StartCoroutine (BSM.UIM.ShowFadeingMessage ("Character " + charName + " is stunned and skips this turn."));
                    characterState = CharacterStateMachine.ENDTURN;
                } else {
                    Debug.Log ("Turn of character: " + this.charName);
                    ActivateMovableTiles ();
                    //handle player input regarding actions to be taken by character
                    BSM.MSM.currentMouseState = MouseStateManager.MouseStateMachine.CHOOSEACTION;

                    characterState = CharacterStateMachine.WAIT;
                }
                break;

            case (CharacterStateMachine.AFTERACTION):
                if (currentActionPoints == 0) {
                    Debug.Log ("Character " + charName + " ran out of Action Points! Turn ended!");
                    characterState = CharacterStateMachine.ENDTURN;
                } else characterState = CharacterStateMachine.CHOOSEACTION;
                break;

            case (CharacterStateMachine.MOVING):
                //idle while moving

                break;
            case (CharacterStateMachine.ENDTURN):
                DeActivateNeighboringTiles ();
                UpdateCharactersStats ();
                Debug.Log (charName + " has ended his turn!");
                characterState = CharacterStateMachine.WAIT;
                BSM.CharactersByInitiative.Remove (this); //RemoveAt (0);
                BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
                break;
        }
    }

    public void performAITurn () {
        if (charName == "Undead Wolf") StartCoroutine(performUndeadWolfAILogic ());
        else Debug.Log ("AI Logic not found");

        // UpdateCharactersStats ();
        // BSM.CharactersByInitiative.Remove (this); //RemoveAt (0);
        // BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
    }

    public IEnumerator SleepForSeconds (int s) {
        //Debug.Log ("AI Controlled Character " + this.charName + " completes his turn now");
        yield return new WaitForSeconds (s);
        //BSM.CharactersByInitiative.RemoveAt (0);
        //BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
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
        this.hexCell.HexBorder.color = Color.black;
        this.hexCell.HexBorder.enabled = false;
        this.hexCell.active = false;
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
        this.hexCell.HexBorder.color = Color.green;
        this.hexCell.HexBorder.enabled = true;
        this.hexCell.active = true;
    }
    public void ActivateAvailableTilesForRange () {
        foreach (Character character in BSM.battlefield.characters) {
            if (character.alive) {
                if (character.playerControlled == true)
                    character.hexCell.HexBorder.color = Color.green;
                else if (character.playerControlled == false)
                    character.hexCell.HexBorder.color = Color.red;
                character.hexCell.HexBorder.enabled = true;
                character.hexCell.active = true;
            }
        }
        this.hexCell.HexBorder.color = Color.green;
        this.hexCell.HexBorder.enabled = true;
        this.hexCell.active = true;
    }
    public void DeactivateAvailableTilesForRange () {
        foreach (Character character in BSM.battlefield.characters) {
            character.hexCell.HexBorder.color = Color.black;
            character.hexCell.HexBorder.enabled = false;
            character.hexCell.active = false;
        }
        this.hexCell.HexBorder.color = Color.black;
        this.hexCell.HexBorder.enabled = false;
        this.hexCell.active = false;
    }

    // public void DeactivateAllTiles() {
    //     foreach (HexCell hexCell in BSM.battlefield.hexGrid.cells) {
    //         hexCell.HexBorder.color = Color.black;
    //         hexCell.HexBorder.enabled = false;
    //         hexCell.active = false;
    //     }
    // }

    // public void MoveCharacter (HexCell destination) {
    //     if (this.immobilizedFor == 0) {
    //         DeActivateNeighboringTiles ();

    //         hexCell.occupied = false;
    //         hexCell.occupiedBy = null;

    //         hexCell = destination;

    //         hexCell.occupied = true;
    //         hexCell.occupiedBy = this;

    //         this.transform.position = hexCell.transform.position;

    //         this.characterSprite.sortingLayerName = "Row" + destination.y;
    //         this.characterArmature.sortingLayerName = "Row" + destination.y;
    //         //characterArmature.animation.Stop(); //works!
    //         //characterArmature.animationName = "idle"; // should work

    //         // moving consumes one Action Point
    //         currentActionPoints--;

    //         //ActivateMovableTiles();
    //     } else {
    //         StartCoroutine (BSM.UIM.ShowFadeingMessage ("Character is immobilized for " + this.immobilizedFor + " turns"));
    //     }
    // }

    public IEnumerator MoveCharacter (HexCell destination) {
        if (this.immobilizedFor == 0) {

            currentActionPoints--;
            DeActivateNeighboringTiles ();

            hexCell.occupied = false;
            hexCell.occupiedBy = null;

            hexCell = destination;

            hexCell.occupied = true;
            hexCell.occupiedBy = this;

            float distanceToMove = Vector3.Distance (this.transform.position, destination.transform.position);
            float startTime = Time.time;
            float moveSpeed = 1.0f;

            while (this.transform.position != destination.transform.position) {

                float distCovered = (Time.time - startTime) * moveSpeed;
                float fracJourney = distCovered / distanceToMove;
                this.transform.position = Vector3.Lerp (this.transform.position, destination.transform.position, fracJourney);

                yield return null;
            }

            //this.transform.position = hexCell.transform.position;

            this.characterSprite.sortingLayerName = "Row" + destination.y;
            this.characterArmature.sortingLayerName = "Row" + destination.y;
            //characterArmature.animation.Stop(); //works!
            //characterArmature.animationName = "idle"; // should work

        } else {
            StartCoroutine (BSM.UIM.ShowFadeingMessage ("Character is immobilized for " + this.immobilizedFor + " turns"));
        }

        if(playerControlled){
            BSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.AFTERACTION;
        }
    }

    public void characterDies () {
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = 270;
        this.transform.rotation = Quaternion.Euler (rotationVector);

        BSM.CharactersByInitiative.Remove (this);
        BSM.UIM.updateOrderQueue (BSM.CharactersByInitiative);

        if ((BSM.checkIfAlliesLeft (BSM.battlefield.characters) && BSM.checkIfEnemiesLeft (BSM.battlefield.characters)) == false) {
            this.characterState = CharacterStateMachine.ENDTURN;
        }

        //this.characterArmature.animation.Stop (); // swap to death animation to be played once in the future
    }

    void UpdateCharactersStats () {

        // Update Action Points
        currentActionPoints += actionPointsRegen;
        if (currentActionPoints > maxActionPoints) currentActionPoints = maxActionPoints;

        // Update Mana
        currentMana += regenMana + power;
        if (currentMana > maxMana) currentMana = maxMana;

        // Update skills cooldowns
        foreach (Ability ability in abilities) {
            if (ability.currentCooldown > 0) ability.currentCooldown -= 1;
        }

        // Update effects
        if (stunnedFor > 0) --stunnedFor;
        if (immobilizedFor > 0) --immobilizedFor;
        if (tauntedFor > 0) --tauntedFor;

    }

    // AI STUFF

    IEnumerator performUndeadWolfAILogic () {

        // abilities: 0 - bite, 1 - howl, 2 - fear
        
        yield return new WaitForSeconds(0.75f);

        if(stunnedFor == 0){
            Character target = null;
            if (tauntedFor > 0) target = tauntTarget;
            else {
                Character lowestHPCharacter = null;
                foreach (Character character in BSM.battlefield.characters) {
                    if (lowestHPCharacter == null || character.currentHP < lowestHPCharacter.currentHP)
                        if (character.alive) lowestHPCharacter = character;
                }
                target = lowestHPCharacter;
            }

            while (target != null && currentActionPoints > 0) {
                if(abilities[2].currentCooldown == 0 && currentActionPoints >= abilities[2].actionPointsCost){
                    Character targetToBeStunned = target;
                    foreach (Character character in BSM.battlefield.characters)
                    {
                        if (character.playerControlled && targetToBeStunned.currentHP < character.currentHP) targetToBeStunned = character;
                    }
                    abilities[2].dealDamage (this, targetToBeStunned);
                    yield return new WaitForSeconds(1.5f);
                    continue;
                }
                else if (immobilizedFor > 0 && abilities[1].currentCooldown == 0 && currentActionPoints >= abilities[1].actionPointsCost) {
                    foreach (Character character in BSM.battlefield.characters) {
                        if (character.playerControlled && character.alive) {
                            abilities[1].dealDamage (this, character);
                            yield return new WaitForSeconds(1.0f);
                        }
                    }
                    continue;
                } else if (target.alive) {
                    bool neighboringTarget = false;
                    foreach (HexCell cell in hexCell.neighbors) {
                        if (cell != null && cell.occupied == true && cell.occupiedBy.charName == target.charName)
                            neighboringTarget = true;
                    }
                    if (neighboringTarget == true)
                        if (abilities[0].currentCooldown == 0) {
                            abilities[0].dealDamage (this, target);
                            yield return new WaitForSeconds(1.5f);
                        }
                        else break;
                    else {
                        HexCell chosenHexCell = moveTowardsTarget (this.hexCell, target.hexCell);
                        if(chosenHexCell.occupied == true){
                            if (abilities[0].currentCooldown == 0) {
                                abilities[0].dealDamage (this, chosenHexCell.occupiedBy);
                                yield return new WaitForSeconds(1.5f);
                            }
                            else break;
                        }
                        else{
                            StartCoroutine (MoveCharacter (chosenHexCell));
                        }
                        yield return new WaitForSeconds(1.0f);
                    }
                    continue;
                }

                break;
            }    
        }
        else{
            StartCoroutine (BSM.UIM.ShowFadeingMessage ("Character " + charName + " is stunned and skips this turn."));
        }

        UpdateCharactersStats ();
        BSM.CharactersByInitiative.Remove (this); //RemoveAt (0);
        BSM.currentState = BattlefieldStateManager.BattlefieldStateMachine.PERFORMTURN;
    }

    HexCell moveTowardsTarget (HexCell from, HexCell to) {

        //if(movingCoroutine != null) StopCoroutine(movingCoroutine);

        Vector3 f = from.transform.position;
        Vector3 t = to.transform.position;

        HexCell chosenHexCell = null;

        if (t.y == f.y) {
            if (t.x < f.x) chosenHexCell = hexCell.neighbors[(int) HexDirection.W];
            else chosenHexCell = hexCell.neighbors[(int) HexDirection.E];
        } else if (t.y > f.y) {
            if (t.x < f.x) chosenHexCell = hexCell.neighbors[(int) HexDirection.NW];
            else chosenHexCell = hexCell.neighbors[(int) HexDirection.NE];
        } else if (t.y < f.y) {
            if (t.x < f.x) chosenHexCell = hexCell.neighbors[(int) HexDirection.SW];
            else chosenHexCell = hexCell.neighbors[(int) HexDirection.SE];
        } else Debug.Log ("Character::moveTowardsTarget couldnt find hex to move to");

        return chosenHexCell;

        // if (t.y == f.y) {
        //     if (t.x < f.x) movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.W]));
        //     else movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.E]));
        // } else if (t.y > f.y) {
        //     if (t.x < f.x) movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.NW]));
        //     else movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.NE]));
        // } else if (t.y < f.y) {
        //     if (t.x < f.x) movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.SW]));
        //     else movingCoroutine = StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.SE]));
        // } else Debug.Log ("Character::moveTowardsTarget couldnt find hex to move to");

        // if (t.y == f.y) {
        //     if (t.x < f.x) StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.W]));
        //     else StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.E]));
        // } else if (t.y > f.y) {
        //     if (t.x < f.x) StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.NW]));
        //     else StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.NE]));
        // } else if (t.y < f.y) {
        //     if (t.x < f.x) StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.SW]));
        //     else StartCoroutine (MoveCharacter (hexCell.neighbors[(int) HexDirection.SE]));
        // } else Debug.Log ("Character::moveTowardsTarget couldnt find hex to move to");

        // if (t.y == f.y) {
        //     if (t.x < f.x) MoveCharacter (hexCell.neighbors[(int) HexDirection.W]);
        //     else MoveCharacter (hexCell.neighbors[(int) HexDirection.E]);
        // } else if (t.y > f.y) {
        //     if (t.x < f.x) MoveCharacter (hexCell.neighbors[(int) HexDirection.NW]);
        //     else MoveCharacter (hexCell.neighbors[(int) HexDirection.NE]);
        // } else if (t.y < f.y) {
        //     if (t.x < f.x) MoveCharacter (hexCell.neighbors[(int) HexDirection.SW]);
        //     else MoveCharacter (hexCell.neighbors[(int) HexDirection.SE]);
        // } else Debug.Log ("Character::moveTowardsTarget couldnt find hex to move to");

    }
}