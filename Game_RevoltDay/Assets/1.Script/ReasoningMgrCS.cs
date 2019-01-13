using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasoningMgrCS : MonoBehaviour {

    public UIMgr _uIMgrCS;

    public GameObject _reasoningUIGO;
    public GameObject _checkPopupUI;
    public GameObject _missionPopupUI;
    public GameObject _trueCheckPopUI;
    private Text _stateValueText;

    public GameObject _currScene;
    public GameObject[] _sceneList;
    public int _currClueValue = 0;

    public PlayerInfoCS _playerInfoCS;
    public bool _allTrue = false;
    public int _trueCount = 0;

    private void Awake()
    {
        _stateValueText = _reasoningUIGO.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    private void OnEnable()
    {
        _checkPopupUI.SetActive(false);
        _missionPopupUI.SetActive(false);
        _trueCheckPopUI.SetActive(false);
    }

    public void settingScene(string sceneName)
    {
        _allTrue = false;
        for (int i = 0; i < _sceneList.Length; i++)
        {
            if (_sceneList[i].name == sceneName)
            {
                _sceneList[i].GetComponent<ReasoningSysCS>()._reasoningMgrCS = this;
                _sceneList[i].GetComponent<ReasoningSysCS>().setText();
                _sceneList[i].SetActive(true);
                _currScene = _sceneList[i];
            }
        }

        ReasoningSysCS tempSysCS = _currScene.GetComponent<ReasoningSysCS>();
        for (int i = 0; i < tempSysCS._clueList.Length; i++)
        {
            Debug.Log(tempSysCS._clueList[i]._isTrue);
            if (tempSysCS._clueList[i]._isTrue) _trueCount++;
            else
            {
                _allTrue = false;
                break;
            }
            _allTrue = true;
        }

        setText();
    }

    public void setText()
    {
        if (!_allTrue) _stateValueText.text = "단서 " + _currClueValue.ToString() + " / " + _playerInfoCS._reasoningValue;
        else _stateValueText.text = "단서 " + _currClueValue.ToString() + " / " + _trueCount;

    }

    public void trueCheckUI(bool YesNo)
    {
        if (YesNo) trueSheck();
        else _trueCheckPopUI.SetActive(false);
    }

    public void trueSheck()
    {
        if (_currScene.GetComponent<ReasoningSysCS>()._trueValue <=
            _currClueValue)
        {
            _uIMgrCS.EndReasoning();
            Debug.Log("사건 해결");
        }
        else
        {
            _uIMgrCS.EndReasoning();
            
            Debug.Log("해결 실패!");
        }
    }

    public void selectPoint(GameObject whoPoint)
    {
        ReasoningSysCS tempSysCS = _currScene.GetComponent<ReasoningSysCS>();
        for (int i = 0; i < tempSysCS._clueList.Length; i++)
        {
            if ((!_allTrue && !tempSysCS._clueList[i]._check && tempSysCS._clueList[i]._cluePoint.name == whoPoint.name && _currClueValue != _playerInfoCS._reasoningValue) ||
                (_allTrue && !tempSysCS._clueList[i]._check && tempSysCS._clueList[i]._cluePoint.name == whoPoint.name && _currClueValue != _trueCount))
            {
                tempSysCS.tempCheckGO = whoPoint.transform.GetChild(0).gameObject;
                tempSysCS.tempSelectGO = tempSysCS._clueList[i];
                tempSysCS._reasoningMgrCS._checkPopupUI.transform.GetChild(1).GetComponent<Text>().text = tempSysCS._clueList[i]._popupText;
                tempSysCS._reasoningMgrCS._checkPopupUI.SetActive(true);
                return;
            }
            else if (tempSysCS._clueList[i]._check && tempSysCS._clueList[i]._cluePoint.name == whoPoint.name)
            {
                whoPoint.transform.GetChild(0).gameObject.SetActive(false);
                tempSysCS._clueList[i]._check = false;
                tempSysCS._reasoningMgrCS._currClueValue--;
                tempSysCS._reasoningMgrCS.setText();
                return;
            }
        }
    }

    public void CheckYesNo(bool YesNo)
    {
        ReasoningSysCS tempSysCS = _currScene.GetComponent<ReasoningSysCS>();

        if (YesNo)
        {
            tempSysCS.tempCheckGO.SetActive(true);
            tempSysCS._reasoningMgrCS._currClueValue++;
            tempSysCS._reasoningMgrCS.setText();
            tempSysCS._reasoningMgrCS._checkPopupUI.SetActive(false);
            tempSysCS.tempSelectGO._check = true;
        }
        else
        {
            tempSysCS._reasoningMgrCS._checkPopupUI.SetActive(false);
        }
    }

    public void resetSys()
    {
        ReasoningSysCS tempSysCS = _currScene.GetComponent<ReasoningSysCS>();

        for (int i = 0; i < tempSysCS._clueList.Length; i++)
        {
            tempSysCS._clueList[i]._check = false;
            tempSysCS._clueList[i]._cluePoint.transform.GetChild(0).gameObject.SetActive(false);
        }
        tempSysCS._reasoningMgrCS._currClueValue = 0;
        tempSysCS._reasoningMgrCS.setText();
    }
}
