﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDateNamespace;

public class SceneMgr : MonoBehaviour {
    public SaveData[] _saveFiles;
    public SaveData _currFile;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public void SelectSave(int value)
    {
        _currFile = _saveFiles[value];
    }

    public void StartInGame()
    {
        LodingMgrCS.LoadScene("1.MainGame");

        //SceneManager.LoadScene("1.MainGame");
    }
}
