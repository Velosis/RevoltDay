using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoCS : MonoBehaviour {

    private List<GameObject> _tileMapList;

    private int _playerTileX = 3;
    private int _playerTileY = 1;

    // Use this for initialization
    void Start () {
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
        transform.GetComponent<RectTransform>().position = _tileMapList[8].GetComponent<RectTransform>().position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaterMoveSys(int mapValue)
    {
        transform.GetComponent<RectTransform>().position = _tileMapList[mapValue].GetComponent<RectTransform>().position;
    }
}
