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

    private void Start()
    {

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


    // 아이템 생성
    public ItemData SettingItemData(ItemData _itemData)
    {
        Debug.Log("아이템 생성 시도");
        ItemData TempItemData = new ItemData();
        TempItemData._Bundle = _itemData._Bundle;
        TempItemData._Chance = _itemData._Chance;
        TempItemData._Codex = _itemData._Codex;
        TempItemData._Dectective = _itemData._Dectective;
        TempItemData._Endure = _itemData._Endure;
        TempItemData._Fight = _itemData._Fight;
        TempItemData._image = _itemData._image;
        TempItemData._Move = _itemData._Move;
        TempItemData._NameEN = _itemData._NameEN;
        TempItemData._NameKR = _itemData._NameKR;
        TempItemData._Nomalprice = _itemData._Nomalprice;
        TempItemData._Nomalstore = _itemData._Nomalstore;
        TempItemData._Restore = _itemData._Restore;
        TempItemData._Sell = _itemData._Sell;
        TempItemData._Sellprice = _itemData._Sellprice;
        TempItemData._Specprice = _itemData._Specprice;
        TempItemData._Specstore = _itemData._Specstore;
        TempItemData._sprite = _itemData._sprite;
        TempItemData._Text = _itemData._Text;
        TempItemData._Type = _itemData._Type;

        return TempItemData;
    }
    public EquipData SettingEquipData(EquipData _equipData)
    {
        EquipData TempEquipData = new EquipData();
        TempEquipData._Bundle = _equipData._Bundle;
        TempEquipData._Codex = _equipData._Codex;
        TempEquipData._Dectective = _equipData._Dectective;
        TempEquipData._DuelType = _equipData._DuelType;
        TempEquipData._Fight = _equipData._Fight;
        TempEquipData._image = _equipData._image;
        TempEquipData._isSet = _equipData._isSet;
        TempEquipData._Move = _equipData._Move;
        TempEquipData._NameEN = _equipData._NameEN;
        TempEquipData._NameKR = _equipData._NameKR;
        TempEquipData._Nomalprice = _equipData._Nomalprice;
        TempEquipData._Nomalstore = _equipData._Nomalstore;
        TempEquipData._Sell = _equipData._Sell;
        TempEquipData._Sellprice = _equipData._Sellprice;
        TempEquipData._skillText = _equipData._skillText;
        TempEquipData._Specprice = _equipData._Specprice;
        TempEquipData._Specstore = _equipData._Specstore;
        TempEquipData._sprite = _equipData._sprite;
        TempEquipData._Text = _equipData._Text;
        TempEquipData._Type = _equipData._Type;

        return TempEquipData;
    }
    public AidData SettingAidData(AidData _AidData)
    {
        AidData TempAidData = new AidData();
        TempAidData._Codex = _AidData._Codex;
        TempAidData._NameKR = _AidData._NameKR;
        TempAidData._NameEN = _AidData._NameEN;
        TempAidData._Type = _AidData._Type;
        TempAidData._image = _AidData._image;
        TempAidData._imageTile = _AidData._imageTile;
        TempAidData._sprite = _AidData._sprite;
        TempAidData._spriteTile = _AidData._spriteTile;
        TempAidData._Token = _AidData._Token;
        TempAidData._Restore = _AidData._Restore;
        TempAidData._Money = _AidData._Money;
        TempAidData._Fight = _AidData._Fight;
        TempAidData._Dectective = _AidData._Dectective;
        TempAidData._Move = _AidData._Move;
        TempAidData._Contract = _AidData._Contract;
        TempAidData._Payment = _AidData._Payment;
        TempAidData._Endure = _AidData._Endure;
        TempAidData._CoolTime = _AidData._CoolTime;
        TempAidData._currCoolTime = _AidData._currCoolTime;
        TempAidData._Text = _AidData._Text;
        TempAidData._isGet = _AidData._isGet;

        return TempAidData;
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
                    TempItemData = SettingItemData(_ItemDatasList[Random.Range(0, _ItemDatasList.Count)]);
                    tempImgIcon.sprite = TempItemData._sprite;
                    TempText.text = TempItemData._NameKR;
                    TempType.text = "아이템 획득";
                    _playerInfoCS._BoxItemList.Add(TempItemData);
                    return true;
                }
                else if (TempRewardData._RewardAidRandom >= Random.Range(0, 100)) 
                {
                    TempAidData = SettingAidData(_AidDatasList[Random.Range(0, _AidDatasList.Count)]);
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
                    Debug.Log("11 작동");
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
