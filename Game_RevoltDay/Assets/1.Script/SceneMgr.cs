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

    public AudioClip _BgmSound;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("DonTileUI"))
        {
            Destroy(GameObject.Find("DonTileUI"));
        }

        GetComponent<AudioSource>().clip = _BgmSound;
        GetComponent<AudioSource>().Play();
    }

    private void Start()
    {
        if (GameObject.Find("GameOver") && GameObject.Find("GameOver").GetComponent<GameSysCS>()._SelectValue != -1)
        {
            SelectSave(GameObject.Find("GameOver").GetComponent<GameSysCS>()._SelectValue);
            Destroy(GameObject.Find("GameOver"));
            StartInGame();
        }else if (GameObject.Find("UIMgr") && GameObject.Find("UIMgr").GetComponent<UIMgr>()._SaveSelectValue != -1)
        {
            SelectSave(GameObject.Find("UIMgr").GetComponent<UIMgr>()._SaveSelectValue);
            Destroy(GameObject.Find("UIMgr"));
            StartInGame();
        }
    }

    public void SelectSave(int value)
    {
        _SaveNumber = value;
    }

    public void StartInGame()
    {
        _fristVideo = true;
        LodingMgrCS.LoadScene("1.MainGame");
        GetComponent<AudioSource>().Stop();
        gameObject.GetComponent<Canvas>().enabled = false;
        gameObject.name = "DonTileUI";
    }


    public void SaveData(SaveData _saveData, int value)
    {
        _currSaveDataList[value] = _saveData;

        JsonData infoJson = JsonMapper.ToJson(_currSaveDataList);

        File.WriteAllText(Application.dataPath + "/6.SaveData/SaveData.json", infoJson.ToString());
    }


}
