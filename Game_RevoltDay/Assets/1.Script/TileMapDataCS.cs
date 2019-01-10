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
        if (_isIssueIcon && !_isBlockade) setSafetyBer(1);

        if (!_isBlockade && !_uIMgrCS._isIssueEvent && Random.Range(0, 100) <= 20 && _playerData.GetComponent<PlayerInfoCS>()._currTrunPoint >= 3)
        {
            Debug.Log("_isBlockade : " + _isBlockade.ToString());
            _tileMapList[Random.Range(0, _tileMapList.Count)].GetComponent<TileMapDataCS>().setIssueEvent();
        }
    }

    public void setIssueEvent()
    {
        _playerData.GetComponent<PlayerInfoCS>()._currTrunPoint = 0;
        _uIMgrCS._isIssueEvent = true;
        _isIssue = true;
        isIssue(true);
        if (_isCrime) _CrimeImgGO.SetActive(true);
    }

    public void setBlockade(bool isBlock)
    {
        _isBlockade = isBlock;
        if (_isBlockade) transform.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        else transform.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

        _uIMgrCS._isIssueEvent = false;
        _isIssue = false;
        isIssue(false);

        if (_isBlockade) _SafetyValue = 0.0f;
    }

    private bool SafetyCheck()
    {
        return (_SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value > 0.0f);
    }

    private void Update()
    {
        if (!_isShop && _tileIndex == 1) _isShop = true;
        if (!_isSpShop && _tileIndex == 7) _isSpShop = true;

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
        for (int i = 0; i < value; i++)
        {
            _SafetyValue = _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value;

            if (value < 0 && _SafetyValue >= (1.0f / 5.0f)) _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value -= ((1.0f / 5.0f) * 1.0f);
            else if (value > 0 && _SafetyValue < 1.0f) _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value += ((1.0f / 5.0f) * 1.0f);
        }
        _SafetyValue = _SafetyImgGO.transform.GetChild(0).GetComponent<Slider>().value;
        if (_SafetyValue >= 1.0f) setBlockade(true);
    }
}
