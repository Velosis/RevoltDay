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

[System.Serializable]
public class ItemData
{
    public int _Codex = 0;
    public string _NameKR = "";
    public string _NameEN = "";
    public eItemType _Type = eItemType.Non;
    public string _image;
    public int _Bundle;
    public int _Restore;
    public int _Fight;
    public int _Dectective;
    public int _Move;
    public bool _Nomalstore;
    public int _Nomalprice;
    public bool _Specstore;
    public int _Specprice;
    public bool _Sell;
    public int _Sellprice;
    public int _Endure;
    public int _Chance;
    public string _Text;
}

public class ShopMgr : MonoBehaviour {
    public PlayerInfoCS _playerInfoCS;

    public Sprite[] _chrImg;
    public List<ItemData> _itemDatas = new List<ItemData>();
    public List<GameObject> _itemBoxs = new List<GameObject>();

    public GameObject _itemScroll;
    public GameObject _content;
    private ScrollRect _scrollRect;
    public GameObject _ChrImgGO;
    public GameObject _BuyPopup;
    public GameObject _BuyMsgPopup;

    public GameObject _itemBotttomInfo;
    public GameObject _itemBox;

    public delegate void ItemSelet(bool _is);
    public static event ItemSelet _isItemSelet;

    public void isItemSeletSys(bool _is)
    {
        _isItemSelet(_is);
    }


    public void ReadCSV()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/ItemTable");

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
            TempItemData._Text = (string)date[i]["Text"];

            _itemDatas.Add(TempItemData);
        }

        itemBottomSetting(_itemDatas[0]);
    }

    public void buyTrueCheck()
    {
        for (int i = 0; i < _itemDatas.Count; i++)
        {
            // 현재 소지금에 따른 아이템 음영처리
            if (_playerInfoCS._currMoney < _itemDatas[i]._Nomalprice) itemBuyNotColor(_itemBoxs[i], true);
            else itemBuyNotColor(_itemBoxs[i], false);
        }
    }

    public void BuyPopupSys(bool _is) { _BuyPopup.SetActive(_is); }
    public void BuyMsgPopup(bool _is)
    {
        _BuyPopup.SetActive(_is);
        StartCoroutine(MsgEff());
    }

    public IEnumerator MsgEff()
    {
        _BuyMsgPopup.SetActive(true);
        ItemDataCS tempItemDataCS;
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
        ShopSetting(1);

        _scrollRect = _itemScroll.GetComponent<ScrollRect>();

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
        _itemBox.SetActive(false);

        SetContentSize();
    }

    private void Start()
    {
        ShopStart();
    }

    public GameObject itemOptionSet(ItemData dateInfo)
    {
        GameObject tempItem = Instantiate(_itemBox, _content.transform);
        tempItem.GetComponent<ItemDataCS>()._currItemData = dateInfo;
        tempItem.transform.GetChild(1).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        tempItem.transform.GetChild(2).gameObject.GetComponent<Text>().text = dateInfo._Nomalprice.ToString() + "원";

        return tempItem;
    }

    public void itemBottomSetting(ItemData dateInfo)
    {
        _itemBotttomInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = dateInfo._NameKR;
        _itemBotttomInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = dateInfo._Text;
        _itemBotttomInfo.transform.GetChild(5).gameObject.GetComponent<Text>().text = dateInfo._Nomalprice.ToString() + "원";
    }

    private void SetContentSize()
    {
        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = _itemBox.GetComponent<RectTransform>().rect.height * _itemDatas.Count;

        _scrollRect.content.sizeDelta = new Vector2(width, height);
    }

}
