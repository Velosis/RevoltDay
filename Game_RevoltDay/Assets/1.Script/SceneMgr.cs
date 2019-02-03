using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.Video;

public enum eSaveType
{
    First,
    Load
}


public class SceneMgr : MonoBehaviour {
    public SaveData[] _currSaveDataList = new SaveData[4];
    public SaveData _NewStart = null;
    public bool _IsNewGame = false;


    public int _SaveNumber = 0;
    public bool _fristVideo = false;

    public eSaveType _eSaveType;

    public AudioClip _BgmSound;

    public VideoClip _VideoUIClip;
    public GameObject _SaveUiGO;
    public GameObject _RootCreatorVideo;
    public GameObject _CreatorVideo;
    public GameObject _OptionUI;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        _CreatorVideo.GetComponent<VideoPlayer>().clip = _VideoUIClip;

        if (GameObject.Find("DonTileUI"))
        {
            Destroy(GameObject.Find("DonTileUI"));
        }


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
            return;
        }

        GetComponent<AudioSource>().clip = _BgmSound;
        GetComponent<AudioSource>().volume = (float)OptionMgrCS._OptionInfo._BgmValue;
        GetComponent<AudioSource>().Play();
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
        _RootCreatorVideo = null;
        _CreatorVideo = null;
        _OptionUI = null;
    }

    public void IsUiOnOff(GameObject _gameObject)
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    public void SelectIsStart(string _str)
    {
        if (_str == "불러오기")
        {
            _eSaveType = eSaveType.Load;
            settingSaveDataUI();
        }
        else if (_str == "처음부터")
        {
            _eSaveType = eSaveType.First;
            _NewStart = new SaveData();
            _IsNewGame = true;
            StartInGame();
        }else if (_str == "만든이")
        {
            GetComponent<AudioSource>().Stop();
            _RootCreatorVideo.SetActive(true);
            _CreatorVideo.GetComponent<VideoPlayer>().Play();
            GetComponent<Canvas>().enabled = false;

        }
        else if (_str == "설정")
        {
            _OptionUI.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)OptionMgrCS.getOptionInfo()._SeValue;
            _OptionUI.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)OptionMgrCS.getOptionInfo()._BgmValue;
        }
        else if (_str == "종료")
        {
            Application.Quit();
        }
    }

    private void LateUpdate()
    {
        if (_OptionUI && _OptionUI.activeSelf && _OptionUI.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value != (float)OptionMgrCS.getOptionInfo()._BgmValue)
            GetComponent<AudioSource>().volume = _OptionUI.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value;
    }

    public void StopVideo()
    {
        _CreatorVideo.GetComponent<VideoPlayer>().Stop();
        GetComponent<Canvas>().enabled = true;
        _RootCreatorVideo.SetActive(false);
        GetComponent<AudioSource>().Play();
    }

    public void settingSaveDataUI()
    {
        for (int i = 0; i < _currSaveDataList.Length; i++)
        {
            _SaveUiGO.transform.GetChild(3 + i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = _currSaveDataList[i].isSaveData ? _currSaveDataList[i].SaveDay : "저장 정보 없음";
        }
    }

    public void StartLoadData(int _value)
    {
        if (!_currSaveDataList[_value].isSaveData) return;

        _SaveNumber = _value;
        StartInGame();
    }

    public void OptionSave()
    {
        OptionInfo optionInfo = new OptionInfo();
        optionInfo._SeValue = _OptionUI.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value;
        optionInfo._BgmValue = _OptionUI.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value;
        OptionMgrCS.OptionSave(optionInfo);

    }

    public void DeleteSaveData(int _value)
    {
        _currSaveDataList[_value] = new SaveData();
        settingSaveDataUI();
        for (int i = 0; i < _currSaveDataList.Length; i++)
        {
            SaveData(_currSaveDataList[i], i);
        }
    }

    public void SaveData(SaveData _saveData, int value)
    {
        _currSaveDataList[value] = _saveData;

        JsonData infoJson = JsonMapper.ToJson(_currSaveDataList);

        File.WriteAllText(Application.persistentDataPath + "/" + "SaveData.json", infoJson.ToString());
    }


}
