using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class ItemData
{
    public int _Codex = 0;
    public string _NameKR = "";
    public string _NameEN = "";
    public string _Type = "";
    public string _itemName = "";
    public string _itemContent = "";
    public int _buyValue = 0;
}

public class ShopMgr : MonoBehaviour {
    public Sprite[] _chrImg;
    public List<ItemData> _itemDatas = new List<ItemData>();

    public GameObject _itemScroll;
    public GameObject _content;
    private ScrollRect _scrollRect;
    private GameObject _ChrImgGO;

    public GameObject _itemBox;

    public void ReadCSV()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/UnitTable");

        for (int i = 0; i < date.Count; i++)
        {
            ItemData TempItemData = new ItemData();
            TempItemData._buyValue

        }
    }

    public void ShopSetting(int value)
    {
        if (value == 0) _ChrImgGO.GetComponent<Image>().sprite = _chrImg[0];
        else if (value == 1) _ChrImgGO.GetComponent<Image>().sprite = _chrImg[1];
    }

    private void Awake()
    {
        _ChrImgGO = transform.GetChild(0).gameObject;
        ShopSetting(0);

        _scrollRect = _itemScroll.GetComponent<ScrollRect>();
        GameObject tempGO;
        for (int i = 0; i < _itemDatas.Count; i++)
        {
            tempGO = Instantiate(_itemBox, _content.transform);

            if (i != 0) tempGO.GetComponent<RectTransform>().position -=
                    Vector3.down * -(tempGO.GetComponent<RectTransform>().rect.height);

        }
        //_scrollRect.content.sizeDelta = new Vector2(100.0f, 100.0f);

        //tempGO = Instantiate(_itemBox, _itemScroll.transform);
        //_scrollRect.content.sizeDelta = new Vector2(100.0f, 100.0f);


        //for (int i = 0; i < _itemDatas.Count; i++)
        //{
        //}
        SetContentSize();
    }

    private void Start()
    {

    }

    private void SetContentSize()
    {
        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = _itemBox.GetComponent<RectTransform>().rect.height * _itemDatas.Count;

        _scrollRect.content.sizeDelta = new Vector2(width, height);
    }

}
