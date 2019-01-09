using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDateNamespace;

public class SaveSys : MonoBehaviour {
    public SaveData _saveFile;
    public PlayerInfoCS _playerInfoCS;
    public EventSysCS _eventSysCS;
    public NpcSysMgr _npcSysMgrCS;

    public bool _isDeleteSave;

    // 플레이어 정보


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("_saveFile._currEventID : " + _saveFile._currEventID);

    }

    private void Update()
    {
        if (_isDeleteSave)
        {
            _isDeleteSave = false;
        }
    }

    public void saveSys()
    {
        // 스토리 진행 관련
        _saveFile._currEventID = _eventSysCS._currEventID;

        // 플레이어 스테이터스
        _saveFile._clueTokenValue = _playerInfoCS._clueTokenValue;
        _saveFile._isAlive = _playerInfoCS._isAlive;
        _saveFile._isTurn = _playerInfoCS._isTurn;
        _saveFile._daleyTurnCount = _playerInfoCS._daleyTurnCount;
        _saveFile._reasoningValue = _playerInfoCS._reasoningValue;

        _saveFile._currTile = _playerInfoCS._currTile;
        _saveFile._tempCurrTile = _playerInfoCS._tempCurrTile;
        _saveFile._currActPoint = _playerInfoCS._currActPoint;
        _saveFile._currTrunPoint = _playerInfoCS._currTrunPoint;

        _saveFile._currHP = _playerInfoCS._currHP;
        _saveFile._MaxHP = _playerInfoCS._MaxHP;
        _saveFile._atkPoint = _playerInfoCS._atkPoint;

        // NPC 정보 저장
        for (int i = 0; i < _npcSysMgrCS._npcList.Length; i++)
        {
            if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Hamicon)
            {
                _saveFile._HamIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFile._HamIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFile._HamIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Jeonicon)
            {
                _saveFile._JeonIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFile._JeonIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFile._JeonIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Parkicon)
            {
                _saveFile._ParkIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFile._ParkIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFile._ParkIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
            else if (_npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == eNpcType.Wishicon)
            {
                _saveFile._WishIcon._isAlive = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isAlive;
                _saveFile._WishIcon._isTurn = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._isTurn;
                _saveFile._WishIcon._currTile = _npcSysMgrCS._npcList[i].GetComponent<PlayerInfoCS>()._currTile;
            }
        }
        
    }
}
