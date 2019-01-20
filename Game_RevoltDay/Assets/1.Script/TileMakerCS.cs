using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMakerCS : MonoBehaviour {
    public GameObject _gameMgr;
    private UIMgr uIMgrCS;

    public List<string> _cityName;
    private Text _cityText;

    public GameObject _tileImage;
    public List<GameObject> _tileMapList = new List<GameObject>();
    private int _tileMapSizeX = 7;
    private int _tileMapSizeY = 5;

    private GameObject tempTileMap;

    private Vector3 _tempIcon;

    public List<GameObject> TileMapList {
        get { return _tileMapList; }
    }

    private void Awake()
    {
        uIMgrCS = GameObject.Find("UIMgr").GetComponent<UIMgr>();

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
                UIMgr._isSafetyUI += _tileMapList[arrIndex].GetComponent<TileMapDataCS>().isSafetyUI;
                UIMgr._tileTurnUpdate += _tileMapList[arrIndex].GetComponent<TileMapDataCS>().tileTrunUpdate;
                arrIndex++;
            }
        }
    }

    private void Start()
    {
        if (_gameMgr.GetComponent<SaveSys>()._saveFile.isSaveData) saveLead();
    }

    public void saveLead()
    {
        Debug.Log("불러오기");
        for (int i = 0; i < _tileMapList.Count; i++)
        {
            _tileMapList[i].GetComponent<TileMapDataCS>()._isBlockade = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isBlockade;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isIssueIcon = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isIssueIcon;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isSafetyEff = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isSafetyEff;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isShop = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isShop;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isSpShop = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isSpShop;
            _tileMapList[i].GetComponent<TileMapDataCS>()._SafetyValue = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._SafetyValue;
            _tileMapList[i].GetComponent<TileMapDataCS>()._tileIndex = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._tileIndex;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isIssue = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isIssue;
            _tileMapList[i].GetComponent<TileMapDataCS>()._isCrime = _gameMgr.GetComponent<SaveSys>()._saveFile._tileMapList[i]._isCrime;
            _tileMapList[i].GetComponent<TileMapDataCS>().saveSetting();
        }
    }

    public int getTileVlaue(float x, float y)
    {
        //_tempIcon = new Vector3(x, y);

        for (int i = 0; i < _tileMapList.Count; i++)
        {
            if (_tileMapList[i].GetComponent<RectTransform>().position.x + _tileMapList[i].GetComponent<RectTransform>().sizeDelta.x / 2.0f > x &&
                _tileMapList[i].GetComponent<RectTransform>().position.x - _tileMapList[i].GetComponent<RectTransform>().sizeDelta.x / 2.0f < x &&
                _tileMapList[i].GetComponent<RectTransform>().position.y + _tileMapList[i].GetComponent<RectTransform>().sizeDelta.y / 2.0f > y &&
                _tileMapList[i].GetComponent<RectTransform>().position.y - _tileMapList[i].GetComponent<RectTransform>().sizeDelta.y / 2.0f < y)
            {
                int tempNum = i;
                _tempIcon = _tileMapList[i].GetComponent<RectTransform>().position;
                return tempNum;
            }
        }
        return -1;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(_tempIcon, 50.0f);
    }
}
