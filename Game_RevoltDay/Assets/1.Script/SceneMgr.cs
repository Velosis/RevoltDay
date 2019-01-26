using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDateNamespace;

public class SceneMgr : MonoBehaviour {
    public SaveData[] _saveFiles;
    public SaveData _currFile;

    public bool _fristVideo = false;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectSave(int value)
    {
        _currFile = _saveFiles[value];
    }

    public void StartInGame()
    {
        _fristVideo = true;
        LodingMgrCS.LoadScene("1.MainGame");
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
