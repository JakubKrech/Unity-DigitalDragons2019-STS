using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum HexDirection {
    NE, E, SE, SW, W, NW
}

public class HexCell : MonoBehaviour
{
    [SerializeField]
    public int x, y;
    Material material;
    SpriteRenderer sr;

    [SerializeField]
    public HexCell[] neighbors = new HexCell[6];


    // Start is called before the first frame update
    void Start()
    {
        Material textMesh = gameObject.GetComponentInChildren<MeshRenderer>().material;
        textMesh.renderQueue = 3002;

        material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
        material.renderQueue = 3001; // without it hexes dissapear while moving the camera

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.color = new Color(1f,1f,1f,0.4f);

        Transform child = this.transform.GetChild(0);
        child.gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        // if(hit2D.collider != null){
        //     if(Input.GetMouseButtonDown(0)){

        //         HexCell hexE = this.GetNeighbor(HexDirection.E);
        //         hexE.GetComponentInChildren<SpriteRenderer>().material.color = Color.red;

        //         // HexCell hexE = neighbors[(int)HexDirection.E];
        //         // SpriteRenderer srE = hexE.GetComponentInChildren<SpriteRenderer>();
        //         // srE.color = Color.red;

        //     }
        // }
    }


    /// OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
    // void OnMouseDown()
    // {
    //     foreach (var hex in neighbors)
    //     {
    //         SpriteRenderer sr = hex.GetComponentInChildren<SpriteRenderer>();
    //         sr.color = Color.red;
    //     }
    // }

    public HexCell GetNeighbor (HexDirection direction) {
        return neighbors[(int)direction];
    }

    public void SetNeighbor (HexDirection direction, HexCell cell) {
        neighbors[(int)direction] = cell;
    }

    public HexDirection Opposite (HexDirection direction) {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public void SetCoordinates (int xx, int yy){
        x = xx;
        y = yy;
    }
}
