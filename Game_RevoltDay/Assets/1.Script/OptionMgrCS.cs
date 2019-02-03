using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public class OptionInfo
{
    public double _BgmValue = 1.0;
    public double _SeValue = 1.0;
}

public class OptionMgrCS : MonoBehaviour {
    static public OptionInfo _OptionInfo = new OptionInfo();

    static public OptionInfo getOptionInfo()
    {
        return _OptionInfo;
    }

    private void Awake()
    {
        if (GameObject.Find(gameObject.name)) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (File.Exists(Application.persistentDataPath + "/" + "OptionInfo.json"))
        {
            string jsonStr = File.ReadAllText(Application.persistentDataPath + "/" + "OptionInfo.json");
            JsonData JsonSaveData = JsonMapper.ToObject(jsonStr);

            _OptionInfo._BgmValue = (double)JsonSaveData["_BgmValue"];
            _OptionInfo._SeValue = (double)JsonSaveData["_SeValue"];
        }
        else
        {
            OptionSetting();
        }
    }

    static public void OptionSave(OptionInfo _optionInfo)
    {
        _OptionInfo = _optionInfo;
        OptionSetting();
    }

    static public void OptionSetting()
    {
        JsonData infoJson = JsonMapper.ToJson(_OptionInfo);

        File.WriteAllText(Application.persistentDataPath + "/" + "OptionInfo.json", infoJson.ToString());
    }
}
