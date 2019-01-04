using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDateNamespace;

public class SaveSys : MonoBehaviour {
    public SaveData _saveFile;

    public bool _isDeleteSave;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_isDeleteSave)
        {
            _isDeleteSave = false;
        }
    }
}
