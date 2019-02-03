using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[System.Serializable]
public class npcSaveData
{
    public int _currTile = -1;
    public bool _isAlive = false;
    public bool _isTurn = false;
}

[System.Serializable]
public class tileMapDate
{
    public int _tileIndex = 0;
    public double _SafetyValue = 0.0f;

    public bool _isSafetyEff = false;
    public bool _isBlockade = false;
    public bool _isIssueIcon = false;

    public bool _isShop = false;
    public bool _isSpShop = false;
    public bool _isIssue = false;
    public bool _isCrime = false;
}

[System.Serializable]
public class itemDataSave
{
    public int _Index = 0;
    public int _currTurn = 0;
}

[System.Serializable]
public class aidData
{
    public int _Index = 0;
    public int _currTurn = 0;
    public bool _isGet = false;
    public bool _setUse = false;
}

[System.Serializable]
public class equipData
{
    public int _Index = 0;
    public bool _setUse = false;
}

[System.Serializable]
public class SaveData
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

    public bool _isPark = false;
    public string _currImg = ""; 
    public int _currTile = 0;
    public int _tempCurrTile = 0;
    public int _currActPoint = 0;
    public int _currTrunPoint = 0;

    public int _currHP = 0;
    public int _MaxHP = 0;
    public int _atkPoint = 0;

    public npcSaveData _WishIcon = new npcSaveData();

    public npcSaveData _HamIcon = new npcSaveData();
    public _HamMoveRoot _HamSaveData = new _HamMoveRoot();

    public npcSaveData _JeonIcon = new npcSaveData();
    public _JeonMoveRoot _JeonSaveData = new _JeonMoveRoot();

    public npcSaveData _YoungIcon = new npcSaveData();
    public _YoungIconMoveRoot _YoungSaveData = new _YoungIconMoveRoot();
}

public class SaveDataCS : MonoBehaviour
{
    public List<SaveData> SaveList = new List<SaveData>();

    private void Start()
    {
        LoadSaveInfo();
    }

    public void FirstSaveFile()
    {
        // 최초 세이브 데이터 생성
        for (int i = 0; i < 4; i++)
        {
            SaveData temp = new SaveData();
            for (int j = 0; j < temp._tileMapList.Length; j++)
            {
                temp._tileMapList[j] = new tileMapDate();
            }

            SaveList.Add(temp);
        }

        JsonData infoJson = JsonMapper.ToJson(SaveList);

        File.WriteAllText(Application.persistentDataPath + "/" + "SaveData.json", infoJson.ToString());
        Debug.Log("최초 세이브 데이터 생성");

    }


    public void LoadSaveInfo()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "SaveData.json"))
        {
            string jsonStr = File.ReadAllText(Application.persistentDataPath + "/" + "SaveData.json");
            JsonData JsonSaveData = JsonMapper.ToObject(jsonStr);


            for (int i = 0; i < JsonSaveData.Count; i++)
            {
                SaveData TempSaveData = new SaveData();


                TempSaveData.isSaveData = (bool)JsonSaveData[i]["isSaveData"];
                if (!TempSaveData.isSaveData) continue;


                TempSaveData.SaveDay = (string)JsonSaveData[i]["SaveDay"];

                // 아이템 불러오기
                for (int q = 0; q < JsonSaveData[i]["_currItemDatasList"].Count; q++)
                {
                    itemDataSave _itemDataSave = new itemDataSave();
                    _itemDataSave._Index = (int)JsonSaveData[i]["_currItemDatasList"][q]["_Index"];
                    _itemDataSave._currTurn = (int)JsonSaveData[i]["_currItemDatasList"][q]["_currTurn"];

                    TempSaveData._currItemDatasList.Add(_itemDataSave);
                }
                for (int q = 0; q < JsonSaveData[i]["_currEquipDatasList"].Count; q++)
                {
                    equipData _equipData = new equipData();
                    _equipData._Index = (int)JsonSaveData[i]["_currEquipDatasList"][q]["_Index"];
                    _equipData._setUse = (bool)JsonSaveData[i]["_currEquipDatasList"][q]["_setUse"];

                    TempSaveData._currEquipDatasList.Add(_equipData);
                }
                for (int q = 0; q < JsonSaveData[i]["_currAidDatasList"].Count; q++)
                {
                    aidData _aidData = new aidData();
                    _aidData._Index = (int)JsonSaveData[i]["_currAidDatasList"][q]["_Index"];
                    _aidData._currTurn = (int)JsonSaveData[i]["_currAidDatasList"][q]["_currTurn"];
                    _aidData._isGet = (bool)JsonSaveData[i]["_currAidDatasList"][q]["_isGet"];
                    _aidData._setUse = (bool)JsonSaveData[i]["_currAidDatasList"][q]["_setUse"];

                    TempSaveData._currAidDatasList.Add(_aidData);
                }
                for (int q = 0; q < JsonSaveData[i]["_useItemList"].Count; q++)
                {
                    itemDataSave _itemDataSave = new itemDataSave();
                    _itemDataSave._Index = (int)JsonSaveData[i]["_useItemList"][q]["_Index"];
                    _itemDataSave._currTurn = (int)JsonSaveData[i]["_useItemList"][q]["_currTurn"];

                    TempSaveData._useItemList.Add(_itemDataSave);
                }

                for (int q = 0; q < JsonSaveData[i]["_tileMapList"].Count; q++)
                {
                    tileMapDate _tileMapDate = new tileMapDate();
                    _tileMapDate._tileIndex = (int)JsonSaveData[i]["_tileMapList"][q]["_tileIndex"];
                    _tileMapDate._SafetyValue = (double)JsonSaveData[i]["_tileMapList"][q]["_SafetyValue"];

                    _tileMapDate._isSafetyEff = (bool)JsonSaveData[i]["_tileMapList"][q]["_isSafetyEff"];
                    _tileMapDate._isBlockade = (bool)JsonSaveData[i]["_tileMapList"][q]["_isBlockade"];
                    _tileMapDate._isIssueIcon = (bool)JsonSaveData[i]["_tileMapList"][q]["_isIssueIcon"];

                    _tileMapDate._isShop = (bool)JsonSaveData[i]["_tileMapList"][q]["_isShop"];
                    _tileMapDate._isSpShop = (bool)JsonSaveData[i]["_tileMapList"][q]["_isSpShop"];
                    _tileMapDate._isIssue = (bool)JsonSaveData[i]["_tileMapList"][q]["_isIssue"];
                    _tileMapDate._isCrime = (bool)JsonSaveData[i]["_tileMapList"][q]["_isCrime"];

                    TempSaveData._tileMapList[q] = _tileMapDate;
                }

                TempSaveData._currEventID = (int)JsonSaveData[i]["_currEventID"];

                TempSaveData._clueTokenValue = (int)JsonSaveData[i]["_clueTokenValue"];
                TempSaveData._isAlive = (bool)JsonSaveData[i]["_isAlive"];
                TempSaveData._isTurn = (bool)JsonSaveData[i]["_isTurn"];
                TempSaveData._daleyTurnCount = (int)JsonSaveData[i]["_daleyTurnCount"];
                TempSaveData._reasoningValue = (int)JsonSaveData[i]["_reasoningValue"];

                TempSaveData._currTile = (int)JsonSaveData[i]["_currTile"];
                TempSaveData._tempCurrTile = (int)JsonSaveData[i]["_tempCurrTile"];
                TempSaveData._currActPoint = (int)JsonSaveData[i]["_currActPoint"]; ;
                TempSaveData._currTrunPoint = (int)JsonSaveData[i]["_currTrunPoint"];

                TempSaveData._currHP = (int)JsonSaveData[i]["_currHP"];
                TempSaveData._MaxHP = (int)JsonSaveData[i]["_MaxHP"];
                TempSaveData._atkPoint = (int)JsonSaveData[i]["_atkPoint"];

                TempSaveData._WishIcon._currTile = (int)JsonSaveData[i]["_WishIcon"]["_currTile"];
                TempSaveData._WishIcon._isAlive = (bool)JsonSaveData[i]["_WishIcon"]["_isAlive"];
                TempSaveData._WishIcon._isTurn = (bool)JsonSaveData[i]["_WishIcon"]["_isTurn"];

                TempSaveData._HamIcon._currTile = (int)JsonSaveData[i]["_HamIcon"]["_currTile"];
                TempSaveData._HamIcon._isAlive = (bool)JsonSaveData[i]["_HamIcon"]["_isAlive"];
                TempSaveData._HamIcon._isTurn = (bool)JsonSaveData[i]["_HamIcon"]["_isTurn"];

                for (int q = 0; q < JsonSaveData[i]["_HamSaveData"].Count; q++)
                {
                    switch ((int)JsonSaveData[i]["_HamSaveData"]["_currRoot"])
                    {
                        case 0:
                            TempSaveData._HamSaveData._currRoot = _HamMoveRoot.eRootType.Non;
                            break;
                        case 1:
                            TempSaveData._HamSaveData._currRoot = _HamMoveRoot.eRootType.A;
                            break;
                        case 2:
                            TempSaveData._HamSaveData._currRoot = _HamMoveRoot.eRootType.B;
                            break;
                        case 3:
                            TempSaveData._HamSaveData._currRoot = _HamMoveRoot.eRootType.C;
                            break;
                        default:
                            break;
                    }

                    TempSaveData._HamSaveData._rootChangeValue = (double)JsonSaveData[i]["_HamSaveData"]["_rootChangeValue"];
                    TempSaveData._HamSaveData._nextRootTile = (int)JsonSaveData[i]["_HamSaveData"]["_nextRootTile"];
                    TempSaveData._HamSaveData._currRootValue = (int)JsonSaveData[i]["_HamSaveData"]["_currRootValue"];

                    TempSaveData._HamSaveData._currList = new int[JsonSaveData[i]["_HamSaveData"]["_currList"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_HamSaveData"]["_currList"].Count; r++)
                    {
                        TempSaveData._HamSaveData._currList[r] = (int)JsonSaveData[i]["_HamSaveData"]["_currList"][r];
                    }

                    TempSaveData._HamSaveData._jeonMoveTileListA = new int[JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListA"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListA"].Count; r++)
                    {
                        TempSaveData._HamSaveData._jeonMoveTileListA[r] = (int)JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListA"][r];
                    }

                    TempSaveData._HamSaveData._jeonMoveTileListB = new int[JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListB"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListB"].Count; r++)
                    {
                        TempSaveData._HamSaveData._jeonMoveTileListB[r] = (int)JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListB"][r];
                    }

                    TempSaveData._HamSaveData._jeonMoveTileListC = new int[JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListC"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListC"].Count; r++)
                    {
                        TempSaveData._HamSaveData._jeonMoveTileListC[r] = (int)JsonSaveData[i]["_HamSaveData"]["_jeonMoveTileListC"][r];
                    }
                }

                TempSaveData._JeonIcon._currTile = (int)JsonSaveData[i]["_JeonIcon"]["_currTile"];
                TempSaveData._JeonIcon._isAlive = (bool)JsonSaveData[i]["_JeonIcon"]["_isAlive"];
                TempSaveData._JeonIcon._isTurn = (bool)JsonSaveData[i]["_JeonIcon"]["_isTurn"];
                for (int q = 0; q < JsonSaveData[i]["_JeonSaveData"].Count; q++)
                {
                    switch ((int)JsonSaveData[i]["_JeonSaveData"]["_currRoot"])
                    {
                        case 0:
                            TempSaveData._JeonSaveData._currRoot = _JeonMoveRoot.eRootType.Non;
                            break;
                        case 1:
                            TempSaveData._JeonSaveData._currRoot = _JeonMoveRoot.eRootType.A;
                            break;
                        case 2:
                            TempSaveData._JeonSaveData._currRoot = _JeonMoveRoot.eRootType.B;
                            break;
                        case 3:
                            TempSaveData._JeonSaveData._currRoot = _JeonMoveRoot.eRootType.C;
                            break;
                        case 4:
                            TempSaveData._JeonSaveData._currRoot = _JeonMoveRoot.eRootType.D;
                            break;
                        default:
                            break;
                    }

                    TempSaveData._JeonSaveData._rootChangeValue = (double)JsonSaveData[i]["_JeonSaveData"]["_rootChangeValue"];
                    TempSaveData._JeonSaveData._nextRootTile = (int)JsonSaveData[i]["_JeonSaveData"]["_nextRootTile"];
                    TempSaveData._JeonSaveData._currRootValue = (int)JsonSaveData[i]["_JeonSaveData"]["_currRootValue"];

                    TempSaveData._JeonSaveData._currList = new int[JsonSaveData[i]["_JeonSaveData"]["_currList"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_JeonSaveData"]["_currList"].Count; r++)
                    {
                        TempSaveData._JeonSaveData._currList[r] = (int)JsonSaveData[i]["_JeonSaveData"]["_currList"][r];
                    }
                    TempSaveData._JeonSaveData._jeonMoveTileListA = new int[JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListA"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListA"].Count; r++)
                    {
                        TempSaveData._JeonSaveData._jeonMoveTileListA[r] = (int)JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListA"][r];
                    }
                    TempSaveData._JeonSaveData._jeonMoveTileListB = new int[JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListB"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListB"].Count; r++)
                    {
                        TempSaveData._JeonSaveData._jeonMoveTileListB[r] = (int)JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListB"][r];
                    }
                    TempSaveData._JeonSaveData._jeonMoveTileListC = new int[JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListC"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListC"].Count; r++)
                    {
                        TempSaveData._JeonSaveData._jeonMoveTileListC[r] = (int)JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListC"][r];
                    }
                    TempSaveData._JeonSaveData._jeonMoveTileListD = new int[JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListD"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListD"].Count; r++)
                    {
                        TempSaveData._JeonSaveData._jeonMoveTileListD[r] = (int)JsonSaveData[i]["_JeonSaveData"]["_jeonMoveTileListD"][r];
                    }
                }

                TempSaveData._YoungIcon._currTile = (int)JsonSaveData[i]["_YoungIcon"]["_currTile"];
                TempSaveData._YoungIcon._isAlive = (bool)JsonSaveData[i]["_YoungIcon"]["_isAlive"];
                TempSaveData._YoungIcon._isTurn = (bool)JsonSaveData[i]["_YoungIcon"]["_isTurn"];

                for (int q = 0; q < JsonSaveData[i]["_YoungSaveData"].Count; q++)
                {
                    TempSaveData._YoungSaveData._nextRootTile = (int)JsonSaveData[i]["_YoungSaveData"]["_nextRootTile"];
                    TempSaveData._YoungSaveData._currRootValue = (int)JsonSaveData[i]["_YoungSaveData"]["_currRootValue"];

                    TempSaveData._YoungSaveData._YoungMoveTileList = new int[JsonSaveData[i]["_YoungSaveData"]["_YoungMoveTileList"].Count];
                    for (int r = 0; r < JsonSaveData[i]["_YoungSaveData"]["_YoungMoveTileList"].Count; r++)
                    {
                        TempSaveData._YoungSaveData._YoungMoveTileList[r] = (int)JsonSaveData[i]["_YoungSaveData"]["_YoungMoveTileList"][r];
                    }
                }

                GetComponent<SceneMgr>()._currSaveDataList[i] = TempSaveData;
            }
        }
        else
        {
            // 최초 세이브 데이터 생성
            FirstSaveFile();
        }
    }
}


