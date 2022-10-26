using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _referenceTileDark;
    [SerializeField] private GameObject _referenceTileLight;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _referenceTileMarkingLeft;
    [SerializeField] private GameObject _referenceTileMarkingBottom;

    private List<List<GameObject>> _boardTiles = new List<List<GameObject>>(); // row major
    private List<GameObject> _boardTextTiles = new List<GameObject>();
    private Vector2Int _currentDims = new Vector2Int(0,0);

    // Start is called before the first frame update
    void Start()
    {
        _referenceTileDark.SetActive(false);
        _referenceTileLight.SetActive(false);
        _referenceTileMarkingLeft.SetActive(false);
        _referenceTileMarkingBottom.SetActive(false);


        GenerateBoard(7, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBoard(int x, int y) 
    {
        _currentDims = new Vector2Int(x, y);
        foreach (var row in _boardTiles) { 
            foreach (var tile in row)
            {
                Destroy(tile);
            }
        }
        _boardTiles.Clear();

        foreach (var tile in _boardTextTiles)
        {
            Destroy(tile);
        }
        _boardTextTiles.Clear();

        // Create Board Tiles
        for (int row = 0; row < y; row++) {
            _boardTiles.Add(new List<GameObject>());

            for (int col = 0; col < x; col++)
            {
                bool isBlack = (row + col) % 2 == 0;
                GameObject newTile = isBlack ? Instantiate(_referenceTileDark) : Instantiate(_referenceTileLight);
                _boardTiles[row].Add(newTile);
                newTile.transform.localPosition = new Vector3(col, 0, row);
                newTile.SetActive(true);
                newTile.transform.SetParent(this.transform);
            }
        }
        
        // Create Text Board Markings
        for (int row = 0; row < y; row++) {
            GameObject leftText = Instantiate(_referenceTileMarkingLeft);
            _boardTextTiles.Add(leftText);
            leftText.SetActive(true);
            leftText.GetComponent<TMP_Text>().SetText((row+1).ToString());
            leftText.transform.SetParent(_canvas.transform);
            Vector3 tilePos = GetCenterPositionOfTile(-1, row);
            leftText.transform.localPosition = new Vector3(tilePos.x, tilePos.z, 0);
            leftText.transform.localRotation = new Quaternion();
        }
        for (int col = 0; col < x; col++)
        {
            GameObject bottomText = Instantiate(_referenceTileMarkingBottom);
            _boardTextTiles.Add(bottomText);
            bottomText.SetActive(true);
            bottomText.GetComponent<TMP_Text>().SetText(((char)(col+65)).ToString());
            bottomText.transform.SetParent(_canvas.transform);
            Vector3 tilePos = GetCenterPositionOfTile(col, -1);
            bottomText.transform.localPosition = new Vector3(tilePos.x, tilePos.z, 0);
            bottomText.transform.localRotation = new Quaternion();
        }

        this.transform.localPosition = new Vector3((-x / 2.0f) + 0.5f, 0, (-y / 2.0f) + 0.5f);
        _canvas.transform.localPosition = this.transform.localPosition*-1;
        Debug.Log("Generate Board finished.");
    }

    //public GameObject getTileAt(int x, int y)
    //{
    //    return _boardTiles[x][y];
    //}

    public Vector3 GetCenterPositionOfTile(int x, int y)
    {
        return new Vector3((-_currentDims.x / 2.0f) + x + 0.5f, 0, (-_currentDims.y / 2.0f) + y + 0.5f);
    }
}
