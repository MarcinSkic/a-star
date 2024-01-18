using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public TextMeshProUGUI text;
    public bool chosenStart = false;
    public bool chosenEnd = false;
    [HideInInspector]
    public TileData clickedTile;

    [SerializeField]
    Button startGame;

    public TilesGenerator tilesGenerator;
    [SerializeField]
    Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { tilesGenerator.ChangeValueOfDrawingSpeed(slider.value); });
        text.text = "Wygeneruj siatkę";
        startGame.onClick.AddListener(ShowMessage);
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
            startGame.onClick.RemoveAllListeners();
            startGame.onClick.AddListener(tilesGenerator.StartGame);
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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.transform != null && !tilesGenerator.startedDrawing)
            {
                if (hit.transform.gameObject.TryGetComponent<TileData>(out clickedTile))
                {
                    ChoosingTiles();
                }
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
        yield return new WaitForSeconds(2f);
        text.text = oldMessage;
    }
    public void Restart(){

        chosenEnd = false;
        chosenStart = false;
        tilesGenerator.startedDrawing = false;

        startGame.onClick.RemoveAllListeners();
        startGame.onClick.AddListener(ShowMessage);
        text.text = "Wybierz kafelka startowego";

        foreach (TileData tile in tilesGenerator.tiles)
        {
            Destroy(tile.gameObject);   //Wiem że to mało wydajne i eleganckie
        }
        tilesGenerator.tiles = null;
        StartCoroutine(tilesGenerator.MakeGrid());
    }
}
