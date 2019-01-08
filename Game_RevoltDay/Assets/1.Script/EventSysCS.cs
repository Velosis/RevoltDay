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
    public string _duel_Index;
    public string _reasoning_Index;
}

public class EventSysCS : MonoBehaviour {
    public List<EventSysDate> _eventSysDatesList = new List<EventSysDate>();
    public int _eventValue;
    public int _currEventID;
    public GameObject[] _iconList;

    public UIMgr _uIMgrCS;
    public PlayerInfoCS _playerInfoCS;
    public TileMakerCS _tileMakerCS;
    private List<GameObject> _tileMapList;

    private NpcSysMgr _npcSysMgr;

    private void Awake()
    {
        _npcSysMgr = GameObject.Find("NpcMgr").GetComponent<NpcSysMgr>();

        _currEventID = 0;

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
            tempEventSys._duel_Index = (string)date[i]["duel_Index"];
            tempEventSys._reasoning_Index = (string)date[i]["reasoning_Index"];
            _eventSysDatesList.Add(tempEventSys);
        }

        _eventValue = _eventSysDatesList.Count;
    }

    private void Start()
    {
        _tileMapList = _tileMakerCS.TileMapList;
    }

    private void Update()
    {
        //if (!_uIMgrCS._TalkMgr.activeSelf &&
        //    !_uIMgrCS._DuelMgr.activeSelf &&
        //    !_uIMgrCS._SearchMgr.activeSelf ) eventSys();
    }

    public void eventSys()
    {
        _currEventID = _eventSysDatesList[_currEventID]._currEventID_Index;

        if (_eventSysDatesList[_currEventID]._playType_Index == "gameStart")
        {
            eventStart();
        }
        else if (_eventSysDatesList[_currEventID]._playType_Index == "null")
        {
            eventStart();
        }else if (
            _eventSysDatesList[_currEventID]._playType_Index == "tile" &&
            _eventSysDatesList[_currEventID]._eventTile_Index == _playerInfoCS._currTile)
        {
            eventStart();
        }

        for (int i = 0; i < _tileMapList.Count; i++)
        {
            _tileMapList[i].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(false);
        }

        if (_eventSysDatesList[_currEventID]._eventTile_Index != 0 &&
            !_tileMapList[_eventSysDatesList[_currEventID]._eventTile_Index].GetComponent<TileMapDataCS>()._CrimeImgGO.activeSelf)
        {
            _tileMapList[_eventSysDatesList[_currEventID]._eventTile_Index].GetComponent<TileMapDataCS>()._CrimeImgGO.SetActive(true);
        }
    }

    public void eventStart()
    {
        if (_eventSysDatesList[_currEventID]._selectIcon_Index != "null") tileSetting();
        else if (_eventSysDatesList[_currEventID]._talkScene_Index != "null") talkEventStart();
        else if (_eventSysDatesList[_currEventID]._duel_Index != "null") duelEventStart();
        else if (_eventSysDatesList[_currEventID]._reasoning_Index!= "null") reasoningEventStart();

        _currEventID = _eventSysDatesList[_currEventID]._nextEventID_Index;
    }

    public void tileSetting()
    {
        for (int i = 0; i < _iconList.Length; i++)
        {
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
        Debug.Log("결투 이벤트 시작");
    }

    public void reasoningEventStart()
    {
        Debug.Log("추리 이벤트 시작");
    }


}
