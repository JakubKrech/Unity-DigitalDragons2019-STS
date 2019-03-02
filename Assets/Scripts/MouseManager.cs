using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Character movedChar;
    public bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        if(hit2D.collider != null){
            
            // if(hit2D.collider.transform.parent.tag.Equals("Hex")) {
            //     if(Input.GetMouseButtonDown(0)){
            //         Debug.Log(hit2D.collider.transform.parent.name);

            //         HexCell hex = hit2D.collider.transform.parent.gameObject.GetComponent<HexCell>(); // accessing HexCell on click
                    
            //         foreach (var neighborHex in hex.neighbors)
            //         {
            //             if (neighborHex != null){
            //                 SpriteRenderer sr = neighborHex.GetComponentInChildren<SpriteRenderer>();
            //                 sr.color = Color.red;
            //             }
            //         }

            //         //HexCell hex = hit2D.collider.transform.parent.hex;

            //         // SpriteRenderer sr = hit2D.collider.transform.gameObject.GetComponentInChildren<SpriteRenderer>();

            //         /*float Alpha = sr.color.a;
            //         Alpha -= 0.1f;
            //         sr.color = new Color(1f,1f,1f,Alpha);*/
            //     }

            //     if(Input.GetMouseButtonDown(1)){
            //         Debug.Log(hit2D.collider.transform.parent.name);

            //         SpriteRenderer sr = hit2D.collider.transform.gameObject.GetComponentInChildren<SpriteRenderer>();

            //         float Alpha = sr.color.a;
            //         Alpha += 0.1f;
            //         sr.color = new Color(1f,1f,1f,Alpha);
            //     }
            // }

            if(hit2D.collider.transform.tag.Equals("Character"))
            {
                if(Input.GetMouseButtonDown(0)){
                    
                    movedChar = hit2D.collider.transform.parent.gameObject.GetComponent<Character>();
                    Debug.Log("CHARACTER " + movedChar.name + " LOCKED!");
                    moving = true;

                }   
            }

            if(hit2D.collider.transform.tag.Equals("Hex")){
                if(Input.GetMouseButtonUp(0)){
                    if(moving){
                        HexCell hex = hit2D.collider.transform.parent.gameObject.GetComponent<HexCell>();

                        if(!hex.occupied){
                            Debug.Log("CHARACTER " + movedChar.name + " moved from " + movedChar.hexCell.name + " to " + hex.name + "!");

                            movedChar.hexCell.occupied = false;
                            movedChar.hexCell = hex;
                            hex.occupied = true;
                            Vector3 newCharPosition = new Vector3(movedChar.hexCell.transform.position.x, movedChar.hexCell.transform.position.y, -0.01f);
                            movedChar.transform.transform.position = newCharPosition;
                            
                            moving = false;
                            movedChar = null;
                        }
                        else 
                        {
                            Debug.Log("HexCell already occupied!");
                            moving = false;
                        }
                    }
                    else moving = false;
                }
            }
        }
        
        //Debug.Log(Input.mousePosition);
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if( Physics.Raycast(ray, out hitInfo) ) {

            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

            Debug.Log("Tile " + hitInfo.collider.transform.parent.name + " was hit!");

            if(Input.GetMouseButton(0)){
                MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();

                mr.material.color = Color.red; 
                //Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
        }
        */
    }
}
