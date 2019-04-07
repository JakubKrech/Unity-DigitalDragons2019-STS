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
    public HexGrid hexGrid;
    public int hexGridWidth, hexGridHeight;

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

    void PlaceCharactersOnTheGrid()
    {
        GameObject charactersParentObject = new GameObject();
        charactersParentObject.transform.parent = this.transform;
        charactersParentObject.name = "Characters";

        PlaceCharacter(hexGrid.GetHexByCoords(1, hexGridHeight / 2), characterPrefabs[0], charactersParentObject);
        PlaceCharacter(hexGrid.GetHexByCoords(1, (hexGridHeight / 2) + 2), characterPrefabs[1], charactersParentObject);
        PlaceCharacter(hexGrid.GetHexByCoords(1, (hexGridHeight / 2) - 2), characterPrefabs[2], charactersParentObject);

        PlaceCharacter(hexGrid.GetHexByCoords(hexGridWidth - 2, hexGridHeight / 2), characterPrefabs[numberOfAllies], charactersParentObject);
    }

    void PlaceCharacter(HexCell charPosition, Character charPrefab, GameObject parent)
    {
        Character newChar = (Character)Instantiate(charPrefab, charPosition.transform.position, Quaternion.identity, parent.transform);
        newChar.name = charPrefab.charName;
        newChar.hexCell = charPosition;
        newChar.characterSprite.sortingLayerName = "Row" + newChar.hexCell.y;
        newChar.characterArmature.sortingLayerName = "Row" + newChar.hexCell.y;
        characters.Add(newChar);

        charPosition.occupied = true;
        charPosition.occupiedBy = newChar;
    }

}
