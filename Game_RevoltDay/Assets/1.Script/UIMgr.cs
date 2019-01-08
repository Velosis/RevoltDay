using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eUiName
{
    all,
    SearchButton,
    ItemButton,
    SafetyButton,
    WaitButton,
    ShopButton,
    TalkButton
}

public enum eSearchSelectType
{
    Non,
    Duel,
    Reasoning
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

    private GameObject _search_buttonUi;
    private GameObject _Item_buttonUi;
    private GameObject _safety_buttonUi;
    private GameObject _wait_buttonUi;
    private GameObject _shop_buttonUi;
    private GameObject _talk_buttonUi;
    public GameObject _loadingImg;
    public string _loadingText;

    public GameObject _SearchMgr;
    public GameObject _TalkMgr;
    public GameObject _DuelMgr;

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

    private void Awake()
    {
        IssueTableRead();

        _loadingImg.SetActive(false);
        _isSafety = true;
        _search_buttonUi = GameObject.Find("SearchButton");
        _Item_buttonUi = GameObject.Find("ItemButton");
        _safety_buttonUi = GameObject.Find("SafetyButton");
        _wait_buttonUi = GameObject.Find("WaitButton");
        _shop_buttonUi = GameObject.Find("ShopButton");
        _talk_buttonUi = GameObject.Find("TalkButton");

        _playerInfoCS = GameObject.Find("PlayerIcon").GetComponent<PlayerInfoCS>();
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
        _npcSysMgr = GameObject.Find("NpcMgr").GetComponent<NpcSysMgr>();

        _shop_buttonUi.SetActive(false);
        _talk_buttonUi.SetActive(false);
        _isIssueEvent = false;

        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(false);

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
        _loadingImg.SetActive(false);
        yield return null;
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

    public void TurnEndSys()
    {
        if (_sNpeTurnEnd) return;

        _sNpeTurnEnd = true;
        _playerInfoCS.PlayerTileXZ();
        StopAllCoroutines();
        _npcSysMgr._npcActIEnumerator = StartCoroutine(_npcSysMgr.NpcAct());
       
    }

    public void StartDuel()
    {
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(true);
    }

    public void EndDuel()
    {
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
        _DuelMgr.SetActive(false);
    }

    public void StartTalk()
    {
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(true);
    }

    public void EndTalk()
    {
        _SearchMgr.SetActive(false);
        _TalkMgr.SetActive(false);
    }

    public void tileTurnUpdate()
    {
        _tileTurnUpdate();

        if (_isIssueEvent) _isIssueEvent = false;
    }

    public void isSafetyUI()
    {
        if (_isSafety) _isSafetyUI(true);
        else _isSafetyUI(false);

        _isSafety = !_isSafety;

    }

    public void SearchUiSys()
    {

        StartCoroutine(loadingEff());

        int tempPlayerCurrTile = _playerInfoCS._currTile;
        _tileMapDataCS = _tileMapList[tempPlayerCurrTile].GetComponent<TileMapDataCS>();

        for (int i = 0; i < 3; i++)
        {
            SearchUISetting(i, eTableType.normal);
        }

        if (_tileMapDataCS._isIssueIcon)
        {
            SearchUISetting(0, eTableType.Issue);
        }else if (_tileMapDataCS._isBlockade)
        {
            SearchUISetting(0, eTableType.Blockade);
        }
    }

    public void isOnSearchMgr()
    {
        if (!_SearchMgr.activeSelf) _playerInfoCS.setActPoint(1);

        SearchUiSys();
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
        else if (tempEvent._eventType_Index == 2)
        {
            _SearchSelectList[value].GetComponent<SearchSelectData>()._currSelectType = eSearchSelectType.Reasoning;
            tempTypeText.text = "-추리-";
            tempSprite.sprite = _SearchSpriteList[1];
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
            _talk_buttonUi.SetActive(isOn);
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
            case eUiName.TalkButton:
                _talk_buttonUi.SetActive(isOn);
                break;
            default:
                break;
        }
    }
}
