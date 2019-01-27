using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class PlayerInfo
{
    public bool Alive = false;
    public int ID = 0;
    public string Name = "";
    public double Gold = 0;

    //public PlayerInfo(int id, string name, double gold)
    //{
    //    ID = id;
    //    Name = name;
    //    Gold = gold;
    //}
}

public class tempSave
{
    public bool isSaveData = false;

    // 저장 시간
    public string SaveDay = "";

    // 아이템 저장
    public List<itemDataSave> _currItemDatasList = new List<itemDataSave>();
    public List<equipData> _currEquipDatasList = new List<equipData>();
    public List<aidData> _currAidDatasList = new List<aidData>();

    public List<itemDataSave> _useItemList = new List<itemDataSave>();

    // 맵 타일 저장
    public tileMapDate[] _tileMapList = new tileMapDate[14];

    // 스토리 진행 관련
    public int _currEventID = 0;

    // 현재 정보
    public int _clueTokenValue = 0;
    public bool _isAlive = false;
    public bool _isTurn = false;
    public int _daleyTurnCount = 0;
    public int _reasoningValue = 0;

    public int _currTile = 0;
    public int _tempCurrTile = 0;
    public int _currActPoint = 0;
    public int _currTrunPoint = 0;

    public int _currHP = 0;
    public int _MaxHP = 0;
    public int _atkPoint = 0;

    //public npcSaveData _WishIcon = null;

    //public npcSaveData _HamIcon = null;
    //public _HamMoveRoot _HamSaveData = null;

    //public npcSaveData _JeonIcon = null;
    //public _JeonMoveRoot _JeonSaveData = null;

    //public npcSaveData _YoungIcon = null;
    //public _YoungIconMoveRoot _YoungSaveData = null;
}


public class JsonText : MonoBehaviour {

    //public List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
    public List<tempSave> tempSaveList = new List<tempSave>();

    private void Start()
    {
        SavePlayerInfo();
        LoadPlayerInfo();
    }

    public void SavePlayerInfo()
    {
        Debug.Log("저장 실행");
        tempSave temp = new tempSave();
        tempSaveList.Add(temp);

        //playerInfoList.Add(new PlayerInfo(1, "이름1", 1001));
        //playerInfoList.Add(new PlayerInfo(1, "이름2", 1002));
        //playerInfoList.Add(new PlayerInfo(1, "이름3", 1003));
        //playerInfoList.Add(new PlayerInfo(1, "이름4", 1004));

        JsonData infoJson = JsonMapper.ToJson(tempSaveList);

        File.WriteAllText(Application.dataPath + "/6.SaveData/PlayerInfoData.json", infoJson.ToString());
    }


    public void LoadPlayerInfo()
    {
        Debug.Log("불러오기");

        if (File.Exists(Application.dataPath + "/6.SaveData/PlayerInfoData.json"))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + "/6.SaveData/PlayerInfoData.json");
            Debug.Log(jsonStr);
            JsonData playerData = JsonMapper.ToObject(jsonStr);

            for (int i = 0; i < playerData.Count; i++)
            {
                Debug.Log(playerData[i]["isSaveData"].ToString());
                Debug.Log(playerData[i]["SaveDay"].ToString());
                Debug.Log(playerData[i]["_currItemDatasList"].ToString());
                Debug.Log(playerData[i]["_currEquipDatasList"].ToString());
                Debug.Log(playerData[i]["_currAidDatasList"].ToString());
                Debug.Log(playerData[i]["_useItemList"].ToString());
                Debug.Log(playerData[i]["_tileMapList"].ToString());
                Debug.Log(playerData[i]["_currEventID"].ToString());
                Debug.Log(playerData[i]["_clueTokenValue"].ToString());
                Debug.Log(playerData[i]["_isAlive"].ToString());
                Debug.Log(playerData[i]["_isTurn"].ToString());
                Debug.Log(playerData[i]["_daleyTurnCount"].ToString());
                Debug.Log(playerData[i]["_reasoningValue"].ToString());
                Debug.Log(playerData[i]["_currTile"].ToString());
                Debug.Log(playerData[i]["_tempCurrTile"].ToString());
                Debug.Log(playerData[i]["_currActPoint"].ToString());
                Debug.Log(playerData[i]["_currTrunPoint"].ToString());
                Debug.Log(playerData[i]["_currHP"].ToString());
                Debug.Log(playerData[i]["_MaxHP"].ToString());
                Debug.Log(playerData[i]["_atkPoint"].ToString());
                //Debug.Log(playerData[i]["_WishIcon"].ToString());
                //Debug.Log(playerData[i]["_HamIcon"].ToString());
                //Debug.Log(playerData[i]["_HamSaveData"].ToString());
                //Debug.Log(playerData[i]["_JeonIcon"].ToString());
                //Debug.Log(playerData[i]["_JeonSaveData"].ToString());
                //Debug.Log(playerData[i]["_YoungIcon"].ToString());
                //Debug.Log(playerData[i]["_YoungSaveData"].ToString());
            }
        }
        else
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
    }
}
