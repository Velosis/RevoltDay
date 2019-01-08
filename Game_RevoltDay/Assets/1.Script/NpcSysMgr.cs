using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSysMgr : MonoBehaviour {
    public UIMgr _uIMgrCS;

    public GameObject[] _npcList;
    public List<GameObject> _tileMapList;
    private int _currNpcNum = 0;
    private int _tempNpcCount = 0;

    public GameObject _playerIcon;
    public GameObject _HamIcon;
    public GameObject _JeonIcon;
    public GameObject _ParkIcon;
    public GameObject _WishIcon;
    public GameObject _YoungIcon;

    private void Awake()
    {
        _npcList = GameObject.Find("GameMgr").GetComponent<EventSysCS>()._iconList;
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;

        for (int i = 0; i < _npcList.Length; i++)
        {
            switch (_npcList[i].GetComponent<PlayerInfoCS>()._eNpcType)
            {
                case eNpcType.gangicon:
                    _playerIcon = _npcList[i];
                    break;
                case eNpcType.Hamicon:
                    _HamIcon = _npcList[i];
                    break;
                case eNpcType.Jeonicon:
                    _JeonIcon = _npcList[i];
                    break;
                case eNpcType.Parkicon:
                    _ParkIcon = _npcList[i];
                    break;
                case eNpcType.Wishicon:
                    _WishIcon = _npcList[i];
                    break;
                case eNpcType.Youngicon:
                    _YoungIcon = _npcList[i];
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        sttingNPC("WishIcon", 5, eNpcType.Wishicon);
    }

    public IEnumerator NpcAct()
    {
        Debug.Log("턴시작");
        _tempNpcCount = 0;
        for (int i = 0; i < _npcList.Length; i++)
        {
            if (_npcList[i].GetComponent<PlayerInfoCS>()._isAlive) _tempNpcCount++;
        }
        _tempNpcCount -= 1; // 플레이어 보정

        while (_tempNpcCount != 0)
        {
            wishAI();
            yield return null;
        }

        if (_tempNpcCount == 0)
        {
            Debug.Log("_tempNpcCount : " + _tempNpcCount);
            _uIMgrCS.SetTurnWaitButton(true);
        }
    }

    public void sttingNPC(string name, int tileMap, eNpcType whoNpc)
    {
        for (int i = 0; i < _npcList.Length; i++)
        {
            if (_npcList[i].name == name)
            {
                _npcList[i].GetComponent<PlayerInfoCS>()._isAlive = true;
                _npcList[i].GetComponent<PlayerInfoCS>()._eNpcType = whoNpc;
                _npcList[i].GetComponent<PlayerInfoCS>().setTileValeu(tileMap);
            }
        }
    }

    public void wishAI()
    {
        int tempCurrTile = _WishIcon.GetComponent<PlayerInfoCS>()._currTile;
        if (Random.Range(1.0f, 100.0f) < 20.0f && _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._isBlockade)
        {
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>().setBlockade(false);
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = false;
            _tempNpcCount--;
            return;
        }
        else if (Random.Range(1.0f, 100.0f) < 10.0f && _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
        {
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>().setSafetyBer(-5.0f);
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = false;
            _tempNpcCount--;

            return;
        }else if (_tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._isBlockade ||
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
        {
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = false;
            _tempNpcCount--;

            return;
        }

        for (int i = 0; i < _tileMapList.Count; i++) // 치안 게이지가 3이상인 곳이 있는지 탐색
        {
            if(_tileMapList[i].GetComponent<TileMapDataCS>()._isBlockade ||
                _tileMapList[i].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
            {
                _WishIcon.GetComponent<PlayerInfoCS>().npcMoveSys(i);
                _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = false;
                _tempNpcCount--;

                return;
            }
        }

        _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = false;
        _tempNpcCount--;

        return;
    }
}
