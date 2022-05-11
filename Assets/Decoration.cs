using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    public bool MouseDown;
    public float DecorationYExtra = 1;
    public float DecorationXExtra = 1;
    public float DecorationZExtra = 1;
    public int Facing = 1;
    public Transform WallTile;
    public Transform FloorTile;

    private void OnMouseDown()
    {
        MouseDown = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && MouseDown && (WallTile != null || FloorTile != null))
        {
            MouseDown = false;
            WallTile = null;
            FloorTile = null;
        }

        if (Input.GetMouseButtonDown(1) && MouseDown && (WallTile != null || FloorTile != null))
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + 45, this.transform.eulerAngles.z);
        }

        if (MouseDown)
        {
            if (WallTile != null && Facing == 1)
            {
                this.transform.position = new Vector3(WallTile.position.x, WallTile.position.y + DecorationYExtra, WallTile.position.z + DecorationZExtra);
            }
            else if (WallTile != null)
            {
                this.transform.position = new Vector3(WallTile.position.x + DecorationXExtra, WallTile.position.y, WallTile.position.z);
            }
            else if (FloorTile != null)
            {
                this.transform.position = new Vector3(FloorTile.position.x, FloorTile.position.y + DecorationYExtra, FloorTile.position.z);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("GridWallRight"))
                {
                    Facing = 1;
                    FloorTile = null;
                    WallTile = hit.transform;
                }
                else if (hit.collider.CompareTag("GridWallLeft"))
                {
                    Facing = -1;
                    FloorTile = null;
                    WallTile = hit.transform;
                }
                else if (hit.collider.CompareTag("GridFloor"))
                {
                    WallTile = null;
                    FloorTile = hit.transform;
                }
            }
        }
    }
}