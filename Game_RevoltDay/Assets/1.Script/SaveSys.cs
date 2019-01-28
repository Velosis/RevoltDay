using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDateNamespace;

public class SaveSys : MonoBehaviour {
    public SaveData _saveFile;
    public SaveData[] _saveFileArr;
    public PlayerInfoCS _playerInfoCS;
    public EventSysCS _eventSysCS;
    public NpcSysMgr _npcSysMgrCS;
    public TileMakerCS _tileMakerCS;

    public UIMgr _uIMgrCS;

    public bool _isDeleteSave;

    public bool _TEST_BOOL;

    private void Awake()
    {
        for (int i = 0; i < _saveFileArr.Length; i++)
        {
            if (!_TEST_BOOL) _saveFile = GameObject.Find("TileUI").GetComponent<SceneMgr>()._currFile;
        }
    }



    private void Update()
    {
        if (_isDeleteSave)
        {
            _isDeleteSave = false;
        }
    }

    public void saveSys(int value)
    {
        // 최초 저장 여부
        _saveFileArr[value].isSaveData = true;

        // 년도 저장
        _saveFileArr[value].SaveDay = System.DateTime.Now.ToString("yyyy. MM. dd");
        _saveFileArr[value].SaveDay += " / " + System.DateTime.Now.ToString("HH시 mm분");

        // 사용중인 아이템 저장
        for (int i = 0; i < _playerInfoCS._currUseItemList.Count; i++)
        {
            _saveFileArr[value]._useItemList[i]._Index = _playerInfoCS._currUseItemList[i]._Codex;
            _saveFileArr[value]._useItemList[i]._currTurn = _playerInfoCS._currUseItemList[i]._currTurnOtp;
        }

        // 아이템 저장
        for (int i = 0; i < _playerInfoCS._BoxItemList.Count; i++)
        {
            _saveFileArr[value]._currItemDatasList[i]._Index = _playerInfoCS._BoxItemList[i]._Codex;
        }
        // 장비 저장
        for (int i = 0; i < _playerInfoCS._BoxEquipList.Count; i++)
        {
            _saveFileArr[value]._currEquipDatasList[i]._Index = _playerInfoCS._BoxEquipList[i]._Codex;
            _saveFileArr[value]._currEquipDatasList[i]._setUse = _playerInfoCS._BoxEquipList[i]._isSet;
        }
        // 조력자 저장
        for (int i = 0; i < _playerInfoCS._BoxAidList.Count; i++)
        {
            _saveFileArr[value]._currAidDatasList[i]._Index = _playerInfoCS._BoxAidList[i]._Codex;
            _saveFileArr[value]._currAidDatasList[i]._currTurn = _playerInfoCS._BoxAidList[i]._currCoolTime;
            _saveFileArr[value]._currAidDatasList[i]._isGet = _playerInfoCS._BoxAidList[i]._isGet;
            _saveFileArr[value]._currAidDatasList[i]._setUse = _playerInfoCS._BoxAidList[i]._isSet;
        }

        // 맵 타일 저장
        for (int i = 0; i < _tileMakerCS.TileMapList.Count; i++)
        {
            _saveFileArr[value]._tileMapList[i]._isBlockade = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isBlockade;
            _saveFileArr[value]._tileMapList[i]._isIssueIcon = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isIssueIcon;
            _saveFileArr[value]._tileMapList[i]._isSafetyEff = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isSafetyEff;
            _saveFileArr[value]._tileMapList[i]._isShop = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isShop;
            _saveFileArr[value]._tileMapList[i]._isSpShop = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isSpShop;
            _saveFileArr[value]._tileMapList[i]._SafetyValue = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._SafetyValue;
            _saveFileArr[value]._tileMapList[i]._tileIndex = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._tileIndex;
            _saveFileArr[value]._tileMapList[i]._isIssue = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isIssue;
            _saveFileArr[value]._tileMapList[i]._isCrime = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.activeSelf;
        }



        // 스토리 진행 관련
        _saveFileArr[value]._currEventID = _eventSysCS._currEventID;

        // 플레이어 스테이터스
        _saveFileArr[value]._clueTokenValue = _playerInfoCS._clueTokenValue;
        _saveFileArr[value]._isAlive = _playerInfoCS._isAlive;
        _saveFileArr[value]._isTurn = _playerInfoCS._isTurn;
        _saveFileArr[value]._daleyTurnCount = _playerInfoCS._daleyTurnCount;
        _saveFileArr[value]._reasoningValue = _playerInfoCS._reasoningValue;

        _saveFileArr[value]._currTile = _playerInfoCS._currTile;
        _saveFileArr[value]._tempCurrTile = _playerInfoCS._tempCurrTile;
        _saveFileArr[value]._currActPoint = _playerInfoCS._currActPoint;
        _saveFileArr[value]._currTrunPoint = _playerInfoCS._currTrunPoint;

        _saveFileArr[value]._currHP = _playerInfoCS._currHP;
        _saveFileArr[value]._MaxHP = _playerInfoCS._MaxHP;
        _saveFileArr[value]._atkPoint = _playerInfoCS._atkPoint;

        // NPC 정보 저장
        for (int i = 0; i < _npcSysMgrCS._npcList.Length; i++)
        {
            if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Hamicon)
            {
                _saveFileArr[value]._HamIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFileArr[value]._HamIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFileArr[value]._HamIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                _saveFileArr[value]._HamSaveData = _npcSysMgrCS._HamRootBox;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Jeonicon)
            {
                _saveFileArr[value]._JeonIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFileArr[value]._JeonIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFileArr[value]._JeonIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                _saveFileArr[value]._JeonSaveData = _npcSysMgrCS._JeonRootBox;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Wishicon)
            {
                _saveFileArr[value]._WishIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFileArr[value]._WishIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFileArr[value]._WishIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Youngicon)
            {
                _saveFileArr[value]._YoungIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFileArr[value]._YoungIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFileArr[value]._YoungIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                _saveFileArr[value]._YoungSaveData = _npcSysMgrCS._YoungRootBox;
            }
        }


        _uIMgrCS.SettingSaveUi(_uIMgrCS._SaveOfLoadUIGO);
    }
}
