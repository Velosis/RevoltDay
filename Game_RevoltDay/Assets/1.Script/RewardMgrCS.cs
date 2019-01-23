using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct RewardData
{
    public int _Index;
    public string _Text;
    public eTableType _TableType;
    public int _RewardMoneyMin;
    public int _RewardMoneyMax;
    public int _RewardSafety;
    public int _RewardClueRandom;
    public int _RewardClueMin;
    public int _RewardClueMax;
    public int _RewardItemRandom;
    public int _RewardAidRandom;
}

public class RewardMgrCS : MonoBehaviour {
    public PlayerInfoCS _playerInfoCS;
    public TileMakerCS _tileMakerCS;
    public ShopMgr _shopMgrCS;
    public GameObject _ShopMgr;

    private List<GameObject> _tileMapGoList;
    private List<ItemData> _ItemDatasList;
    private List<AidData> _AidDatasList;

    public GameObject _RewardBoxGO;
    public GameObject[] _RewardBoxGOArr = new GameObject[4];
    private Vector3 _BoxOrginVec3;

    public List<RewardData> _normalRewardList = new List<RewardData>();
    public List<RewardData> _IssueRewardList = new List<RewardData>();
    public List<RewardData> _BlockadeRewardList = new List<RewardData>();

    public Sprite _MoneyImg;
    public Sprite _SafetyImg;
    public Sprite _ClueImg;

    private int _RewardValue = 0;

    public void ReadCSV()
    {
        List<Dictionary<string, object>> date;

        // 보상 테이블 로드
        #region
        date = CSVReader.Read("2.SceneTable/RewardTable");

        for (int i = 0; i < date.Count; i++)
        {
            RewardData TempRewardData = new RewardData();
            TempRewardData._Index = (int)date[i]["Index"];
            TempRewardData._Text = (string)date[i]["Text"];
            switch ((string)date[i]["type"])
            {
                case "일반":
                    TempRewardData._TableType = eTableType.normal;
                    break;
                case "긴급":
                    TempRewardData._TableType = eTableType.Issue;
                    break;
                case "봉쇄":
                    TempRewardData._TableType = eTableType.Blockade;
                    break;
                default:
                    break;
            }
            TempRewardData._RewardMoneyMin = (int)date[i]["RewardMoneyMin"];
            TempRewardData._RewardMoneyMax = (int)date[i]["RewardMoneyMax"];
            TempRewardData._RewardSafety = (int)date[i]["RewardSafety"];
            TempRewardData._RewardClueRandom = (int)date[i]["RewardClueRandom"];
            TempRewardData._RewardClueMin = (int)date[i]["RewardClueMin"];
            TempRewardData._RewardClueMax = (int)date[i]["RewardClueMax"];
            TempRewardData._RewardItemRandom = (int)date[i]["RewardItemRandom"];
            TempRewardData._RewardAidRandom = (int)date[i]["RewardAidRandom"];

            switch (TempRewardData._TableType)
            {
                case eTableType.normal:
                    _normalRewardList.Add(TempRewardData);
                    break;
                case eTableType.Blockade:
                    _BlockadeRewardList.Add(TempRewardData);
                    break;
                case eTableType.Issue:
                    _IssueRewardList.Add(TempRewardData);
                    break;
            }
        }
        #endregion
    }

    private void Awake()
    {
        ReadCSV();
        _tileMapGoList = _tileMakerCS.TileMapList;



        _RewardBoxGO.SetActive(false);

        for (int i = 0; i < _RewardBoxGOArr.Length; i++)
        {
            _RewardBoxGOArr[i] = Instantiate(_RewardBoxGO, transform.GetChild(1).gameObject.transform);
            _BoxOrginVec3 = _RewardBoxGOArr[i].transform.localPosition;

        }
    }

    public void isUiOnoff(bool _is)
    {
        gameObject.SetActive(_is);
    }

    public void StartRewardSys(int _tileArr, eTableType _eTableType)
    {
        gameObject.SetActive(true);
        _RewardValue = 0;

        if (SettingRewardArr(_RewardValue, 0, _tileArr, _eTableType)) _RewardValue++;
        if (SettingRewardArr(_RewardValue, 1, _tileArr, _eTableType)) _RewardValue++;
        if (SettingRewardArr(_RewardValue, 2, _tileArr, _eTableType)) _RewardValue++;
        if (SettingRewardArr(_RewardValue, 3, _tileArr, _eTableType)) _RewardValue++;

        SettingRewardBox(_RewardValue);
    }

    public bool SettingRewardArr(int arrIndex ,int value, int _tileInfo, eTableType _eTableType)
    {
        _ItemDatasList = _ShopMgr.GetComponent<ShopMgr>()._itemDatas;
        _AidDatasList = _ShopMgr.GetComponent<ShopMgr>()._AidDatas;

        Image tempImgIcon = _RewardBoxGOArr[arrIndex].transform.GetChild(1).gameObject.GetComponent<Image>();
        Text TempText = _RewardBoxGOArr[arrIndex].transform.GetChild(3).gameObject.GetComponent<Text>();
        Text TempType = _RewardBoxGOArr[arrIndex].transform.GetChild(4).gameObject.GetComponent<Text>();
        tempImgIcon.color = Color.white;

        RewardData TempRewardData = new RewardData();
        switch (_eTableType)
        {
            case eTableType.normal:
                TempRewardData = _normalRewardList[Random.Range(0, _normalRewardList.Count)];
                break;
            case eTableType.Blockade:
                TempRewardData = _BlockadeRewardList[Random.Range(0, _BlockadeRewardList.Count)];
                break;
            case eTableType.Issue:
                TempRewardData = _IssueRewardList[Random.Range(0, _IssueRewardList.Count)];
                break;
            default:
                break;
        }
        int tempValue = 0;

        ItemData TempItemData = new ItemData();
        AidData TempAidData = new AidData();

        switch (value)
        {
            case 0: // 돈 획득
                tempImgIcon.sprite = _MoneyImg;
                tempValue = Random.Range(TempRewardData._RewardMoneyMin, TempRewardData._RewardMoneyMax + 1);
                TempText.text = tempValue + "원";
                TempType.text = "보수 획득";
                _playerInfoCS.setMoney(tempValue);
                return true;
            case 1: // 아이템 획득
                if (TempRewardData._RewardItemRandom >= Random.Range(0, 100)) 
                {
                    TempItemData = ResourceMgrCS.SettingItemData(_ItemDatasList[Random.Range(0, _ItemDatasList.Count)]);
                    tempImgIcon.sprite = TempItemData._sprite;
                    TempText.text = TempItemData._NameKR;
                    TempType.text = "아이템 획득";
                    _playerInfoCS._BoxItemList.Add(TempItemData);
                    return true;
                }
                else if (TempRewardData._RewardAidRandom >= Random.Range(0, 100)) 
                {
                    TempAidData = ResourceMgrCS.SettingAidData(_AidDatasList[Random.Range(0, _AidDatasList.Count)]);
                    if (TempAidData._isGet) return false;
                    tempImgIcon.sprite = TempAidData._spriteTile;
                    tempImgIcon.color = Color.black;
                    TempText.text = TempAidData._NameKR;
                    TempType.text = "명함 획득";
                    _playerInfoCS._BoxAidList.Add(TempAidData);
                    return true;
                }
                else return false;
            case 2:
                if (TempRewardData._RewardClueRandom >= Random.Range(0, 100))
                {
                    tempImgIcon.sprite = _ClueImg;
                    tempValue = Random.Range(TempRewardData._RewardClueMin, TempRewardData._RewardClueMin + 1);
                    TempText.text = tempValue + "개";
                    TempType.text = "단서 획득";
                    _playerInfoCS.setClue(tempValue);
                    return true;
                }
                else return false;

            case 3:
                if (_tileInfo == -1) return false;
                
                tempImgIcon.sprite = _SafetyImg;
                tempImgIcon.color = Color.green;
                if (_tileMapGoList[_tileInfo].GetComponent<TileMapDataCS>()._isBlockade)
                {
                    TempText.text = "치안 완화";
                    TempType.text = "봉쇄 해체";
                    _tileMapGoList[_tileInfo].GetComponent<TileMapDataCS>().setBlockade(false);
                    return true;
                }
                else if (!_tileMapGoList[_tileInfo].GetComponent<TileMapDataCS>()._isBlockade &&
                    _tileMapGoList[_tileInfo].GetComponent<TileMapDataCS>()._SafetyValue >= 0.0f)
                {
                    TempText.text = "치안 완화";
                    TempType.text = "치안 안정";
                    _tileMapGoList[_tileInfo].GetComponent<TileMapDataCS>().setSafetyBer(0.0f);
                    return true;
                }
                return false;
            default:
                break;
        }
        return false;
    }

    public void SettingRewardBox(int value)
    {
        for (int i = 0; i < 4; i++)
        {
            _RewardBoxGOArr[i].SetActive(false);
            _RewardBoxGOArr[i].transform.localPosition = _BoxOrginVec3;
        }

        float tempSizeW = (_RewardBoxGOArr[0].GetComponent<RectTransform>().rect.width / 2.0f) + 50.0f;
        int tempValue = value - 1;
        for (int i = 0; i < value; i++)
        {
            _RewardBoxGOArr[i].SetActive(true);
            _RewardBoxGOArr[i].transform.Translate((tempSizeW * i),0,0);
            _RewardBoxGOArr[i].transform.Translate(-(tempSizeW * tempValue), 0, 0);
            tempValue--;
        }
    }
}
