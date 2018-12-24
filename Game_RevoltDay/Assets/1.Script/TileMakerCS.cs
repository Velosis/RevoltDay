using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMakerCS : MonoBehaviour {

    public List<string> _cityName;
    private Text _cityText;

    public GameObject _tileImage;
    private List<GameObject> _tileMapList = new List<GameObject>();
    private int _tileMapSizeX = 7;
    private int _tileMapSizeY = 5;

    private GameObject tempTileMap;

    public List<GameObject> TileMapList {
        get { return _tileMapList; }
    }

    private void Awake()
    {
        int arrIndex = 0;

        float tempSize = _tileImage.GetComponent<RectTransform>().rect.width;
        for (int x = 0; x < _tileMapSizeX; x++)
        {
            for (int y = 0; y < _tileMapSizeY; y++)
            {
                if (x <= 2 && y == 0) continue;
                if (x == 0 && y <= 2) continue;
                if (x == 1 && y == 1) continue;
                if (x >= 4 && y == 0) continue;
                if (x >= 5 && y <= 2) continue;
                if (x >= 1 && x != 6 && y == 4) continue;
                if (x >= 2 && x < 4 && y == 3) continue;
                if (x == 3 && y == 2) continue;

                tempTileMap = Instantiate(_tileImage, transform.position, transform.rotation, transform);
                tempTileMap.transform.Translate((float)x * tempSize, (float)y * tempSize, 0, Space.Self);

                _tileMapList.Add(tempTileMap);
                _tileMapList[arrIndex].GetComponentInChildren<Text>().text = _cityName[arrIndex].ToString();
                _tileMapList[arrIndex].GetComponent<TileMapDataCS>()._tileIndex = arrIndex;
                arrIndex++;

            }
        }
    }
    private void Start()
    {

    }
}
