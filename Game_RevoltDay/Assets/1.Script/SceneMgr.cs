using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;

public class SceneMgr : MonoBehaviour {
    public SaveData[] _currSaveDataList = new SaveData[4];
    public int _SaveNumber = 0;

    public bool _fristVideo = false;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectSave(int value)
    {
        _SaveNumber = value;
    }

    public void StartInGame()
    {
        _fristVideo = true;
        LodingMgrCS.LoadScene("1.MainGame");
        gameObject.GetComponent<Canvas>().enabled = false;
    }


    public void SaveData(SaveData _saveData, int value)
    {
        _currSaveDataList[value] = _saveData;

        JsonData infoJson = JsonMapper.ToJson(_currSaveDataList);

        File.WriteAllText(Application.dataPath + "/6.SaveData/SaveData.json", infoJson.ToString());
    }


}
