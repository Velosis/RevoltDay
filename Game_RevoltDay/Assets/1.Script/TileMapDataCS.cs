using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMapDataCS : MonoBehaviour {
    private UIMgr _uIMgrCS;
    private List<GameObject> _tileMapList;


    public GameObject _playerData;
    private GameObject _TileBG;
    public GameObject _IssueImgGO;
    public GameObject _CrimeImgGO;
    public GameObject _SafetyImgGO;
    public GameObject _SafetyEffImgGO;
    public int _tileIndex = 0;
    public float _SafetyValue;

    public bool _isSafetyEff;
    public bool _isBlockade;
    public bool _isIssueIcon;

    public bool _isShop;
    public bool _isSpShop;

    public bool _isIssue;
    public bool _isCrime;

    public bool _isSafetyCheck = false;

    private void Awake()
    {
        _uIMgrCS = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;

        _isSafetyEff = false;
        _isBlockade = false;
        _isIssueIcon = false;

        _isShop = false;
        _isSpShop = false;

        _playerData = GameObject.Find("PlayerIcon");
        _TileBG = transform.GetChild(0).gameObject;
        _TileBG.SetActive(false);

        _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value = 0.0f;
        _SafetyImgGO.SetActive(false);
        _SafetyEffImgGO.SetActive(false);
        _IssueImgGO.SetActive(false);
        _CrimeImgGO.SetActive(false);

        _isIssue = false;
    }

    public void saveSetting()
    {
        _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value = _SafetyValue;
        isIssue(_isIssue);
        if (_isSafetyEff) StartCoroutine(safetyEff());
    }

    public void tileTrunUpdate()
    {
        if (!_isSafetyCheck && _isIssueIcon && !_isBlockade) setSafetyBer(1);

        int RandTile = Random.Range(0, _tileMapList.Count);
        if (!_tileMapList[RandTile].GetComponent<TileMapDataCS>()._isBlockade &&
            !_tileMapList[RandTile].GetComponent<TileMapDataCS>()._uIMgrCS._isIssueEvent &&
            Random.Range(0, 100) <= 20 &&
            _tileMapList[RandTile].GetComponent<TileMapDataCS>()._playerData.GetComponent<PlayerInfoCS>()._currTrunPoint >= 3)
        {
            _tileMapList[RandTile].GetComponent<TileMapDataCS>().setIssueEvent();
            _tileMapList[RandTile].GetComponent<TileMapDataCS>()._isSafetyCheck = true;
            return;
        }
        _isSafetyCheck = false;
    }

    public void setIssueEvent()
    {
        _playerData.GetComponent<PlayerInfoCS>()._currTrunPoint = 0;
        _uIMgrCS._isIssueEvent = true;
        _isIssue = true;
        isIssue(_isIssue);

        _IssueImgGO.GetComponent<Canvas>().overrideSorting = true;

        if (_isCrime) _CrimeImgGO.SetActive(true);
    }

    public void setBlockade(bool isBlock)
    {
        _isBlockade = isBlock;
        if (_isBlockade) transform.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        else transform.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

        _uIMgrCS._isIssueEvent = false;
        _isIssue = false;
        isIssue(_isIssue);

        _SafetyValue = 0.0f;
        _IssueImgGO.SetActive(false);
        if (!isBlock)
        {
            StopAllCoroutines();
            _isSafetyEff = false;
        }
    }

    private bool SafetyCheck()
    {
        return (_SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value > 0.0f);
    }

    private void Update()
    {
        if (!_isBlockade && !_isSafetyEff && SafetyCheck()) StartCoroutine(safetyEff());
    }

    public IEnumerator safetyEff()
    {
        _isSafetyEff = true;
        _SafetyEffImgGO.SetActive(true);
        Color tempColor = Color.red;
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            timer += Time.deltaTime;
            tempColor.a = 1.0f - timer;
            _SafetyEffImgGO.GetComponent<Image>().color = tempColor;

            yield return null;
        }

        _isSafetyEff = false;
        yield break;
    }

    public void isIssue(bool isValue)
    {
        _isIssueIcon = isValue;
        _IssueImgGO.SetActive(_isIssueIcon);
    }

    public void isSafetyUI(bool isUI)
    {
        _TileBG.SetActive(isUI);
        if (SafetyCheck()) _SafetyImgGO.SetActive(isUI);
    }

    public void setSafetyBer(float value)
    {
        if (value > 0) _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value += 0.2f * Mathf.Abs(value);
        else if (value < 0) _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value -= 0.2f * Mathf.Abs(value);
        else if (value == 0.0f) _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value = 0.0f;

        _SafetyValue = _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value;
        if (_SafetyValue >= 1.0f) setBlockade(true);
        else if (_SafetyValue <= 0.0f)
        {
            _SafetyImgGO.SetActive(false);
            _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value = 0.0f;
            isIssue(false);
        }
    }

    private void OnDisable()
    {
        _IssueImgGO.GetComponent<Canvas>().overrideSorting = false;
    }
}
