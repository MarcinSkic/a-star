using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TilesGenerator : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1)]
    float speedOfDrawing;
    
    
    [SerializeField]
    Transform generationStartPoint; //Where generation of grid begins

    [SerializeField]
    TileData tilePrefab;

    [SerializeField]
    Button startGrid;   //Starts generation

    public TileData startPoint;
    public TileData endPoint;

    public TileData[,] tiles;
    
    public UIController uiController;

    [HideInInspector]
    public bool startedDrawing = false;

    int distance;

    int rows = 10, columns = 10;  //Size of grid

    void Start()
    {
        startGrid.onClick.AddListener(StartHelper);
    }
    public void StartHelper()
    {
        
        startGrid.onClick.RemoveAllListeners();
        startGrid.GetComponentInChildren<TextMeshProUGUI>().text = "Zrestartuj siatkę";
        uiController.text.text = "Wybierz kafelka startowego";
        startGrid.onClick.AddListener(uiController.Restart);
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
                    if (i > 0)
                    {
                        tile.UpdateNeighbour(tiles[x - 1, i - 1]);
                        if (x < rows - 1)
                        {
                            tile.UpdateNeighbour(tiles[x + 1, i - 1]);
                        }
                    }
                    tile.UpdateNeighbour(tiles[x-1,i]);
                }
                
                tiles[x, i] = tile;

                yield return new WaitForSeconds(0.005f);
            }
        }
        
    }
    public void StartGame()
    {
        StartCoroutine(ChoosePath(startPoint,endPoint));
    }
    IEnumerator ChoosePath(TileData startPoint, TileData endPoint)
    {
        startedDrawing = true;
        bool workHehe = true;
        bool checkedForNeighboures = false;
        bool firstEntry = true;
        TileData current = null;
        int i = 0;
        List<TileData> accesible = new List<TileData>();
        List<TileData> calculated = new List<TileData>();
        accesible.Add(startPoint);

        while (workHehe)
        {
            Debug.Log(accesible.Count);
            
            if (firstEntry) 
            {
                firstEntry = false;
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

            

            if (current.isEndPoint)
            {
                uiController.text.text = "Ścieżka znaleziona";
                
                StopAllCoroutines();
                StartCoroutine(MarkPath(current));
                
                
            }
            

            foreach (TileData neighbour in current.neighboures)
            {
                if(neighbour.isLocked || calculated.Contains(neighbour))
                {
                    continue;
                }
                if (!accesible.Contains(neighbour) || Vector2.Distance(current.transform.parent.transform.position,neighbour.transform.position)+current.transform.parent.GetComponent<TileData>().g_cost < neighbour.g_cost)
                {
                    yield return new WaitForSeconds((1/speedOfDrawing)/50f);
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

            checkedForNeighboures = true;

            if (accesible.Count == 0 && checkedForNeighboures)
            {
                uiController.text.text = "Nie da się dojść do celu, wszystkie ścieżki zablokowane";
                startedDrawing = false;
                StopAllCoroutines();
            }
            i++;
        }
    }
    IEnumerator MarkPath(TileData parent)
    {
        yield return new WaitForSeconds((1 / speedOfDrawing)/50f);
        

        if (parent.isStartPoint)
        {
            StopAllCoroutines();
        }

        parent.gameObject.GetComponent<Renderer>().material.color = Color.red;

        StartCoroutine(MarkPath(parent.transform.parent.GetComponent<TileData>()));
    }
    public void ChangeValueOfDrawingSpeed(float value)
    {
        speedOfDrawing = Mathf.Clamp(value,0.1f,1f);
    }
    /*public TileData MarkPath(TileData parent)
    {
        parent.gameObject.GetComponent<Renderer>().material.color = Color.red;
        if (parent.isStartPoint)
        {

            return null;
        }       
        return MarkPath(parent.transform.parent.GetComponent<TileData>());
    }*/

}
