using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    [Header("HexGrid Settings")]
    public HexGrid hexGridPrefab;
    private HexGrid hexGrid;
    public int hexGridWidth, hexGridHeight;

    [Header("Background Settings")]
    public GameObject backgroundPrefab;
    public Sprite backgroundImage;

    [Header("Characters Settings")]
    public int numberOfAllies = 3;
    public Character[] characterPrefabs;
    public List<Character> characters;
    //private SpriteRenderer spriteRend;


    /// Awake is called when the script instance is being loaded
    void Awake()
    {
        hexGrid = (HexGrid)Instantiate(hexGridPrefab, new Vector3(0,0,0), Quaternion.identity, this.transform);
        hexGrid.Create(hexGridWidth, hexGridHeight);
        Vector3 hexGridCenter = hexGrid.GetGridCenteredPosition();
        hexGrid.transform.position = new Vector3(-hexGridCenter.x, -hexGridCenter.y, 0);
        hexGrid.name = "HexGrid";

        GameObject background = (GameObject)Instantiate(backgroundPrefab, new Vector3(0,0,5f), Quaternion.identity, this.transform);
        background.name = "Background";
        SpriteRenderer backgroundSR = background.GetComponent<SpriteRenderer>();
        //backgroundSR.sprite = backgroundImage;

        // Vector3 charPosition = hexGrid.GetHexByCoords(3, 5).transform.position;
        // Character char1 = (Character)Instantiate(characterPrefabs[0], charPosition, Quaternion.identity, this.transform);
        // char1.name = char1.charName;


        PlaceCharactersOnTheGrid();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceCharactersOnTheGrid()
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            Vector3 charPosition = hexGrid.GetHexByCoords(Random.Range(0, 1), Random.Range(0, hexGridHeight - 1)).transform.position;
            Character newChar = (Character)Instantiate(characterPrefabs[0], charPosition, Quaternion.identity, this.transform);

            Color newColor = new Color(
                Random.Range(0, 1f), 
                Random.Range(0, 1f), 
                Random.Range(0, 1f)
            );

            newChar.GetComponentInChildren<SpriteRenderer>().color = newColor;
            characters.Add(newChar);
        }

    }
}
