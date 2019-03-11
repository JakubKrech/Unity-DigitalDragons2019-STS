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
    //public Sprite backgroundImage;

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

}
