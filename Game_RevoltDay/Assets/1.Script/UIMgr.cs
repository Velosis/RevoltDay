using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public enum eUiName
{
    all,
    SearchButton,
    ItemButton,
    SafetyButton,
    WaitButton,
    ShopButton,
}

public enum eSearchSelectType
{
    Non,
    Duel,
}

public class eventDate
{
    public int _eventType_Index = 0;
    public string _IssueName_Index = "";
    public string _sceneID_Index = "";
    public string _type_Index = "";
}

public class UIMgr : MonoBehaviour {
    static public bool _sNpeTurnEnd = false;
    public GameObject _TileUIMgr;

    public ScreenEffMgrCS _screenEffMgrCS;
    public EventSysCS _eventSysCS;
    public SaveSys _saveSysCS;
    public GameObject _SaveOfLoadUIGO;

    private GameObject _search_buttonUi;
    private GameObject _Item_buttonUi;
    private GameObject _safety_buttonUi;
    private GameObject _wait_buttonUi;
    private GameObject _shop_buttonUi;
    public GameObject _loadingImg;
    public string _loadingText;

    public GameObject _SearchMgr;
    public GameObject _TalkMgr;
    public GameObject _DuelMgr;
    public GameObject _ReasoningMgr;

    public GameObject _ShopMgr;
    public GameObject _OptionMgr;

    public GameObject _StateMgr;
    private GameObject _StateTabButtonGO;
    private GameObject _EquipTabButtonGO;
    private GameObject _ItemTabButtonGO;
    private GameObject _CardTabButtonGO;

    private bool _isSafety;
    public delegate void SafetyUiView(bool isUI);
    public static event SafetyUiView _isSafetyUI;

    public delegate void TileTurnUpdate();
    public static event TileTurnUpdate _tileTurnUpdate;
    public bool _isIssueEvent;

    private PlayerInfoCS _playerInfoCS;
    private List<GameObject> _tileMapList;
    private TileMapDataCS _tileMapDataCS;
    private NpcSysMgr _npcSysMgr;

    public GameObject[] _SearchSelectList;
    public Sprite[] _SearchSpriteList;

    public Dictionary<string, eventDate> _normalTableList = new Dictionary<string, eventDate>();
    public Dictionary<string, eventDate> _issueTableList = new Dictionary<string, eventDate>();
    public Dictionary<string, eventDate> _blockadeTableList = new Dictionary<string, eventDate>();

    public int _SaveSelectValue = -1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _SaveSelectValue = -1;

        _TileUIMgr = GameObject.Find("DonTileUI");

        IssueTableRead();
        _DuelMgr.GetComponent<DuelSysCS>().readUnitTable();

        _loadingImg.SetActive(false);
        _isSafety = true;
        _search_buttonUi = GameObject.Find("SearchButton");
        _Item_buttonUi = GameObject.Find("ItemButton");
        _safety_buttonUi = GameObject.Find("SafetyButton");
        _wait_buttonUi = GameObject.Find("WaitButton");
        _shop_buttonUi = GameObject.Find("ShopButton");

        _playerInfoCS = GameObject.Find("PlayerIcon").GetComponent<PlayerInfoCS>();
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
        _npcSysMgr = GameObject.Find("NpcMgr").GetComponent<NpcSysMgr>();
        
        _StateMgr.SetActive(false);

        _shop_buttonUi.SetActive(false);
        _isIssueEvent = false;

        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(false);
        _ReasoningMgr.SetActive(false);
        _OptionMgr.SetActive(false);

        for (int i = 0; i < _SearchSelectList.Length; i++)
        {
            _SearchSelectList[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public IEnumerator loadingEff()
    {
        _loadingImg.SetActive(true);
        float tempCurrTimemr = 0.0f;
        while (tempCurrTimemr <= 1.0f)
        {
            tempCurrTimemr += 0.3f;
            _loadingImg.transform.GetChild(1).GetComponent<Text>().text = _loadingText + ".";
            yield return new WaitForSeconds(0.2f);
            _loadingImg.transform.GetChild(1).GetComponent<Text>().text = _loadingText + "..";
            yield return new WaitForSeconds(0.2f);
            _loadingImg.transform.GetChild(1).GetComponent<Text>().text = _loadingText + "...";
            yield return new WaitForSeconds(0.2f);
        }
        int tempPlayerCurrTile = _playerInfoCS._currTile;
        _tileMapDataCS = _tileMapList[tempPlayerCurrTile].GetComponent<TileMapDataCS>();

        for (int i = 0; i < 3; i++)
        {
            SearchUISetting(i, eTableType.normal);
        }

        if (_tileMapDataCS._isIssueIcon)
        {
            SearchUISetting(0, eTableType.Issue);
        }
        else if (_tileMapDataCS._isBlockade)
        {
            SearchUISetting(0, eTableType.Blockade);
        }

        _loadingImg.SetActive(false);



        yield return null;
    }

    public void isOnOff (GameObject isUIGo)
    {
        isUIGo.SetActive(!isUIGo.activeSelf);
    }

    private void IssueTableRead()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/IssueTable");

        for (var i = 0; i < date.Count; i++)
        {
            eventDate tempEventDate = new eventDate();
            tempEventDate._eventType_Index = (int)date[i]["eventType_Index"];
            tempEventDate._IssueName_Index = (string)date[i]["IssueName_Index"];
            tempEventDate._sceneID_Index = (string)date[i]["sceneID_Index"];
            tempEventDate._type_Index = (string)date[i]["type_Index"];

            if ((string)date[i]["type_Index"] == "긴급") _issueTableList.Add((string)date[i]["IssueName_Index"], tempEventDate);
            else if ((string)date[i]["type_Index"] == "일반") _normalTableList.Add((string)date[i]["IssueName_Index"], tempEventDate);
            else if ((string)date[i]["type_Index"] == "봉쇄") _blockadeTableList.Add((string)date[i]["IssueName_Index"], tempEventDate);
        }
    }

    public void ScenesChange(string _str)
    {
        SceneManager.LoadScene(_str);

        if (_SaveSelectValue == -1)
            Destroy(gameObject);
    }

    public void SettingSaveUi(GameObject _gameObject)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_saveSysCS._saveFileArr[i].isSaveData) _gameObject.transform.GetChild(3 + i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = _saveSysCS._saveFileArr[i].SaveDay;
            else _gameObject.transform.GetChild(3 + i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "저장 정보 없음";
        }
    }

    public void LoadSelectCheck(int value)
    {
        if (_OptionMgr.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text != "불러오기 옵션") return;

        _SaveSelectValue = value;
        Destroy(_TileUIMgr);
        ScenesChange("0.Tilte");
    }

    public void SaveSelectCheck(int value)
    {
        if (_OptionMgr.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text != "저장 옵션") return;

        _saveSysCS.saveSys(value);
    }

    public void SaveOfLoadUiOnOff(GameObject _gameObject)
    {
        string tempStr = EventSystem.current.currentSelectedGameObject.name;
        if (tempStr == "SaveButton") // 저장 버튼
        {
            _OptionMgr.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "저장 옵션";

            SettingSaveUi(_SaveOfLoadUIGO);
        }
        else if (tempStr == "LoadButton") // 불러오기 버튼
        {
            _OptionMgr.transform.GetChild(10).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "불러오기 옵션";
            _SaveSelectValue = -1;
            SettingSaveUi(_SaveOfLoadUIGO);
        }
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    public void StartOption()
    {
        _OptionMgr.SetActive(true);
    }

    public void EndOption()
    {
        _OptionMgr.SetActive(false);

    }

    public void StartShop()
    {
        _ShopMgr.SetActive(true);
        _ShopMgr.GetComponent<ShopMgr>().ShopStart();
    }

    public void EndShop()
    {
        _ShopMgr.SetActive(false);
    }

    public void StartStateUI()
    {
        _StateMgr.SetActive(true);
    }

    public void EndStateUI()
    {
        _StateMgr.SetActive(false);
    }

    public void TurnEndSys()
    {
        if (_sNpeTurnEnd) return;

        _sNpeTurnEnd = true;
        _playerInfoCS.PlayerTileXZ();
        StartCoroutine(_npcSysMgr.NpcAct());
    }

    public void StartDuel()
    {
        _playerInfoCS.PlayerMoveNot(false);
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(true);
        _screenEffMgrCS.startEff();
    }

    public void EndDuel()
    {
        _playerInfoCS.PlayerMoveNot(true);
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(false);
        _screenEffMgrCS.startEff();
    }

    public void StartTalk()
    {
        _playerInfoCS.PlayerMoveNot(false);

        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(true);
        _screenEffMgrCS.startEff();
    }

    public void EndTalk()
    {
        _playerInfoCS.PlayerMoveNot(true);

        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _screenEffMgrCS.startEff();
    }

    public void StartReasoning()
    {
        _playerInfoCS.PlayerMoveNot(false);

        _TalkMgr.SetActive(false);
        _ReasoningMgr.SetActive(true);
        _screenEffMgrCS.startEff();
    }

    public void EndReasoning()
    {
        _playerInfoCS.PlayerMoveNot(true);

        _ReasoningMgr.SetActive(false);
        _screenEffMgrCS.startEff();
    }

    public void tileTurnUpdate()
    {
        _eventSysCS.CrimeCheck();
        _tileTurnUpdate();

        if (_isIssueEvent) _isIssueEvent = false;
    }

    public void isSafetyUI()
    {
        if (_isSafety) _isSafetyUI(true);
        else _isSafetyUI(false);

        _isSafety = !_isSafety;
    }

    public void isOnSearchMgr()
    {
        if (!_SearchMgr.activeSelf)
        {
            _playerInfoCS.setActPoint(1);
            StartCoroutine(loadingEff());
        }

        _SearchMgr.SetActive(!_SearchMgr.activeSelf);
    }

    public void SearchUISetting(int value, eTableType type)
    {
        Image tempSprite = _SearchSelectList[value].transform.GetChild(2).GetComponent<Image>();
        Text tempNameText = _SearchSelectList[value].transform.GetChild(3).GetComponent<Text>();
        Text tempTypeText = _SearchSelectList[value].transform.GetChild(4).GetComponent<Text>();
        Text tempTableType = _SearchSelectList[value].transform.GetChild(5).GetComponent<Text>();

        List<string> keyList = null;
        string tempRandStr = null;
        eventDate tempEvent = null;
        switch (type)
        {
            case eTableType.normal:
                tempTableType.text = "";
                keyList = new List<string>(_normalTableList.Keys);
                tempRandStr = keyList[Random.Range(0, keyList.Count)];
                _normalTableList.TryGetValue(tempRandStr, out tempEvent);
                break;
            case eTableType.Issue:
                tempTableType.text = "긴급!";
                keyList = new List<string>(_issueTableList.Keys);
                tempRandStr = keyList[Random.Range(0, keyList.Count)];
                _issueTableList.TryGetValue(tempRandStr, out tempEvent);

                break;
            case eTableType.Blockade:
                tempTableType.text = "봉쇄 해체";
                keyList = new List<string>(_blockadeTableList.Keys);
                tempRandStr = keyList[Random.Range(0, keyList.Count)];
                _blockadeTableList.TryGetValue(tempRandStr, out tempEvent);

                break;
            case eTableType.Crime:
                break;
            default:
                break;
        }

        _SearchSelectList[value].GetComponent<SearchSelectData>()._currTableType = type;
        if (tempEvent._eventType_Index == 1)
        {
            _SearchSelectList[value].GetComponent<SearchSelectData>()._currSelectType = eSearchSelectType.Duel;
            tempTypeText.text = "-결투-";
            tempSprite.sprite = _SearchSpriteList[0];
        }

        _SearchSelectList[value].GetComponent<SearchSelectData>()._sceneID = tempEvent._sceneID_Index;
        
        tempNameText.text = tempRandStr;
    }

    public void isUiOnOff(eUiName uiName, bool isOn)
    {
        if (uiName == eUiName.all)
        {
            _search_buttonUi.SetActive(isOn);
            _Item_buttonUi.SetActive(isOn);
            _safety_buttonUi.SetActive(isOn);
            _wait_buttonUi.SetActive(isOn);
            _shop_buttonUi.SetActive(isOn);
            return;
        }

        switch (uiName)
        {
            case eUiName.SearchButton:
                _search_buttonUi.SetActive(isOn);
                break;
            case eUiName.ItemButton:
                _Item_buttonUi.SetActive(isOn);
                break;
            case eUiName.SafetyButton:
                _safety_buttonUi.SetActive(isOn);
                break;
            case eUiName.WaitButton:
                _wait_buttonUi.SetActive(isOn);
                break;
            case eUiName.ShopButton:
                _shop_buttonUi.SetActive(isOn);
                break;
            default:
                break;
        }
    }
}
