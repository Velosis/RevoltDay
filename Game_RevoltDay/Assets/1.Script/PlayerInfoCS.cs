using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCS : MonoBehaviour {

    public GameObject _currPlayerGO;
    static public Image _currPlayerImg;


    private List<GameObject> _tileMapList;

    private int _playerTileX = 3;
    private int _playerTileY = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _currPlayerImg = _currPlayerGO.GetComponent<Image>();
    }

    void Start () {

        if (GameObject.Find("MapTileMgr"))
        {
            _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
            transform.GetComponent<RectTransform>().position = _tileMapList[8].GetComponent<RectTransform>().position;
        }
    }

    public void PlaterMoveSys(int mapValue)
    {
        transform.GetComponent<RectTransform>().position = _tileMapList[mapValue].GetComponent<RectTransform>().position;
    }
}
