using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    1. Generating Battlefield
        - HexGrid
        - Background
        - UI Canvas with UI elements
        - Placing characters in their default positions
    2. Phase of choosing player controlled characters starting locations
    3. Start of game loop
        while(atLeastOneAllyAlive && atLeastOneEnemyAlive)
        {
            - characters perform actions in order set by their initiative stats
            - characters have Action Points (AP) pool - each turn they are given some ammount of AP, every action (move, attack, spell) costs AP (and sometimes other resources, like mana or health)
            - moving one hex costs one AP by default
            - character may store unused AP for next turns, but he can store only as many as his MAX AP allows


        }

    Current RenderQueue:
        3001 - background - BattlefieldManager.InitializeBackground()
        3002 - border HexCell
        3003 - inner HexCell
        3005 - text on HexCell
*/

public class Battlefield : MonoBehaviour
{
    [Header("HexGrid Settings")]
    public HexGrid hexGridPrefab;
    private HexGrid hexGrid;
    public int hexGridWidth, hexGridHeight;

    [Header("Background Settings")]
    public GameObject backgroundPrefab;
    private GameObject background;
    public Sprite backgroundImage;

    // [Header("UI Settings")]
    // public Canvas canvasPrefab;
    // private Canvas canvas;
    // private Text gameStatePanelText;
    // private Text buttonMiddleBottomText;
    // private Button buttonMiddleBottom;

    [Header("Characters Settings")]
    public int numberOfAllies = 3;
    public Character[] characterPrefabs;
    public List<Character> characters;
    //private SpriteRenderer spriteRend;

    [Header("Elements")]
    public BattlefieldStateManager BSM;
    public UIManager UIManager;
    
    

    /// Awake is called when the script instance is being loaded
    void Awake()
    {
        InitializeHexGrid();
        Debug.Log("HexGrid initialized");
        InitializeBackground();
        Debug.Log("Background initialized");
        //InitializeUICanvas();
        //Debug.Log("UICanvas initialized");
        PlaceCharactersOnTheGrid();
        Debug.Log("Charaters initialized and placed on the default positions");
    }

    // Start is called before the first frame update
    void Start()
    {
       Debug.Log("Battlefield Initialization Completed");
       BSM.StartTurnLogic();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
        // split turn state manager logic into other file 
        // zrobić oddzielne state managery dla kazdego charactera, domyślnie mające wait i wlaczane z glownego
        // jak glowny wlaczy jakis character, to siebie wlacza na wait

        // switch(currentState)
        // {
        //     case(GameStateMachine.PRESTART):
        //             UpdateActiveCharsByInitiative();
        //             currentState = GameStateMachine.PERFORMTURN;
        //         break;

        //     case(GameStateMachine.PERFORMTURN):
        //             ChooseActiveCharacter();

        //             if(activeCharacter.playerControlled) PerformCharacterTurn(activeCharacter);
        //             else PerformAITurn(activeCharacter);

        //             //currentState = GameStateMachine.WAIT;
        //         break;

        //     case(GameStateMachine.WAIT):
        //             // idle
        //         break;

        //     case(GameStateMachine.ENDTURN):
        //             activeCharacterNumber = 0;
        //             UpdateActiveCharsByInitiative();
        //             currentState = GameStateMachine.PERFORMTURN;
        //         break;

        //     case(GameStateMachine.RESULTSCREEN):

        //         break;
        // }
    }

    // BATTLEFIELD INITIALIZATION FUNCTIONS

    void InitializeHexGrid()
    {
        hexGrid = (HexGrid)Instantiate(hexGridPrefab, new Vector3(0,0,0), Quaternion.identity, this.transform);
        hexGrid.Create(hexGridWidth, hexGridHeight);
        Vector3 hexGridCenter = hexGrid.GetGridCenteredPosition();
        hexGrid.transform.position = new Vector3(-hexGridCenter.x, -hexGridCenter.y, 0);
        hexGrid.name = "HexGrid";
    }

    void InitializeBackground()
    {
        background = (GameObject)Instantiate(backgroundPrefab, new Vector3(0,0,0.9f), Quaternion.identity, this.transform);
        background.name = "Background";
        SpriteRenderer backgroundSR = background.GetComponent<SpriteRenderer>();
        backgroundSR.material.renderQueue = 3001;
    }

    // void InitializeUICanvas()
    // {
    //     canvas = (Canvas)Instantiate(canvasPrefab, this.transform.position, Quaternion.identity, this.transform);
    //     canvas.name = "UI Canvas";
        
    //     gameStatePanelText = GameObject.Find("GameStatePanelText").GetComponent<Text>();
    //     gameStatePanelText.text = "Jakis tekst, np mini opisik fabularny bosseła asdasasdasdasdasd";
        
    //     buttonMiddleBottomText = GameObject.Find("ButtonMiddleBottomText").GetComponent<Text>();
    //     buttonMiddleBottomText.text = "Click to start battle!";

    //     buttonMiddleBottom = GameObject.Find("ButtonMiddleBottom").GetComponent<Button>();
    // }

    void PlaceCharactersOnTheGrid()
    {
        PlaceCharacter(hexGrid.GetHexByCoords(1, hexGridHeight / 2), characterPrefabs[0], "Ally Character 1");
        PlaceCharacter(hexGrid.GetHexByCoords(1, (hexGridHeight / 2) + 2), characterPrefabs[1], "Ally Character 2");
        PlaceCharacter(hexGrid.GetHexByCoords(1, (hexGridHeight / 2) - 2), characterPrefabs[2], "Ally Character 3");

        PlaceCharacter(hexGrid.GetHexByCoords(hexGridWidth - 2, hexGridHeight / 2), characterPrefabs[numberOfAllies], "Enemy Character 1");
    }

    void PlaceCharacter(HexCell charPosition, Character charPrefab, string charName)
    {
        Character newChar = (Character)Instantiate(charPrefab, charPosition.transform.position, Quaternion.identity, this.transform);
        newChar.name = charName;
        newChar.hexCell = charPosition;
        characters.Add(newChar);

        charPosition.occupied = true;
        charPosition.occupiedBy = newChar;
    }

    // GAMEPLAY LOOP FUNCTIONS

    // void UpdateActiveCharsByInitiative()
    // {
    //     foreach (var c in characters)
    //     {
    //         activeCharsByInitiative.Add(c);
    //     }

    //     activeCharsByInitiative.OrderBy(characters=>characters.initiative);
    //     activeCharsByInitiative.Reverse();

    //     // foreach (var c in activeCharsByInitiative)
    //     // {
    //     //     Debug.Log(c.charName + " " + c.initiative);
    //     // }
    // }

    // void PerformCharacterTurn(Character character)
    // {
    //     foreach (var hex in character.hexCell.neighbors)
    //     {
    //         hex.sr.color = Color.blue;
    //     }
    //     Debug.Log("TURN OF: " + character.charName);

        

    //     // foreach (var hex in character.hexCell.neighbors)
    //     // {
    //     //     hex.sr.color = Color.black;
    //     // }
    // }

    // void PerformAITurn(Character character)
    // {
    //     Debug.Log("AI " + character.charName + " WILL PERFORM TURN NOW");
    //     activeCharacterNumber++;
    //     currentState = GameStateMachine.PERFORMTURN;
    // }

    // void ChooseActiveCharacter()
    // {
    //     if(activeCharacterNumber > activeCharsByInitiative.Count) currentState = GameStateMachine.ENDTURN;
    //     else activeCharacter = activeCharsByInitiative[activeCharacterNumber];
    // }

    // IEnumerator PerformAction()
    // {

    //     yield return null;
    // }
}
