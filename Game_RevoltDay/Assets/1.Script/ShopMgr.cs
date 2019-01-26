using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum eItemType
{
    Non,
    Healing,
    Buff,
    Move
}
public enum eEquipType
{
    Non,
    Fight,
    Dectective
}
public enum eAidType
{
    Non,
    Buff,
    Now
}


[System.Serializable]
public class ItemData
{
    public int _Codex = 0;
    public string _NameKR = "";
    public string _NameEN = "";
    public eItemType _Type = eItemType.Non;
    public string _image = "";
    public Sprite _sprite = null;
    public int _Bundle = 0;
    public int _Restore = 0;
    public int _Fight = 0;
    public int _Dectective = 0;
    public int _Move = 0;
    public bool _Nomalstore = false;
    public int _Nomalprice = 0;
    public bool _Specstore = false;
    public int _Specprice = 0;
    public bool _Sell = false;
    public int _Sellprice = 0;
    public int _Endure = 0;
    public int _Chance = 0;
    public int _TurnOtp = 0;
    public int _currTurnOtp = 0;
    public string _Text = "";
}
[System.Serializable]
public class EquipData
{
    public int _Codex = 0;
    public string _NameKR = "";
    public string _NameEN = "";
    public eEquipType _Type = eEquipType.Non;
    public string _image = "";
    public Sprite _sprite = null;
    public int _Bundle = 0;
    public int _Fight = 0;
    public eDuelType _DuelType = eDuelType.P_Grappler;
    public int _Dectective = 0;
    public int _Move = 0;
    public bool _Nomalstore = false;
    public int _Nomalprice = 0;
    public bool _Specstore = false;
    public int _Specprice = 0;
    public bool _Sell = false;
    public int _Sellprice = 0;
    public string _Text = "";
    public string _skillText = "";

    public bool _isSet = false;
}
[System.Serializable]
public class AidData
{
    public int _Codex = 0;
    public string _NameKR = "";
    public string _NameEN = "";
    public eAidType _Type = eAidType.Non;
    public string _image = "";
    public string _imageTile = "";
    public Sprite _sprite = null;
    public Sprite _spriteTile = null;
    public int _Token = 0;
    public int _Restore = 0;
    public int _Money = 0;
    public int _Fight = 0;
    public int _Dectective = 0;
    public int _Move = 0;
    public int _Contract = 0;
    public int _Payment = 0;
    public int _Endure = 0;
    public int _CoolTime = 0;
    public int _currCoolTime = 0;
    public string _Text = "";
    public bool _isGet = false;

    public bool _isSet = false;
}

public class ShopMgr : MonoBehaviour {
    public PlayerInfoCS _playerInfoCS;
    public SaveSys _gameMgr;

    public Sprite[] _chrImg;
    public List<ItemData> _itemDatas = new List<ItemData>();
    public List<EquipData> _EquipDatas = new List<EquipData>();
    public List<AidData> _AidDatas = new List<AidData>();

    public List<GameObject> _itemBoxs = new List<GameObject>();

    public GameObject _itemScroll;
    public GameObject _content;
    private ScrollRect _scrollRect;
    public GameObject _ChrImgGO;
    public GameObject _BuyPopup;
    public GameObject _BuyMsgPopup;

    public GameObject _itemBotttomInfo;
    public GameObject _itemBox;

    public ItemData _currSelectItem;
    public EquipData _currSelectEquip;

    public bool _isShopSP = false;

    public bool _TestGetItem = false;

    public delegate void ItemSelet(bool _is);
    public static event ItemSelet _isItemSelet;

    public void isItemSeletSys(bool _is)
    {
        _isItemSelet(_is);
    }

    // 아이템 정보 불러오기
    public void ReadCSV()
    {
        List<Dictionary<string, object>> date;

        // 장비 테이블 로드
        #region
        date = CSVReader.Read("2.SceneTable/EquipTable");

        for (int i = 0; i < date.Count; i++)
        {
            EquipData TempItemData = new EquipData();
            TempItemData._Codex = (int)date[i]["Codex"];
            TempItemData._NameKR = (string)date[i]["NameKR"];
            TempItemData._NameEN = (string)date[i]["NameEN"];

            switch ((string)date[i]["Type"])
            {
                case "격투":
                    TempItemData._Type = eEquipType.Fight;
                    break;
                case "추리":
                    TempItemData._Type = eEquipType.Dectective;
                    break;
                default:
                    Debug.Log("ReadCSV : 존재하지 않는 아이템 타입 입니다.");
                    break;
            }

            TempItemData._image = (string)date[i]["Image"];
            Sprite temp = null;
            ResourceMgrCS._IconImg.TryGetValue(TempItemData._image, out temp);
            TempItemData._sprite = temp;

            TempItemData._Bundle = (int)date[i]["Bundle"];
            TempItemData._Fight = (int)date[i]["Fight"];

            switch ((string)date[i]["Skill"])
            {
                case "Infight":
                    TempItemData._DuelType = eDuelType.S_InFighter;
                    break;
                case "Outfight":
                    TempItemData._DuelType = eDuelType.R_OutFighter;
                    break;
                case "Grappling":
                    TempItemData._DuelType = eDuelType.P_Grappler;
                    break;
                case "Defence":
                    TempItemData._DuelType = eDuelType.D_Defence;
                    break;
                default:
                    //Debug.Log("ReadCSV : 존재하지 않는 아이템 타입 입니다.");
                    break;
            }

            TempItemData._Dectective = (int)date[i]["Dectective"];
            TempItemData._Move = (int)date[i]["Move"];

            if ((string)date[i]["Nomalstore"] == "TRUE") TempItemData._Nomalstore = true;
            else TempItemData._Nomalstore = false;

            TempItemData._Nomalprice = (int)date[i]["Nomalprice"];

            if ((string)date[i]["Specstore"] == "TRUE") TempItemData._Specstore = true;
            else TempItemData._Specstore = false;

            TempItemData._Specprice = (int)date[i]["Specprice"];

            if ((string)date[i]["Sell"] == "TRUE") TempItemData._Sell = true;
            else TempItemData._Sell = false;

            TempItemData._Sellprice = (int)date[i]["Sellprice"];
            TempItemData._Text = (string)date[i]["Text"];
            TempItemData._skillText = (string)date[i]["Skilltext"];

            _EquipDatas.Add(TempItemData);
            if (_TestGetItem) _playerInfoCS._BoxEquipList.Add(TempItemData);
        }
        Debug.Log("장비 테이블 로드");
        #endregion

        // 아이템 테이블 로드
        #region
        date = CSVReader.Read("2.SceneTable/ItemTable");

        for (int i = 0; i < date.Count; i++)
        {
            ItemData TempItemData = new ItemData();
            TempItemData._Codex = (int)date[i]["Codex"];
            TempItemData._NameKR = (string)date[i]["NameKR"];
            TempItemData._NameEN = (string)date[i]["NameEN"];

            switch ((string)date[i]["Type"])
            {
                case "회복":
                    TempItemData._Type = eItemType.Healing;
                    break;
                case "버프":
                    TempItemData._Type = eItemType.Buff;
                    break;
                case "이동":
                    TempItemData._Type = eItemType.Move;
                    break;
                default:
                    Debug.Log("ReadCSV : 존재하지 않는 아이템 타입 입니다.");
                    break;
            }

            TempItemData._image = (string)date[i]["Image"];
            Sprite temp = null;
            ResourceMgrCS._IconImg.TryGetValue(TempItemData._image,out temp);
            TempItemData._sprite = temp;
            TempItemData._Bundle = (int)date[i]["Bundle"];
            TempItemData._Restore = (int)date[i]["Restore"];
            TempItemData._Fight = (int)date[i]["Fight"];
            TempItemData._Dectective = (int)date[i]["Dectective"];
            TempItemData._Move = (int)date[i]["Move"];

            if ((string)date[i]["Nomalstore"] == "TRUE") TempItemData._Nomalstore = true;
            else TempItemData._Nomalstore = false;

            TempItemData._Nomalprice = (int)date[i]["Nomalprice"];

            if ((string)date[i]["Specstore"] == "TRUE") TempItemData._Specstore = true;
            else TempItemData._Specstore = false;

            TempItemData._Specprice = (int)date[i]["Specprice"];

            if ((string)date[i]["Sell"] == "TRUE") TempItemData._Sell = true;
            else TempItemData._Sell = false;

            TempItemData._Sellprice = (int)date[i]["Sellprice"];
            TempItemData._Endure = (int)date[i]["Endure"];
            TempItemData._Chance = (int)date[i]["chance"];
            TempItemData._TurnOtp = (int)date[i]["turnOpt"];
            TempItemData._currTurnOtp = TempItemData._TurnOtp;
            TempItemData._Text = (string)date[i]["Text"];

            _itemDatas.Add(TempItemData);
            if (_TestGetItem) _playerInfoCS._BoxItemList.Add(TempItemData);
        }
        Debug.Log("아이템 테이블 로드");

        itemBottomSetting(_itemDatas[0]);
        #endregion

        // 조력자 테이블 로드
        #region
        date = CSVReader.Read("2.SceneTable/AIdTable");

        for (int i = 0; i < date.Count; i++)
        {
            AidData TempItemData = new AidData();
            TempItemData._Codex = (int)date[i]["Codex"];
            TempItemData._NameKR = (string)date[i]["NameKR"];
            TempItemData._NameEN = (string)date[i]["NameEN"];

            switch ((string)date[i]["Type"])
            {
                case "버프":
                    TempItemData._Type = eAidType.Buff;
                    break;
                case "일시":
                    TempItemData._Type = eAidType.Now;
                    break;
                default:
                    Debug.Log("ReadCSV : 존재하지 않는 아이템 타입 입니다.");
                    break;
            }

            TempItemData._image = (string)date[i]["Image"];
            Sprite temp = null;
            ResourceMgrCS._IconImg.TryGetValue(TempItemData._image, out temp);
            TempItemData._sprite = temp;
            TempItemData._imageTile = (string)date[i]["Image_Tile"];
            ResourceMgrCS._IconImg.TryGetValue(TempItemData._imageTile, out temp);
            TempItemData._spriteTile = temp;

            TempItemData._Token = (int)date[i]["Token"];
            TempItemData._Restore = (int)date[i]["Restore"];
            TempItemData._Money = (int)date[i]["Money"];
            TempItemData._Fight = (int)date[i]["Fight"];
            TempItemData._Dectective = (int)date[i]["Dectective"];
            TempItemData._Move = (int)date[i]["Move"];
            TempItemData._Contract = (int)date[i]["Contract"];
            TempItemData._Payment = (int)date[i]["Payment"];
            TempItemData._Endure = (int)date[i]["Endure"];
            TempItemData._CoolTime = (int)date[i]["CoolTime"];
            TempItemData._Text = (string)date[i]["Text"];
            TempItemData._isGet = false;

            _AidDatas.Add(TempItemData);
            if (_TestGetItem) _playerInfoCS._BoxAidList.Add(TempItemData);
        }
        Debug.Log("조력자 테이블 로드");
        // 테스트용 코드

        #endregion
    }

    public void buyTrueCheck()
    {
        if (_isShopSP)
        {
            for (int i = 0; i < 4; i++)
            {
                // 현재 소지금에 따른 아이템 음영처리
                if (_playerInfoCS._currMoney < _EquipDatas[i]._Nomalprice) itemBuyNotColor(_itemBoxs[i], true);
                else itemBuyNotColor(_itemBoxs[i], false);
            }
        }
        else
        {
            for (int i = 0; i < _itemDatas.Count; i++)
            {
                // 현재 소지금에 따른 아이템 음영처리
                if (_playerInfoCS._currMoney < _itemDatas[i]._Nomalprice) itemBuyNotColor(_itemBoxs[i], true);
                else itemBuyNotColor(_itemBoxs[i], false);
            }
        }
    }

    public void BuyPopupSys(bool _is) { _BuyPopup.SetActive(_is); }
    public void SetBuyItem(ItemData _itemData) { _currSelectEquip = null; _currSelectItem = _itemData; }
    public void SetBuyEquip(EquipData _EquipData) { _currSelectItem = null; _currSelectEquip = _EquipData; }

    public void BuyMsgPopup(bool _is)
    {
        if (_currSelectItem == null) _playerInfoCS._BoxEquipList.Add(ResourceMgrCS.SettingEquipData(_currSelectEquip));
        else _playerInfoCS._BoxItemList.Add(ResourceMgrCS.SettingItemData(_currSelectItem));

        _BuyPopup.SetActive(_is);
        StartCoroutine(MsgEff());
    }

    public IEnumerator MsgEff()
    {
        _BuyMsgPopup.SetActive(true);
        ItemDataCS tempItemDataCS;
        if (_currSelectItem == null)
        {
            for (int i = 0; i < _EquipDatas.Count; i++)
            {
                if (!_itemBoxs[i].GetComponent<ItemDataCS>()._isSelect) continue;
                else
                {
                    tempItemDataCS = _itemBoxs[i].GetComponent<ItemDataCS>();
                    _BuyMsgPopup.transform.GetChild(3).gameObject.GetComponent<Text>().text = tempItemDataCS._currEquipData._NameKR;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < _itemDatas.Count; i++)
            {
                if (!_itemBoxs[i].GetComponent<ItemDataCS>()._isSelect) continue;
                else
                {
                    tempItemDataCS = _itemBoxs[i].GetComponent<ItemDataCS>();
                    _BuyMsgPopup.transform.GetChild(3).gameObject.GetComponent<Text>().text = tempItemDataCS._currItemData._NameKR;
                    break;
                }
            }
        }


        float TempTime = 0.0f;
        while (TempTime < 1.0f)
        {
            TempTime += Time.deltaTime;
            yield return null;
        }
        _BuyMsgPopup.SetActive(false);
        yield break;
    }

    public void ShopSetting(int value)
    {
        if (value == 0) _ChrImgGO.GetComponent<Image>().sprite = _chrImg[0];
        else if (value == 1) _ChrImgGO.GetComponent<Image>().sprite = _chrImg[1];
    }

    public void ShopStart()
    {
        _itemBox.SetActive(true);
        for (int i = 0; i < _itemBoxs.Count; i++)
        {
            Destroy(_itemBoxs[i]);
        }
        _itemBoxs.Clear();

        if (_playerInfoCS._currTile == 1) SettingShopSP();
        else if (_playerInfoCS._currTile == 7) SettingShopNormal();
        _itemBox.SetActive(false);

        buyTrueCheck();
    }

    public void itemBuyNotColor(GameObject gameObject, bool _is)
    {
        Color tempColor;
        if (!_is) tempColor = new Color(1, 1, 1, 1);
        else tempColor = new Color(0.5f, 0.5f, 0.5f, 1);

        gameObject.GetComponent<Image>().color = tempColor;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = tempColor;
        gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().color = tempColor;
        gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().color = tempColor;
    }

    private void Awake()
    {
        ReadCSV();
        _scrollRect = _itemScroll.GetComponent<ScrollRect>();

        // 장비 로드
        loadItemList();

        _itemBox.SetActive(false);
    }
    
    public void loadItemList() // 저장 데이터 불러오기
    {
        SaveSys tempData = _gameMgr.GetComponent<SaveSys>();

        // 아이템 로드
        for (int i = 0; i < tempData._saveFile._currItemDatasList.Length; i++)
        {
            if (tempData._saveFile._currItemDatasList[i]._Index == 0) break;
            for (int j = 0; j < _itemDatas.Count; j++)
            {
                if (tempData._saveFile._currItemDatasList[i]._Index == _itemDatas[j]._Codex)
                {
                    ItemData TempItemData = ResourceMgrCS.SettingItemData(_itemDatas[j]);
                    _playerInfoCS._BoxItemList.Add(TempItemData);
                    break;
                }
            }
        }

        // 사용된 아이템 로드
        for (int i = 0; i < tempData._saveFile._useItemList.Length; i++)
        {
            if (tempData._saveFile._useItemList[i]._Index == 0) break;
            for (int j = 0; j < _itemDatas.Count; j++)
            {
                if (tempData._saveFile._useItemList[i]._Index == _itemDatas[j]._Codex)
                {
                    ItemData TempItemData = ResourceMgrCS.SettingItemData(_itemDatas[j]);
                    TempItemData._currTurnOtp = tempData._saveFile._useItemList[i]._currTurn;

                    _playerInfoCS._currUseItemList.Add(TempItemData);
                    break;
                }
            }
        }


        // 장비 로드
        for (int i = 0; i < tempData._saveFile._currEquipDatasList.Length; i++)
        {
            if (tempData._saveFile._currEquipDatasList[i]._Index == 0) break;
            for (int j = 0; j < _EquipDatas.Count; j++)
            {
                if (tempData._saveFile._currEquipDatasList[i]._Index == _EquipDatas[j]._Codex)
                {
                    EquipData TempEquipData = ResourceMgrCS.SettingEquipData(_EquipDatas[j]);
                    TempEquipData._isSet = tempData._saveFile._currEquipDatasList[i]._setUse;
                    if (TempEquipData._isSet)
                    {
                        switch (TempEquipData._Type)
                        {
                            case eEquipType.Non:
                                break;
                            case eEquipType.Fight:
                                _playerInfoCS._currUseEquipF = TempEquipData;
                                break;
                            case eEquipType.Dectective:
                                _playerInfoCS._currUseEquipD = TempEquipData;
                                break;
                            default:
                                break;
                        }
                    }

                    _playerInfoCS._BoxEquipList.Add(TempEquipData);
                    break;
                }
            }
        }


        // 조력자 로드
        for (int i = 0; i < tempData._saveFile._currAidDatasList.Length; i++)
        {
            if (tempData._saveFile._currAidDatasList[i]._Index == 0) break;
            for (int j = 0; j < _AidDatas.Count; j++)
            {
                if (tempData._saveFile._currAidDatasList[i]._Index == _AidDatas[j]._Codex)
                {
                    AidData TempAidData = ResourceMgrCS.SettingAidData(_AidDatas[j]);
                    TempAidData._currCoolTime = tempData._saveFile._currAidDatasList[i]._currTurn;
                    TempAidData._isGet = tempData._saveFile._currAidDatasList[i]._isGet;
                    TempAidData._isSet = tempData._saveFile._currAidDatasList[i]._setUse;
                    if (TempAidData._isSet) _playerInfoCS._currUseAid = TempAidData;

                    _playerInfoCS._BoxAidList.Add(TempAidData);
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (_TestGetItem)
        {
            _TestGetItem = false;

            for (int i = 0; i < _itemDatas.Count; i++)
            {
                _playerInfoCS._BoxItemList.Add(ResourceMgrCS.SettingItemData(_itemDatas[i]));
            }
            for (int i = 0; i < _AidDatas.Count; i++)
            {
                _playerInfoCS._BoxAidList.Add(ResourceMgrCS.SettingAidData(_AidDatas[i]));
            }
            for (int i = 0; i < _EquipDatas.Count; i++)
            {
                _playerInfoCS._BoxEquipList.Add(ResourceMgrCS.SettingEquipData(_EquipDatas[i]));
            }
        }
    }

    public void SettingShopNormal()
    {
        _isShopSP = false;
        ShopSetting(1);

        GameObject tempGO;
        for (int i = 0; i < _itemDatas.Count; i++)
        {
            tempGO = itemOptionSet(_itemDatas[i]);
            if (i != 0)
            {
                tempGO.GetComponent<RectTransform>().position -=
                    Vector3.down * -(tempGO.GetComponent<RectTransform>().rect.height * i);
            }

            _isItemSelet += tempGO.GetComponent<ItemDataCS>().isSelect;
            _itemBoxs.Add(tempGO);
        }

        SetContentSize(false);
    }

    public void SettingShopSP()
    {
        _isShopSP = true;

        ShopSetting(0);

        // 아이템 셔플
        #region
        int[] TempRand = new int[4];
        for (int i = 0; i < 4; i++)
        {
            TempRand[i] = Random.Range(0, _EquipDatas.Count);
            for (int j = 0; j < 4; j++)
            {
                if (i == j) continue;

                if (TempRand[i] == TempRand[j])
                {
                    TempRand[i] = Random.Range(0, _EquipDatas.Count);
                    j = 0;
                }
            }
        }
        Debug.Log(TempRand[0].ToString() + TempRand[1].ToString() + TempRand[2].ToString() + TempRand[3].ToString());


        #endregion

        GameObject tempGO;
        for (int i = 0; i < TempRand.Length; i++)
        {
            tempGO = EquipOptionSet(_EquipDatas[TempRand[i]]);
            if (i != 0)
            {
                tempGO.GetComponent<RectTransform>().position -=
                    Vector3.down * -(tempGO.GetComponent<RectTransform>().rect.height * i);
            }

            _isItemSelet += tempGO.GetComponent<ItemDataCS>().isSelect;
            _itemBoxs.Add(tempGO);
        }

        SetContentSize(true);
    }

    public GameObject itemOptionSet(ItemData dateInfo)
    {
        GameObject tempItem = Instantiate(_itemBox, _content.transform);
        tempItem.GetComponent<ItemDataCS>()._currItemData = dateInfo;
        tempItem.transform.GetChild(1).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        tempItem.transform.GetChild(2).gameObject.GetComponent<Text>().text = dateInfo._Nomalprice.ToString() + "원";

        return tempItem;
    }

    public GameObject EquipOptionSet(EquipData dateInfo)
    {
        GameObject tempEquip = Instantiate(_itemBox, _content.transform);
        tempEquip.GetComponent<ItemDataCS>()._currEquipData = dateInfo;
        tempEquip.transform.GetChild(1).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        tempEquip.transform.GetChild(2).gameObject.GetComponent<Text>().text = dateInfo._Specprice.ToString() + "원";

        return tempEquip;
    }

    public void itemBottomSetting(ItemData dateInfo)
    {
        _itemBotttomInfo.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = dateInfo._sprite;
        _itemBotttomInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        _itemBotttomInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = dateInfo._Text;
        _itemBotttomInfo.transform.GetChild(5).gameObject.GetComponent<Text>().text = dateInfo._Nomalprice.ToString() + "원";
    }

    public void EquipBottomSetting(EquipData dateInfo)
    {
        _itemBotttomInfo.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = dateInfo._sprite;
        _itemBotttomInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        _itemBotttomInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = dateInfo._Text;
        _itemBotttomInfo.transform.GetChild(5).gameObject.GetComponent<Text>().text = dateInfo._Specprice.ToString() + "원";
    }


    private void SetContentSize(bool _isSP)
    {
        float width = 0.0f;
        float height = 0.0f;

        if (_isSP)
        {
            width = _itemBox.GetComponent<RectTransform>().rect.width;
            height = _itemBox.GetComponent<RectTransform>().rect.height * 4;
        }
        else
        {

            width = _itemBox.GetComponent<RectTransform>().rect.width;
            height = _itemBox.GetComponent<RectTransform>().rect.height * _itemDatas.Count;
        }

        _scrollRect.content.sizeDelta = new Vector2(width, height);
    }

}
