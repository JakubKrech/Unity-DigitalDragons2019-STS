using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseStateManager : MonoBehaviour {
    public enum MouseStateMachine { IDLE, CHOOSEACTION, CHOOSETARGET };
 public MouseStateMachine currentMouseState = MouseStateMachine.IDLE;
 public bool characterBeingLookedUp = false;
 public BattlefieldStateManager BattlefieldSM;
 public UIManager UIM;
 //public ButtonStateManager ButtonSM;
 public Ability clickedAbility;
 public int clickedAbilityIndex;
 public Image clickedAbilityBorder;

 // Start is called before the first frame update
 void Start () {

    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection (ray);

        switch (currentMouseState) {

            case (MouseStateMachine.IDLE):
                if (hit2D.collider && hit2D.transform.tag.Equals ("Character"))
                    lookAtCharacterStats (hit2D);
                //idle
                break;

            case (MouseStateMachine.CHOOSEACTION):
                ChooseActionState (hit2D);
                break;

            case (MouseStateMachine.CHOOSETARGET):
                ChooseTargetState (hit2D);
                break;

        }

        if (characterBeingLookedUp && hit2D.collider && hit2D.transform.tag != "Character") {
            if (BattlefieldSM.CharactersByInitiative.Any ())
                BattlefieldSM.battlefield.UIManager.updateUIToChosenCharacter (BattlefieldSM.CharactersByInitiative[0]);
            else
                BattlefieldSM.battlefield.UIManager.updateUIToChosenCharacter (BattlefieldSM.lastActiveCharacter);

            characterBeingLookedUp = false;
        }

    }

    void ChooseActionState (RaycastHit2D hit2D) {
        if (hit2D.collider != null) {
            string hit2DTag = hit2D.transform.tag;

            if (hit2DTag.Equals ("Hex")) {
                if (Input.GetMouseButtonDown (0)) {
                    HexCell clickedHex = hit2D.collider.transform.parent.gameObject.GetComponent<HexCell> ();
                    if (clickedHex.active) {
                        BattlefieldSM.CharactersByInitiative[0].MoveCharacter (clickedHex);
                        BattlefieldSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.AFTERACTION;
                        currentMouseState = MouseStateMachine.IDLE;
                    }
                }
            } else if (hit2DTag.Equals ("Character")) {
                lookAtCharacterStats (hit2D);
            }
        }
    }

    void ChooseTargetState (RaycastHit2D hit2D) {

        Character attacker = BattlefieldSM.CharactersByInitiative[0];

        if (hit2D.collider != null) {
            string hit2DTag = hit2D.transform.tag;

            if (hit2DTag.Equals ("Character")) {
                lookAtCharacterStats (hit2D);

                if (Input.GetMouseButtonDown (0) && hit2D.transform.parent.gameObject.GetComponent<Character>().hexCell.active) {
                    attacker.DeActivateNeighboringTiles();
                    attacker.DeactivateAvailableTilesForRange();

                    Character attacked = hit2D.transform.parent.gameObject.GetComponent<Character> ();
                    clickedAbility.dealDamage (attacker, attacked);
                    clickedAbility.currentCooldown = clickedAbility.cooldown;
                    clickedAbilityBorder.color = Color.white;

                    if (attacker.currentHP > 0) {
                        clickedAbility = null;
                        BattlefieldSM.CharactersByInitiative[0].ActivateMovableTiles ();
                        currentMouseState = MouseStateMachine.IDLE;
                        BattlefieldSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.AFTERACTION;
                    } else {
                        currentMouseState = MouseStateMachine.IDLE;
                        BattlefieldSM.CharactersByInitiative[0].characterState = Character.CharacterStateMachine.ENDTURN;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown (1) || Input.GetKeyDown (KeyCode.Escape)) {
            Debug.Log ("Ability Cancelled");
            attacker.DeActivateNeighboringTiles();
            attacker.DeactivateAvailableTilesForRange();

            clickedAbility = null;
            BattlefieldSM.CharactersByInitiative[0].ActivateMovableTiles ();
            UIM.AbilityBarBorders[clickedAbilityIndex].color = Color.white;
            currentMouseState = MouseStateMachine.CHOOSEACTION;
        }
    }

    void lookAtCharacterStats (RaycastHit2D hit2D) {
        if (hit2D.collider != null) {
            BattlefieldSM.battlefield.UIManager.updateUIToChosenCharacter (hit2D.collider.transform.parent.gameObject.GetComponent<Character> ());
            characterBeingLookedUp = true;
        }
    }

}