using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public TilesGenerator manager;

    public bool isLocked;
    public bool isStartPoint;
    public bool isEndPoint;
    public bool isPath;
    

    public TileData UpNeighbour;
    public TileData DownNeighbour;
    public TileData RightNeighbour;
    public TileData LeftNeighbour;

    public void UpdateLeftNeighbour(TileData tile)
    {
        tile.RightNeighbour = this;
        LeftNeighbour = tile;
    }
    public void UpdateDownNeighbour(TileData tile)
    {
        tile.UpNeighbour = this;
        DownNeighbour = tile;
    }

    public void ChoosePath(Vector2 target, int distance)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;

        if (target.Equals(transform.position) || distance >= 10)
        {
            return; //Return list of proper tiles
        }
        Vector2 direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && RightNeighbour != null && LeftNeighbour != null)
        {
            if(direction.x > 0 && RightNeighbour != null)
            {

                RightNeighbour.ChoosePath(target,distance++);
            } 
            else if(LeftNeighbour != null)
            {               
                LeftNeighbour.ChoosePath(target, distance++);
            }
        } 
        else
        {
            if(direction.y > 0 && UpNeighbour != null)
            {
                UpNeighbour.ChoosePath(target, distance++);
            }
            else if (DownNeighbour != null)
            {
                DownNeighbour.ChoosePath(target, distance++);
            }
            else
            {
                Debug.Log("Im STUCK!!!!");
            }
        }
    }
}
