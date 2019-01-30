using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSysCS : MonoBehaviour {
    public SaveData[] _currSaveDataList = new SaveData[4];
    public int _SelectValue = -1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _SelectValue = -1;
        _currSaveDataList = GameObject.Find("DonTileUI").GetComponent<SceneMgr>()._currSaveDataList;
        Destroy(GameObject.Find("DonTileUI"));
    }

    public void sceneChange(string _str)
    {
        SceneManager.LoadScene(_str);
        GetComponent<Canvas>().enabled = false;
    }

    public void isOnOffUI(GameObject _gameObject)
    {
        SaveUiSetting();
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    public void SelectSaveValye (int value)
    {
        if (!_currSaveDataList[value].isSaveData)
        {
            return;
        }

        _SelectValue = value;
        sceneChange("0.Tilte");
    }

    public void SaveUiSetting()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_currSaveDataList[i].isSaveData) transform.GetChild(4).gameObject.transform.GetChild(3 + i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = _currSaveDataList[i].SaveDay;
            else transform.GetChild(4).gameObject.transform.GetChild(3 + i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "저장 정보 없음";
        }
    }

}
