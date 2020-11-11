using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    public bool clicked = false;
    public bool chosenStart = false;
    public bool chosenEnd = false;
    [HideInInspector]
    public TileData clickedTile;

    public TilesGenerator tilesGenerator;

    private void Start()
    {
        text.text = "Wybierz kafelka startowego";
        tilesGenerator.startGame.onClick.AddListener(ShowMessage);

    }
    public void ChoosingTiles()
    {
        if (!chosenStart)
        {
            tilesGenerator.startPoint = clickedTile;
            clickedTile.isStartPoint = true;
            clickedTile.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            text.text = "Wybierz kafelka końcowego";
            chosenStart = true;
        } 
        else if (!chosenEnd)
        {
            tilesGenerator.endPoint = clickedTile;
            clickedTile.isEndPoint = true;
            clickedTile.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            text.text = "Jak chcesz to dodaj kafelki zablokowane i możesz zaczynać";
            chosenEnd = true;
            tilesGenerator.startGame.onClick.AddListener(tilesGenerator.StartGame);
        } 
        else 
        {
            clickedTile.isLocked = true;
            clickedTile.gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Send?");

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.TryGetComponent<TileData>(out clickedTile))
                {
                    ChoosingTiles();
                }                        
        }
    }

    public void ShowMessage()
    {
        StartCoroutine(Text(text.text));
    }
    public IEnumerator Text(string oldMessage)
    {
        text.text = "Najpierw wybierz kafelki startu i końca";
        yield return new WaitForSeconds(3f);
        text.text = oldMessage;
    }
}
