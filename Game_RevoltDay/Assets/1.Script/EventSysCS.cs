using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSysDate
{
    public int _eventID_Index;
    public string _playType_Index;
    public int _clueToken_Index;
    public int _eventTile_Index;
    public int _currEventID_Index;
    public int _nextEventID_Index;
    public string _selectIcon_Index;
    public int _settingTile_Index;
    public string _talkScene_Index;
    public int _duel_Index;
    public string _reasoning_Index;
}

public class EventSysCS : MonoBehaviour {
    public GameObject _gameMgr;

    public List<EventSysDate> _eventSysDatesList = new List<EventSysDate>();
    public int _eventValue;
    public int _currEventID;
    public GameObject[] _iconList;

    public UIMgr _uIMgrCS;
    public PlayerInfoCS _playerInfoCS;
    public TileMakerCS _tileMakerCS;
    private List<GameObject> _tileMapList;

    private NpcSysMgr _npcSysMgr;

    public bool _isStop;
    private void Awake()
    {
        _npcSysMgr = GameObject.Find("NpcMgr").GetComponent<NpcSysMgr>();
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/EventTable");

        for (var i = 0; i < date.Count; i++)
        {
            EventSysDate tempEventSys = new EventSysDate();
            tempEventSys._eventID_Index = (int)date[i]["eventID_Index"];
            tempEventSys._playType_Index = (string)date[i]["playType_Index"];
            tempEventSys._clueToken_Index = (int)date[i]["clueToken_Index"];
            tempEventSys._eventTile_Index = (int)date[i]["eventTile_Index"];
            tempEventSys._currEventID_Index = (int)date[i]["currEventID_Index"];
            tempEventSys._nextEventID_Index = (int)date[i]["nextEventID_Index"];
            tempEventSys._selectIcon_Index = (string)date[i]["selectIcon_Index"];
            tempEventSys._settingTile_Index = (int)date[i]["settingTile_Index"];
            tempEventSys._talkScene_Index = (string)date[i]["talkScene_Index"];
            tempEventSys._duel_Index = (int)date[i]["duel_Index"];
            tempEventSys._reasoning_Index = (string)date[i]["reasoning_Index"];
            _eventSysDatesList.Add(tempEventSys);
        }

        _eventValue = _eventSysDatesList.Count;
        _tileMapList = _tileMakerCS.TileMapList;

        _currEventID = 0;
        if (_gameMgr.GetComponent<SaveSys>()._saveFile.isSaveData) saveLead();
    }

    public void saveLead()
    {
        _currEventID = _gameMgr.GetComponent<SaveSys>()._saveFile._currEventID;
    }

    private void Update()
    {
        if (!_isStop &&
            !_uIMgrCS._TalkMgr.activeSelf &&
            !_uIMgrCS._DuelMgr.activeSelf &&
            !_uIMgrCS._SearchMgr.activeSelf &&
            !_uIMgrCS._ReasoningMgr.activeSelf) eventSys();
    }

    public void eventSys()
    {
        if (_currEventID == 0) _currEventID = _eventSysDatesList[_currEventID]._currEventID_Index;

        for (int i = 0; i < _eventSysDatesList.Count; i++)
        {
            if (_eventSysDatesList[i]._eventID_Index == _currEventID)
            {
                _currEventID = i;

                break;
            }
        }

        if (_eventSysDatesList[_currEventID]._playType_Index == "tile" &&
            _eventSysDatesList[_currEventID]._eventTile_Index != 0)
        {
            CrimeShowSys(_eventSysDatesList[_currEventID]._eventTile_Index);
        }
        else if (_eventSysDatesList[_currEventID]._playType_Index == "tile")
        {
            CrimeShowNot();
        }

        if (_eventSysDatesList[_currEventID]._playType_Index == "clueToken" &&
            _eventSysDatesList[_currEventID]._clueToken_Index > 0 &&
            _eventSysDatesList[_currEventID]._eventTile_Index != 0)
        {
            CrimeCheck();
        }

        if (_eventSysDatesList[_currEventID]._playType_Index == "gameStart")
        {
            eventStart();
        }
        else if (_eventSysDatesList[_currEventID]._playType_Index == "null")
        {
            eventStart();
        }
        else if
        (_eventSysDatesList[_currEventID]._playType_Index == "tile" &&
        _eventSysDatesList[_currEventID]._eventTile_Index == _playerInfoCS._currTile)
        {
            
            eventStart();
        }
        else if
        (_eventSysDatesList[_currEventID]._playType_Index == "clueToken" &&
        _eventSysDatesList[_currEventID]._clueToken_Index <= _playerInfoCS._clueTokenValue &&
        _eventSysDatesList[_currEventID]._eventTile_Index == _playerInfoCS._currTile)
        {
            // 특정 타일에 도착했을 경우 조건 만큼의 단서를 초기화하고 이벤트를 실행한다.
            _playerInfoCS._clueTokenValue = 0;
            eventStart();
        }
    }

    public void CrimeShowNot()
    {
        for (int i = 0; i < _tileMapList.Count; i++)
        {
            _tileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(false);
        }
    }

    public void CrimeShowSys(int value)
    {
        for (int i = 0; i < _tileMapList.Count; i++)
        {
            if (value != i) continue;
            else _tileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(true);
        }
    }

    public void CrimeCheck()
    {
        if (_playerInfoCS._clueTokenValue >= _eventSysDatesList[_currEventID]._clueToken_Index) // 조건되는 토큰을 가지고 있는 경우
        {
            _tileMapList[_eventSysDatesList[_currEventID]._eventTile_Index].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(true); // 타일을 켜준다.
        }
        else if ((_eventSysDatesList[_currEventID]._clueToken_Index > 0)) // 조건에 충족하지 못한 경우 다 꺼준다.
        {
            for (int i = 0; i < _tileMapList.Count; i++)
            {
                _tileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(false);
            }
        }
    }

    public void eventStart()
    {
        if (_eventSysDatesList[_currEventID]._selectIcon_Index != "null") tileSetting();
        else if (_eventSysDatesList[_currEventID]._talkScene_Index != "null") talkEventStart();
        else if (_eventSysDatesList[_currEventID]._duel_Index != 0) duelEventStart();
        else if (_eventSysDatesList[_currEventID]._reasoning_Index!= "null") reasoningEventStart(_eventSysDatesList[_currEventID]._reasoning_Index);

        _currEventID = _eventSysDatesList[_currEventID]._nextEventID_Index;
    }

    public void tileSetting()
    {
        eNpcType tempName = eNpcType.normalEnemy;
        for (int i = 0; i < _iconList.Length; i++)
        {
            if (_eventSysDatesList[_currEventID]._selectIcon_Index != "PlayerIcon")
            {
                switch (_eventSysDatesList[_currEventID]._selectIcon_Index)
                {
                    case "Hamicon":
                        tempName = eNpcType.Hamicon;
                        break;
                    case "Jeonicon":
                        tempName = eNpcType.Jeonicon;
                        break;
                    case "Wishicon":
                        tempName = eNpcType.Wishicon;
                        break;
                    case "Youngicon":
                        tempName = eNpcType.Youngicon;
                        break;
                    case "Parkicon":
                        tempName = eNpcType.Parkicon;
                        _playerInfoCS._isParkIcon = true;
                        _playerInfoCS._currPlayerImg.sprite = _iconList[2].GetComponent<PlayerInfoCS>()._currPlayerImg.sprite;
                        break;
                    default:
                        Debug.Log("tileSetting() : 잘못된 아이콘 정보 입니다.");
                        break;
                }
                _npcSysMgr.sttingNPC(_eventSysDatesList[_currEventID]._settingTile_Index, tempName);
                return;
            }

            if (_eventSysDatesList[_currEventID]._selectIcon_Index != "PlayerIcon" &&
                _eventSysDatesList[_currEventID]._settingTile_Index == 444)
            {

                switch (_eventSysDatesList[_currEventID]._selectIcon_Index)
                {
                    case "Hamicon":
                        tempName = eNpcType.Hamicon;
                        break;
                    case "Jeonicon":
                        tempName = eNpcType.Jeonicon;
                        break;
                    case "Wishicon":
                        tempName = eNpcType.Wishicon;
                        break;
                    case "Youngicon":
                        tempName = eNpcType.Youngicon;
                        break;
                    default:
                        Debug.Log("tileSetting() : 잘못된 아이콘 정보 입니다.");
                        break;
                }
                _npcSysMgr.DieNpc(tempName);
                return;
            }

            if (_iconList[i].name == _eventSysDatesList[_currEventID]._selectIcon_Index)
            {


                _iconList[i].GetComponent<PlayerInfoCS>().setTileValeu(_eventSysDatesList[_currEventID]._settingTile_Index);
                return;
            }
        }
    }

    public void talkEventStart()
    {
        _uIMgrCS.StartTalk();
        ReadCsvCS tempReadCsvCS = _uIMgrCS._TalkMgr.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<ReadCsvCS>();
        tempReadCsvCS._tableType = eTableType.Crime;
        tempReadCsvCS._eSearchSelectType = eSearchSelectType.Non;
        tempReadCsvCS._crimeSceneName = _eventSysDatesList[_currEventID]._talkScene_Index;
        tempReadCsvCS.startDateLoad();
    }

    public void duelEventStart()
    {
        Debug.Log("격투 이벤트 시작");
        _uIMgrCS.StartDuel();
        _uIMgrCS._DuelMgr.GetComponent<DuelSysCS>().DuelStartSys(eNpcType.normalEnemy, _eventSysDatesList[_currEventID]._duel_Index, true, eTableType.Non);
    }

    public void reasoningEventStart(string reasoningName)
    {
        Debug.Log("추리 이벤트 시작");
        _uIMgrCS.StartReasoning();
        _uIMgrCS._ReasoningMgr.GetComponent<ReasoningMgrCS>().settingScene(reasoningName);
    }
}
