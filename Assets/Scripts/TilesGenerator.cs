using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    Button startGame; //Starts finding path

    public TileData[,] tiles;
    

    int distance;

    int rows = 5, columns = 5;  //Size of grid

    void Start()
    {
        startGrid.onClick.AddListener(StartHelper);
        startGame.onClick.AddListener(StartGame);
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

                if(i > 0)
                {
                    tile.UpdateDownNeighbour(tiles[x,i-1]);
                }
                if(x > 0)
                {
                    tile.UpdateLeftNeighbour(tiles[x-1,i]);
                }
                
                tiles[x, i] = tile;

                yield return new WaitForSeconds(0.2f);
            }
        }
        
    }
    public void StartGame()
    {

        TileData startPoint = null;
        TileData endPoint = null;

        foreach(TileData tile in tiles)
        {
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
        }

        ChoosePath(startPoint);

    }
    public void ChoosePath(TileData startPoint)
    {
        bool enterLoopHehe = true;
        List<TileData> accesible = new List<TileData>();
        List<TileData> calculated = new List<TileData>();
        accesible.Add(startPoint);

        while (enterLoopHehe)
        {
            foreach()
        }
        gameObject.GetComponent<Renderer>().material.color = Color.green;

        
        
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
