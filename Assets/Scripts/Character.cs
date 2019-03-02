using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public  string charName;
    public int level, health, mana, actionPoints;
    public int strength, agility, power, initiative;
    Material material;
    public HexCell hexCell;

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
        
    }


    // OnMouseDown is called when the user has pressed the mouse button while
    // over the GUIElement or Collider.
    void OnMouseDown()
    {

    }
}
