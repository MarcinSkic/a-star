using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class TilesGenerator : MonoBehaviour
{
    [SerializeField]
    Transform generationStartPoint; //Where generation of grid begins

    [SerializeField]
    TileData tilePrefab;

    [SerializeField]
    Button startGrid;   //Starts generation

    public Button startGame; //Starts finding path

    public TileData startPoint;
    public TileData endPoint;

    public TileData[,] tiles;
    
    public UIController uiController;


    int distance;

    int rows = 10, columns = 10;  //Size of grid

    void Start()
    {
        startGrid.onClick.AddListener(StartHelper);
    }
    public void StartHelper()
    {
        StartCoroutine(MakeGrid());
    }
    public IEnumerator MakeGrid()
    {
        tiles = new TileData [rows,columns];

        for (int i = 0; i < columns; i++)
        {            
            for(int x = 0; x < rows; x++)
            {
                var tile = Instantiate(tilePrefab, generationStartPoint);
                tile.transform.Translate(new Vector2(x*1.1f, i*1.1f));
                tile.manager = this;
                tile.uIController = uiController;

                if(i > 0)
                {
                    tile.UpdateNeighbour(tiles[x,i-1]);
                }
                if(x > 0)
                {
                    tile.UpdateNeighbour(tiles[x-1,i]);
                }
                
                tiles[x, i] = tile;

                yield return new WaitForSeconds(0.005f);
            }
        }
        
    }
    public void StartGame()
    {
        Debug.Log("Henlo!");
        //startPoint = null;
        //endPoint = null;

        /*foreach(TileData tile in tiles)
        {
            Debug.Log("eh?");
            while(startPoint == null || endPoint == null)
            {
                if (tile.isStartPoint)
                {
                    startPoint = tile;
                }
                else if (tile.isEndPoint)
                {
                    endPoint = tile;
                }
            }
        }*/

        ChoosePath(startPoint,endPoint);

    }
    public void ChoosePath(TileData startPoint, TileData endPoint)
    {
        bool workHehe = true;
        TileData current = null;
        int i = 0;
        List<TileData> accesible = new List<TileData>();
        List<TileData> calculated = new List<TileData>();
        accesible.Add(startPoint);

        while (workHehe)
        {
            
            if (accesible.Count == 1) 
            {
                current = accesible[0];
                current.h_cost = Vector2.Distance(current.transform.position, endPoint.transform.position);
                current.f_cost = current.h_cost + current.g_cost;
                //Debug.Log("Inside assigning current");
            } 
            else
            {
                current = accesible[0];
                foreach (TileData tile in accesible)
                {
                    if (current.f_cost > tile.f_cost || (current.f_cost == tile.f_cost && current.h_cost > tile.h_cost))
                    {
                        current = tile;
                    }                  
                }
            }

            current.GetComponent<Renderer>().material.color = Color.green;

            accesible.Remove(current);
            calculated.Add(current);

            if (accesible.Count == 0)
            {
                uiController.text.text = "Nie da się dojść do celu, wszystkie ścieżki zablokowane";
            }
            if (current.isEndPoint)
            {
                MarkPath(current);
                return; //something
                
            }
            foreach(TileData neighbour in current.neighboures)
            {
                if(neighbour.isLocked || calculated.Contains(neighbour))
                {
                    continue;
                }
                if (!accesible.Contains(neighbour))
                {
                    //Debug.Log("Sprawdza tych somsiadow");
                    neighbour.g_cost = current.g_cost + Vector2.Distance(current.transform.position, neighbour.transform.position);
                    neighbour.h_cost = Vector2.Distance(endPoint.transform.position, neighbour.transform.position);
                    neighbour.f_cost = neighbour.g_cost + neighbour.h_cost;
                    neighbour.gameObject.transform.parent = current.gameObject.transform;
                    if (!accesible.Contains(neighbour))
                    {
                        accesible.Add(neighbour);
                    }
                }
            }
            i++;
        }
    }
    public TileData MarkPath(TileData parent)
    {
        parent.gameObject.GetComponent<Renderer>().material.color = Color.red;
        if (parent.isStartPoint)
        {

            return null;
        }       
        return MarkPath(parent.transform.parent.GetComponent<TileData>());
    }
    /*public Transform ChoosePath(Vector2 start, Vector2 end)
    {
        Vector2 direction = new Vector2(end.x - start.x, end.y - start.y);
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x < 0)
            {

            }
        }       
        else
        {

        }
        return  
    }*/
    //public enum Directions {Up, Down, Right, Left };
}
