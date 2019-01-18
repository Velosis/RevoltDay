using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMgrCS : MonoBehaviour {
    public PlayerInfoCS _playerInfoCS;

    public GameObject _TabButtonSetGO;
    private GameObject _StateTabButton;
    private GameObject _EquipTabButton;
    private GameObject _ItemTabButton;
    private GameObject _CardTabButton;

    public GameObject _StateScreenSetGO;
    private GameObject _StateScreen;
    private GameObject _EquipScreen;
    private GameObject _ItemScreen;
    private GameObject _CardScreen;


    public GameObject _itemBox;
    private ScrollRect _scrollRect;

    private void Awake()
    {
        _StateTabButton = _TabButtonSetGO.transform.GetChild(0).gameObject;
        _EquipTabButton = _TabButtonSetGO.transform.GetChild(1).gameObject;
        _ItemTabButton = _TabButtonSetGO.transform.GetChild(2).gameObject;
        _CardTabButton = _TabButtonSetGO.transform.GetChild(3).gameObject;

        _StateScreen = _StateScreenSetGO.transform.GetChild(0).gameObject;
        _EquipScreen = _StateScreenSetGO.transform.GetChild(1).gameObject;
        _ItemScreen = _StateScreenSetGO.transform.GetChild(2).gameObject;
        _CardScreen = _StateScreenSetGO.transform.GetChild(3).gameObject;

        StateMgrUIOff();
    }

    private void OnEnable()
    {
        BoxItemListSetting(_EquipScreen);
        BoxItemListSetting(_ItemScreen);
        BoxItemListSetting(_CardScreen);
    }

    public void BoxItemListSetting(GameObject isScreen)
    {
        if (_playerInfoCS._BoxItemList.Count == 0) return;


        int TempSize = _playerInfoCS._BoxItemList.Count;
        Debug.Log(TempSize + "개 있음");
        GameObject TempCurrScreen = isScreen;

        //if (_EquipScreen.activeSelf) TempCurrScreen = _EquipScreen;
        //else if (_ItemScreen.activeSelf) TempCurrScreen = _ItemScreen;
        //else if (_CardScreen.activeSelf) TempCurrScreen = _CardScreen;

        _scrollRect = TempCurrScreen.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<ScrollRect>();
        GameObject TempContent = TempCurrScreen.transform.GetChild(1).
            gameObject.transform.GetChild(0).
            gameObject.transform.GetChild(0).
            gameObject.transform.GetChild(0).gameObject;

        if (TempContent.transform.childCount != 0)
        {
            for (int i = 0; i < TempContent.transform.childCount; i++)
            {
                Destroy(TempContent.transform.GetChild(i).gameObject);
            }
        }

        _itemBox.SetActive(true);
        int HeightCount = 0;
        int WidthCount = 0;
        for (int i = 0; i < TempSize; i++)
        {
            GameObject TempBox = Instantiate(_itemBox, TempContent.transform);
            //TempBox.transform.GetChild(0).gameObject.GetComponent<Image>() = _itemBox.GetComponent<ItemData>()._NameKR;
            TempBox.transform.GetChild(1).gameObject.GetComponent<Text>().text = _playerInfoCS._BoxItemList[i]._NameKR;
            //기본 우측 정렬
            TempBox.GetComponent<RectTransform>().position += Vector3.right * (TempBox.GetComponent<RectTransform>().rect.width / 2.0f + 20.0f);
            TempBox.GetComponent<RectTransform>().position += Vector3.down * (TempBox.GetComponent<RectTransform>().rect.height / 2.0f + 10.0f);

            //크기에 따른 정렬
            TempBox.GetComponent<RectTransform>().position += Vector3.right * ((TempBox.GetComponent<RectTransform>().rect.width * WidthCount));

            TempBox.GetComponent<RectTransform>().position += Vector3.down * ((TempBox.GetComponent<RectTransform>().rect.height * HeightCount));
            if (WidthCount != 0) TempBox.GetComponent<RectTransform>().position += Vector3.right * (WidthCount * 10.0f);
            TempBox.GetComponent<RectTransform>().position += Vector3.down * (HeightCount * 10.0f);

            WidthCount++;
            if (WidthCount >= 5)
            {
                WidthCount = 0;
                HeightCount++;
            }

            Debug.Log(i + "개 존재");
        }
        _itemBox.SetActive(false);

        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = (_itemBox.GetComponent<RectTransform>().rect.height + (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f + 10.0f)) * HeightCount;
        //height -= (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f - 10.0f);
        Debug.Log(width + ", " + height);
        Debug.Log(_scrollRect.content.sizeDelta);
        _scrollRect.content.sizeDelta = new Vector2(width, height);
        Debug.Log(_scrollRect.content.sizeDelta);

    }

    public void StateMgrUI(int value)
    {
        StateMgrUIOff();
        Color TempColorB = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        switch (value)
        {
            case 0:
                _StateTabButton.GetComponent<Image>().color = TempColorB;
                _StateScreen.SetActive(true);
                break;
            case 1:
                _EquipTabButton.GetComponent<Image>().color = TempColorB;
                _EquipScreen.SetActive(true);
                break;
            case 2:
                _ItemTabButton.GetComponent<Image>().color = TempColorB;
                _ItemScreen.SetActive(true);
                break;
            case 3:
                _CardTabButton.GetComponent<Image>().color = TempColorB;
                _CardScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void StateMgrUIOff()
    {
        Color TempColorA = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        _StateTabButton.GetComponent<Image>().color = TempColorA;
        _EquipTabButton.GetComponent<Image>().color = TempColorA;
        _ItemTabButton.GetComponent<Image>().color = TempColorA;
        _CardTabButton.GetComponent<Image>().color = TempColorA;

        _StateScreen.SetActive(false);
        _EquipScreen.SetActive(false);
        _ItemScreen.SetActive(false);
        _CardScreen.SetActive(false);
    }
}
