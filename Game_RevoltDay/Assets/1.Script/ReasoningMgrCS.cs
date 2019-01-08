using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasoningMgrCS : MonoBehaviour {

    public GameObject _reasoningUIGO;
    public GameObject _checkPopupUI;
    public GameObject _missionPopupUI;
    public GameObject _trueCheckPopUI;
    private Text _stateValueText;

    public GameObject _currScene;

    public GameObject[] _sceneList;
    public int _currClueValue = 0;

    private void Awake()
    {
        _stateValueText = _reasoningUIGO.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        settingScene("Scene_0");
    }

    public void settingScene(string sceneName)
    {
        for (int i = 0; i < _sceneList.Length; i++)
        {
            if (_sceneList[i].name == sceneName)
            {
                _sceneList[i].SetActive(true);
                _currScene = _sceneList[i];
                return;
            }
        }
    }

    public void setText()
    {
        _stateValueText.text = _currClueValue.ToString() + " / " + 2;
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
            Debug.Log("사건 해결");
        }
        else
        {
            Debug.Log("해결 실패!");
        }
    }
}
