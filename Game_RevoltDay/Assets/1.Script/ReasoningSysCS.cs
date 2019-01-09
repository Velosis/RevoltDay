using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class clueData
{
    public GameObject _cluePoint;
    public bool _check = false;
    public bool _isTrue = false;
    public string _popupText;
}


public class ReasoningSysCS : MonoBehaviour {
    public string _missionText;
    public int _trueValue;
    public clueData[] _clueList;

    public ReasoningMgrCS _reasoningMgrCS;
    private GameObject tempCheckGO;
    private clueData tempSelectGO;

    private void OnEnable()
    {
        _reasoningMgrCS._missionPopupUI.transform.GetChild(1).GetComponent<Text>().text = _missionText;
    }

    public void resetSys()
    {
        for (int i = 0; i < _clueList.Length; i++)
        {
            _clueList[i]._check = false;
            _clueList[i]._cluePoint.transform.GetChild(0).gameObject.SetActive(false);
        }
        _reasoningMgrCS._currClueValue = 0;
        _reasoningMgrCS.setText();
    }

    public void selectPoint (GameObject whoPoint)
    {
        for (int i = 0; i < _clueList.Length; i++)
        {
            if (!_clueList[i]._check && _clueList[i]._cluePoint.name == whoPoint.name)
            {
                tempCheckGO = whoPoint.transform.GetChild(0).gameObject;
                tempSelectGO = _clueList[i];
                _reasoningMgrCS._checkPopupUI.transform.GetChild(1).GetComponent<Text>().text = _clueList[i]._popupText;
                _reasoningMgrCS._checkPopupUI.SetActive(true);
                return;
            }else if (_clueList[i]._check && _clueList[i]._cluePoint.name == whoPoint.name)
            {
                whoPoint.transform.GetChild(0).gameObject.SetActive(false);
                _clueList[i]._check = false;
                _reasoningMgrCS._currClueValue--;
                _reasoningMgrCS.setText();
                return;
            }
        }
    }

    public void CheckYesNo (bool YesNo)
    {
        if (YesNo)
        {
            tempCheckGO.SetActive(true);
            _reasoningMgrCS._currClueValue++;
            _reasoningMgrCS.setText();
            _reasoningMgrCS._checkPopupUI.SetActive(false);
            tempSelectGO._check = true;
        }
        else
        {
            _reasoningMgrCS._checkPopupUI.SetActive(false);
        }
    }


}
