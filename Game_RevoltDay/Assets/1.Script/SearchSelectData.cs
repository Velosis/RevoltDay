using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSelectData : MonoBehaviour {

    private UIMgr _uIMgrCS;
    public eTableType _currTableType = 0;
    public eSearchSelectType _currSelectType = 0;
    public string _sceneID = "";


    private void Awake()
    {
        _uIMgrCS = GameObject.Find("UIMgr").GetComponent<UIMgr>();
    }

    public void sceneChange()
    {
        _uIMgrCS._TalkMgr.SetActive(true);
        ReadCsvCS tempReadCsvCS = _uIMgrCS._TalkMgr.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<ReadCsvCS>();
        tempReadCsvCS._crimeSceneName = _sceneID;
        tempReadCsvCS._tableType = _currTableType;
        tempReadCsvCS._eSearchSelectType = _currSelectType;
        tempReadCsvCS.startDateLoad();
    }
}
