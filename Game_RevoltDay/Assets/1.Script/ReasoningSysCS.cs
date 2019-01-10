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
    public GameObject tempCheckGO;
    public clueData tempSelectGO;

    public void setText()
    {
        _reasoningMgrCS._missionPopupUI.transform.GetChild(1).GetComponent<Text>().text = _missionText;

    }


}
