using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSys : MonoBehaviour {
    public SaveData _saveFile;
    public SaveData[] _saveFileArr = new SaveData[4];
    public PlayerInfoCS _playerInfoCS;
    public EventSysCS _eventSysCS;
    public NpcSysMgr _npcSysMgrCS;
    public TileMakerCS _tileMakerCS;

    private SceneMgr _sceneMgrCS;

    public UIMgr _uIMgrCS;

    public bool _isDeleteSave;

    public bool _TEST_BOOL;

    private void Awake()
    {


        if (!_TEST_BOOL)
        {
            _sceneMgrCS = GameObject.Find("DonTileUI").GetComponent<SceneMgr>();
            _saveFileArr = _sceneMgrCS._currSaveDataList;
            _saveFile = _saveFileArr[_sceneMgrCS._SaveNumber];

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
        SaveData TempSaveData = new SaveData();

        // 최초 저장 여부
        TempSaveData.isSaveData = true;
        // 년도 저장
        TempSaveData.SaveDay = System.DateTime.Now.ToString("yyyy. MM. dd");
        TempSaveData.SaveDay += " / " + System.DateTime.Now.ToString("HH시 mm분");

        // 사용중인 아이템 저장
        for (int i = 0; i < _playerInfoCS._currUseItemList.Count; i++)
        {
            itemDataSave _itemData = new itemDataSave();

            _itemData._Index = _playerInfoCS._currUseItemList[i]._Codex;
            _itemData._currTurn = _playerInfoCS._currUseItemList[i]._currTurnOtp;

            TempSaveData._useItemList.Add(_itemData);
        }

        // 아이템 저장
        for (int i = 0; i < _playerInfoCS._BoxItemList.Count; i++)
        {
            itemDataSave _itemDataSave = new itemDataSave();

            _itemDataSave._Index = _playerInfoCS._BoxItemList[i]._Codex;

            TempSaveData._currItemDatasList.Add(_itemDataSave);
        }
        // 장비 저장
        for (int i = 0; i < _playerInfoCS._BoxEquipList.Count; i++)
        {
            equipData _equipData = new equipData();
            _equipData._Index = _playerInfoCS._BoxEquipList[i]._Codex;
            _equipData._setUse = _playerInfoCS._BoxEquipList[i]._isSet;

            TempSaveData._currEquipDatasList.Add(_equipData);
        }
        // 조력자 저장
        for (int i = 0; i < _playerInfoCS._BoxAidList.Count; i++)
        {
            aidData _aidData = new aidData();

            _aidData._Index = _playerInfoCS._BoxAidList[i]._Codex;
            _aidData._currTurn = _playerInfoCS._BoxAidList[i]._currCoolTime;
            _aidData._isGet = _playerInfoCS._BoxAidList[i]._isGet;
            _aidData._setUse = _playerInfoCS._BoxAidList[i]._isSet;

            TempSaveData._currAidDatasList.Add(_aidData);
        }

        // 맵 타일 저장
        for (int i = 0; i < _tileMakerCS.TileMapList.Count; i++)
        {
            tileMapDate TempTileMapDate = new tileMapDate();

            TempTileMapDate._isBlockade = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isBlockade;
            TempTileMapDate._isIssueIcon = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isIssueIcon;
            TempTileMapDate._isSafetyEff = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isSafetyEff;
            TempTileMapDate._isShop = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isShop;
            TempTileMapDate._isSpShop = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isSpShop;
            TempTileMapDate._SafetyValue = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._SafetyValue;
            TempTileMapDate._tileIndex = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._tileIndex;
            TempTileMapDate._isIssue = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._isIssue;
            TempTileMapDate._isCrime = _tileMakerCS.TileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.activeSelf;

            TempSaveData._tileMapList[i] = TempTileMapDate;
        }

        // NPC 정보 저장
        for (int i = 0; i < _npcSysMgrCS._npcList.Length; i++)
        {
            if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Hamicon)
            {
                TempSaveData._HamIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                TempSaveData._HamIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                TempSaveData._HamIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                TempSaveData._HamSaveData = _npcSysMgrCS._HamRootBox;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Jeonicon)
            {
                TempSaveData._JeonIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                TempSaveData._JeonIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                TempSaveData._JeonIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                TempSaveData._JeonSaveData = _npcSysMgrCS._JeonRootBox;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Wishicon)
            {
                TempSaveData._WishIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                TempSaveData._WishIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                TempSaveData._WishIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Youngicon)
            {
                TempSaveData._YoungIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                TempSaveData._YoungIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                TempSaveData._YoungIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
                TempSaveData._YoungSaveData = _npcSysMgrCS._YoungRootBox;
            }
        }

        // 기타
        #region
        // 스토리 진행 관련
        TempSaveData._currEventID = _eventSysCS._currEventID;

        // 플레이어 스테이터스
        TempSaveData._clueTokenValue = _playerInfoCS._clueTokenValue;
        TempSaveData._isAlive = _playerInfoCS._isAlive;
        TempSaveData._isTurn = _playerInfoCS._isTurn;
        TempSaveData._daleyTurnCount = _playerInfoCS._daleyTurnCount;
        TempSaveData._reasoningValue = _playerInfoCS._reasoningValue;

        TempSaveData._currTile = _playerInfoCS._currTile;
        TempSaveData._tempCurrTile = _playerInfoCS._tempCurrTile;
        TempSaveData._currActPoint = _playerInfoCS._currActPoint;
        TempSaveData._currTrunPoint = _playerInfoCS._currTrunPoint;

        TempSaveData._currHP = _playerInfoCS._currHP;
        TempSaveData._MaxHP = _playerInfoCS._MaxHP;
        TempSaveData._atkPoint = _playerInfoCS._atkPoint;
        #endregion

        _sceneMgrCS.SaveData(TempSaveData, value);

        _uIMgrCS.SettingSaveUi(_uIMgrCS._SaveOfLoadUIGO);
    }
}
