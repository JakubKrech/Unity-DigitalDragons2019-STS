using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    public HexGrid hexGridPrefab;
    public GameObject backgroundPrefab;
    public Sprite backgroundImage;
    public Character[] characterArray;
    //private SpriteRenderer spriteRend;


    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        HexGrid hexGrid = (HexGrid)Instantiate(hexGridPrefab, new Vector3(0,0,0), Quaternion.identity, this.transform);
        hexGrid.name = "HexGrid";
        hexGrid.width = 5;

        GameObject background = (GameObject)Instantiate(backgroundPrefab, new Vector3(8f,4.6f,5f), Quaternion.identity, this.transform);
        background.name = "Background";
        SpriteRenderer backgroundSR = background.GetComponent<SpriteRenderer>();
        backgroundSR.sprite = backgroundImage;

        Vector3 charPosition = hexGrid.GetHexByCoords(3, 5).transform.position;
        charPosition.x += 0.1f;
        charPosition.y += 0.2f;
        charPosition.z = -0.01f;
        Character char1 = (Character)Instantiate(characterArray[0], charPosition, Quaternion.identity, this.transform);
        char1.name = char1.charName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
