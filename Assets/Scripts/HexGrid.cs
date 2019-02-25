using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMeshPro

public class HexGrid : MonoBehaviour
{
    public HexCell hexPrefab;
    public int height = 7; // height i width maja byc nieparzyste
    public int width = 11;

    [SerializeField]
    public List<HexCell> cells;
    private float xOffset = 1.764f;
    private float yOffset = 1.528f;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        cells = new List<HexCell>();

        GenerateGrid();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
   
    }

    void GenerateGrid()
    {
        for (int y = height / 2; y > 0 ; y--)
        {
            for (int x = 0; x < width - y; x++)
            {
                float x_row_offset = y * xOffset/2;

                float xPos = x * xOffset + x_row_offset;
                
                //GameObject hex = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, (height / 2 - y) * yOffset, 0), Quaternion.identity);
                HexCell hex = (HexCell)Instantiate(hexPrefab, new Vector3(xPos, (height / 2 - y) * yOffset, 0), Quaternion.identity);

                int xCoord = x;
                int yCoord = (height / 2 - y);

                hex.name = "Hex (" + xCoord + ", " + yCoord + ")";
                hex.transform.SetParent(this.transform);

                TextMesh tm = hex.GetComponentInChildren<TextMesh>();
                tm.text = "(" + xCoord + ", " + yCoord + ")";

                hex.SetCoordinates(xCoord, yCoord);

                cells.Add(hex);

                Update_E_W_Neighbors(hex, width - y, height);
                Update_SE_NW_Neighbors(hex, width - y, height);
                Update_SW_NE_Neighbors(hex, width - y, height);
            }
        }

        for (int x = 0; x < width; x++)
        {
            float xPos = x * xOffset;

            HexCell hex = (HexCell)Instantiate(hexPrefab, new Vector3(xPos, (height / 2) * yOffset, 0), Quaternion.identity);

            int xCoord = x;
            int yCoord = (height / 2) ;

            hex.name = "Hex (" + xCoord + ", " + yCoord + ")";
            hex.transform.SetParent(this.transform);

            TextMesh tm = hex.GetComponentInChildren<TextMesh>();
            tm.text = "(" + xCoord + ", " + yCoord + ")";

            hex.SetCoordinates(xCoord, yCoord);

            cells.Add(hex);

            Update_E_W_Neighbors(hex, width, height);
            Update_SE_NW_Neighbors(hex, width, height);
            Update_SW_NE_Neighbors(hex, width, height);
        }

        for (int y = 1; y <= height / 2 ; y++)
        {
            for (int x = 0; x < width - y; x++)
            {
                float x_row_offset = y * xOffset/2;

                float xPos = x * xOffset + x_row_offset;
                
                HexCell hex = (HexCell)Instantiate(hexPrefab, new Vector3(xPos, (height / 2 + y) * yOffset, 0), Quaternion.identity);

                int xCoord = x;
                int yCoord = (height / 2 + y);

                hex.name = "Hex (" + xCoord + ", " + yCoord + ")";
                hex.transform.SetParent(this.transform);

                TextMesh tm = hex.GetComponentInChildren<TextMesh>();
                tm.text = "(" + xCoord + ", " + yCoord + ")";

                hex.SetCoordinates(xCoord, yCoord);

                cells.Add(hex);

                Update_E_W_Neighbors(hex, width - y, height);
                Update_SE_NW_Neighbors(hex, hex.x + 1, hex.y, width, height);
                Update_SW_NE_Neighbors(hex, hex.x + 1, hex.y, width, height);
            }
        }
    }
    
    public HexCell GetHexByCoords(int x, int y){  
        foreach (var hex in cells)
        {
            if(hex.x == x && hex.y == y) return hex;
        }
        
        // If hex was not found in the list
        Debug.LogError("Function GetCellByCoords failed to find hex of coordinates (" + x + "," + y + ") in cells list!");
        return cells[0];
    }

    void Update_E_W_Neighbors(HexCell hex, int width, int height)
    {
        if(hex.x > 0) { // skip leftmost hexes
            HexCell westNeighbor = GetHexByCoords(hex.x - 1, hex.y);
            hex.SetNeighbor(HexDirection.W, westNeighbor);
            westNeighbor.SetNeighbor(HexDirection.E, hex);
        }
    }
    void Update_SE_NW_Neighbors(HexCell hex, int rowWidth, int height) // for bottom and mid part of grid
    {
        if(hex.y > 0 && hex.x < rowWidth - 1) { //skip bottom row and rightmost hexes
            HexCell southeastNeighbor = GetHexByCoords(hex.x, hex.y - 1);
            hex.SetNeighbor(HexDirection.SE, southeastNeighbor);
            southeastNeighbor.SetNeighbor(HexDirection.NW, hex);
        }
    }
    void Update_SE_NW_Neighbors(HexCell hex, int x, int y, int width, int height) // for top part of grid
    {
        HexCell southeastNeighbor = GetHexByCoords(hex.x + 1, hex.y - 1);
        hex.SetNeighbor(HexDirection.SE, southeastNeighbor);
        southeastNeighbor.SetNeighbor(HexDirection.NW, hex);
    }

    void Update_SW_NE_Neighbors(HexCell hex, int width, int height) // for bottom and mid part of grid
    {
        if(hex.y > 0 && hex.x > 0) { // skip bottom row and leftmost hexes
            HexCell southwestNeighbor = GetHexByCoords(hex.x - 1, hex.y - 1);
            hex.SetNeighbor(HexDirection.SW, southwestNeighbor);
            southwestNeighbor.SetNeighbor(HexDirection.NE, hex);
        }
    }

    void Update_SW_NE_Neighbors(HexCell hex, int x, int y, int width, int height) // for top part of grid
    {
        if(hex.y > 0) { // skip bottom row and leftmost hexes
            HexCell southwestNeighbor = GetHexByCoords(hex.x, hex.y - 1);
            hex.SetNeighbor(HexDirection.SW, southwestNeighbor);
            southwestNeighbor.SetNeighbor(HexDirection.NE, hex);
        }
    }
}
