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

    public GameObject _currScreen;
    private ItemData _currSelectItem;
    private EquipData _currSelectEquip;
    private AidData _currSelectAid;

    private Image _ItemInfoImg;
    private Text _ItemName;
    private Text _ItemTextName;
    private Text _ItemStateText;
    private Text _ItemTypeText;
    private Text _ItemStateBonusText;
    private Text _ItemIsGetText;
    private Text _ItemContractText;
    private Text _ItemPaymentText;
    private Text _ItemCoolTimeText;
    private Text _ItemText;

    public GameObject _UseAIdPupupBox;
    public GameObject _UseAIdSysPupupBox;
    public GameObject _NotUseAidSysPupup;

    public GameObject _UseSetEquipPupup;
    public GameObject _NotUseEquipSysPupup;


    public GameObject _itemBox;
    private ScrollRect _scrollRect;

    public Sprite[] _ChrCgSprites;

    public delegate void ItemSelectState(bool _is);
    public static event ItemSelectState _isItemSelect;

    private void Awake()
    {
        _UseAIdPupupBox.SetActive(false);
        _UseAIdSysPupupBox.SetActive(false);
        _NotUseAidSysPupup.SetActive(false);
        _UseSetEquipPupup.SetActive(false);
        _NotUseEquipSysPupup.SetActive(false);

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
        BoxEquipListSetting(_EquipScreen);
        BoxItemListSetting(_ItemScreen);
        BoxAidListSetting(_CardScreen);
        StateMgrUI(0);
    }

    public IEnumerator NotUseAid(eAidType _eAidType)
    {
        Text tempText = _NotUseAidSysPupup.transform.GetChild(2).gameObject.GetComponent<Text>();
        switch (_eAidType)
        {
            case eAidType.Non:
                tempText.text = "이미 조력자에게 의뢰중입니다.";
                break;

            case eAidType.Buff:
                tempText.text = _ItemStateText.text;
                break;
            case eAidType.Now:
                tempText.text = _ItemStateText.text;
                break;
            default:
                break;
        }

        _NotUseAidSysPupup.SetActive(true);
        float TempTime = 0.0f;
        while (TempTime < 1.0f)
        {
            TempTime += Time.deltaTime;
            yield return null;
        }
        _NotUseAidSysPupup.SetActive(false);
        yield break;
    }
    public void SetAid()
    {
        // 조력자 의뢰 처리
        if (_playerInfoCS._currMoney >= _currSelectAid._Contract)
        {
            _playerInfoCS._currMoney -= _currSelectAid._Contract;
            UseAidSys(false);

            if (!_playerInfoCS.setAidUse(_currSelectAid)) return;
            StartCoroutine(NotUseAid(_currSelectAid._Type));
        }
    }
    public void UseAidSys(bool _is)
    {
        _UseAIdSysPupupBox.transform.GetChild(3).gameObject.GetComponent<Text>().text = _currSelectAid._NameKR;
        _UseAIdSysPupupBox.transform.GetChild(4).gameObject.GetComponent<Text>().text = "소지금 : " + _playerInfoCS._currMoney + "원";
        _UseAIdSysPupupBox.transform.GetChild(5).gameObject.GetComponent<Text>().text = _ItemStateText.text;
        _UseAIdSysPupupBox.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = "계약 : " + _currSelectAid._Contract + "원";

        if (_playerInfoCS._currMoney < _currSelectAid._Contract)
        {
            _UseAIdSysPupupBox.transform.GetChild(6).gameObject.GetComponent<Image>().color = Color.gray;
            _UseAIdSysPupupBox.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.gray;
        }
        else
        {
            _UseAIdSysPupupBox.transform.GetChild(6).gameObject.GetComponent<Image>().color = Color.white;
            _UseAIdSysPupupBox.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        }

        _UseAIdSysPupupBox.SetActive(_is);
    }
    public void IsUseAIdPupupUI(bool _is)
    {
        // 계약금액 표기
        _UseAIdPupupBox.transform.GetChild(3).gameObject.gameObject.GetComponent<Text>().text = _currSelectAid._NameKR;
        _UseAIdPupupBox.transform.GetChild(4).gameObject.gameObject.GetComponent<Text>().text = "소지금 : " + _playerInfoCS._currMoney + "원";

        _UseAIdPupupBox.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "계약 : " + _currSelectAid._Contract + "원";

        if (_playerInfoCS._currMoney < _currSelectAid._Contract)
        {
            _UseAIdPupupBox.transform.GetChild(5).gameObject.GetComponent<Image>().color = Color.gray;
            _UseAIdPupupBox.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.gray;
        }
        else
        {
            _UseAIdPupupBox.transform.GetChild(5).gameObject.GetComponent<Image>().color = Color.white;
            _UseAIdPupupBox.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        }

        _UseAIdPupupBox.SetActive(_is);
    }
    public void IsGetAid()
    {
        // 조력자 계약 처리
        if (_playerInfoCS._currMoney >= _currSelectAid._Contract)
        {
            _playerInfoCS._currMoney -= _currSelectAid._Contract;
            _currSelectAid._isGet = true;
            IsUseAIdPupupUI(false);
            BoxAidListSetting(_CardScreen);
            AidUiSetting(_currSelectAid);

            switch (_currSelectAid._Type)
            {
                case eAidType.Non:
                    break;
                case eAidType.Buff:
                    break;
                case eAidType.Now:

                    break;
                default:
                    break;
            }
        }
    }

    public void UseSetEquipSys()
    {
        if (_currSelectEquip._Type == eEquipType.Fight)
        {
            if (_playerInfoCS._currUseEquipF._Codex != 0)
            {
                _playerInfoCS._currUseEquipF._isSet = false;
                _playerInfoCS._currUseEquipF = new EquipData();
            }

            if (_playerInfoCS._currUseEquipF._Codex == 0)
            {
                _currSelectEquip._isSet = true;
                _playerInfoCS._currUseEquipF = _currSelectEquip;
            }
        }
        else
        {
            if (_playerInfoCS._currUseEquipD._Codex != 0)
            {
                _playerInfoCS._currUseEquipD._isSet = false;
                _playerInfoCS._currUseEquipD = new EquipData();
            }

            if (_playerInfoCS._currUseEquipD._Codex == 0)
            {
                _currSelectEquip._isSet = true;
                _playerInfoCS._currUseEquipD = _currSelectEquip;
            }
        }

        _UseSetEquipPupup.SetActive(false);
        BoxEquipListSetting(_EquipScreen);
    }

    public void IsUseEquip(bool _is)
    {
        //if (_playerInfoCS._currUseEquip._Codex != 0) _UseSetEquipPupup.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "해체";
        //else _UseSetEquipPupup.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "장착";

        // 장비 이름 표기
        _UseSetEquipPupup.transform.GetChild(3).gameObject.GetComponent<Text>().text = _currSelectEquip._NameKR;
        _UseSetEquipPupup.SetActive(_is);
    }

    public void allNotSelect() { _isItemSelect(false); }

    public void BoxEquipListSetting(GameObject isScreen)
    {
        if (_playerInfoCS._BoxEquipList.Count == 0) return;

        int TempSize = _playerInfoCS._BoxEquipList.Count;
        GameObject TempCurrScreen = isScreen;

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


            TempBox.GetComponent<ItemDataCS>()._arrIdex = i;
            TempBox.GetComponent<ItemDataCS>()._currEquipData = _playerInfoCS._BoxEquipList[i];

            if (TempBox.GetComponent<ItemDataCS>()._currEquipData._isSet) TempBox.transform.GetChild(3).gameObject.SetActive(true);
            else TempBox.transform.GetChild(3).gameObject.SetActive(false);

            TempBox.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = TempBox.GetComponent<ItemDataCS>()._currEquipData._sprite;
            TempBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = TempBox.GetComponent<ItemDataCS>()._currEquipData._NameKR;
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
        }
        _itemBox.SetActive(false);

        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = (_itemBox.GetComponent<RectTransform>().rect.height + (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f + 10.0f)) * HeightCount;
        //height -= (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f - 10.0f);
        _scrollRect.content.sizeDelta = new Vector2(width, height);
    }
    public void BoxItemListSetting(GameObject isScreen)
    {
        if (_playerInfoCS._BoxItemList.Count == 0) return;

        int TempSize = _playerInfoCS._BoxItemList.Count;
        GameObject TempCurrScreen = isScreen;

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
            TempBox.GetComponent<ItemDataCS>()._arrIdex = i;
            TempBox.GetComponent<ItemDataCS>()._currItemData = _playerInfoCS._BoxItemList[i];
            TempBox.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = TempBox.GetComponent<ItemDataCS>()._currItemData._sprite;
            TempBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = TempBox.GetComponent<ItemDataCS>()._currItemData._NameKR;
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
        }
        _itemBox.SetActive(false);

        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = (_itemBox.GetComponent<RectTransform>().rect.height + (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f + 10.0f)) * HeightCount;
        //height -= (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f - 10.0f);
        _scrollRect.content.sizeDelta = new Vector2(width, height);

    }
    public void BoxAidListSetting(GameObject isScreen)
    {
        if (_playerInfoCS._BoxAidList.Count == 0) return;

        int TempSize = _playerInfoCS._BoxAidList.Count;
        GameObject TempCurrScreen = isScreen;

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
            TempBox.GetComponent<ItemDataCS>()._arrIdex = i;
            TempBox.GetComponent<ItemDataCS>()._currAidData = _playerInfoCS._BoxAidList[i];
            TempBox.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = TempBox.GetComponent<ItemDataCS>()._currAidData._spriteTile;
            TempBox.transform.GetChild(2).gameObject.GetComponent<Text>().text = TempBox.GetComponent<ItemDataCS>()._currAidData._NameKR;
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

            if (TempBox.GetComponent<ItemDataCS>()._currAidData._isGet) TempBox.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            else TempBox.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.black;
        }
        _itemBox.SetActive(false);

        float width = _itemBox.GetComponent<RectTransform>().rect.width;
        float height = (_itemBox.GetComponent<RectTransform>().rect.height + (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f + 10.0f)) * HeightCount;
        //height -= (_itemBox.GetComponent<RectTransform>().rect.height / 2.0f - 10.0f);
        _scrollRect.content.sizeDelta = new Vector2(width, height);
    }
    public void EquipUiSetting(EquipData _EquipData)
    {
        _currSelectEquip = _EquipData;
        GameObject TempScreen = _currScreen;
        _ItemInfoImg = TempScreen.transform.GetChild(2).gameObject.GetComponent<Image>();
        _ItemName = TempScreen.transform.GetChild(3).gameObject.GetComponent<Text>();
        //_ItemTextName = TempScreen.transform.GetChild(4).gameObject.GetComponent<Text>();
        _ItemStateText = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
        _ItemStateBonusText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
        _ItemTypeText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();
        _ItemText = TempScreen.transform.GetChild(8).gameObject.GetComponent<Text>();

        _ItemInfoImg.sprite = _currSelectEquip._sprite;
        _ItemName.text = _currSelectEquip._NameKR;
        _ItemStateText.text = "스타일 : ";
        switch (_currSelectEquip._DuelType)
        {
            case eDuelType.S_InFighter:
                _ItemStateText.text += "인파이터";
                break;
            case eDuelType.R_OutFighter:
                _ItemStateText.text += "아웃파이터";
                break;
            case eDuelType.P_Grappler:
                _ItemStateText.text += "그래플러";
                break;
            case eDuelType.D_Defence:
                _ItemStateText.text += "방어";
                break;
            case eDuelType.R_Reasoning:
                _ItemStateText.text += "통찰";
                break;
            default:
                break;
        }

        _ItemTypeText.text = "타입 : ";
        switch (_currSelectEquip._Type)
        {
            case eEquipType.Non:
                break;
            case eEquipType.Fight:
                _ItemTypeText.text += "격투";
                break;
            case eEquipType.Dectective:
                _ItemTypeText.text += "추리";
                break;
            default:
                break;
        }
        _ItemStateBonusText.text = "옵션 : ";
        switch (_currSelectEquip._Type)
        {
            case eEquipType.Non:
                break;
            case eEquipType.Fight:
                _ItemStateBonusText.text += "+" + _currSelectEquip._Fight + " 격투 보너스";
                break;
            case eEquipType.Dectective:
                _ItemStateBonusText.text += "+" + _currSelectEquip._Dectective + " 추리 보정";
                break;
            default:
                break;
        }
        if(_currSelectEquip._Fight < 0) _ItemStateBonusText.text = "옵션 : " + _currSelectEquip._Fight + " 피해 방어";

        _ItemText.text = _currSelectEquip._Text;
    }
    public void ItemUiSetting(ItemData _itemData)
    {
        _currSelectItem = _itemData;
        GameObject TempScreen = _currScreen;
        _ItemInfoImg = TempScreen.transform.GetChild(2).gameObject.GetComponent<Image>();
        _ItemName = TempScreen.transform.GetChild(3).gameObject.GetComponent<Text>();
        //_ItemTextName = TempScreen.transform.GetChild(4).gameObject.GetComponent<Text>();
        _ItemStateText = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
        _ItemTypeText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
        _ItemText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();

        _ItemInfoImg.sprite = _currSelectItem._sprite;
        _ItemName.text = _currSelectItem._NameKR;
        //_ItemTextName.text = _currSelectItem._Text;
        if (_currSelectItem._Restore != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Restore + " 회복";
        else if (_currSelectItem._Fight != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Fight + " 격투 보너스";
        else if (_currSelectItem._Dectective != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Dectective + " 추리 보정";
        else if (_currSelectItem._Move != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Move + " 행동력 보너스";

        _ItemTypeText.text = "타입 : " + _currSelectItem._Type;
        _ItemText.text = _currSelectItem._Text;
    }
    public void AidUiSetting(AidData _AidData)
    {
        _currSelectAid = _AidData;
        GameObject TempScreen = _currScreen;
        TempScreen.transform.GetChild(4).gameObject.SetActive(true);
        _ItemInfoImg = TempScreen.transform.GetChild(4).gameObject.GetComponent<Image>();
        if (_currSelectAid._isGet) _ItemInfoImg.color = Color.white;
        else _ItemInfoImg.color = Color.black;

        _ItemName = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
        _ItemStateText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
        _ItemTypeText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();
        _ItemText = TempScreen.transform.GetChild(8).gameObject.GetComponent<Text>();

        _ItemIsGetText = TempScreen.transform.GetChild(9).gameObject.GetComponent<Text>(); ;
        _ItemContractText = TempScreen.transform.GetChild(10).gameObject.GetComponent<Text>(); ;
        _ItemPaymentText = TempScreen.transform.GetChild(11).gameObject.GetComponent<Text>(); ;
        _ItemCoolTimeText = TempScreen.transform.GetChild(12).gameObject.GetComponent<Text>(); ;

        _ItemIsGetText.text = _currSelectAid._isGet ? "계약완료" : "미 계약";
        _ItemContractText.text = _currSelectAid._Contract + "원";
        _ItemPaymentText.text = _currSelectAid._Payment + "원";
        if (!_currSelectAid._isGet) _ItemCoolTimeText.text = "의뢰 불가";
        else if (_currSelectAid._currCoolTime != 0) _ItemCoolTimeText.text = "대기중(" + _currSelectAid._currCoolTime + ")";
        else _ItemCoolTimeText.text = "의뢰 가능";

        _ItemInfoImg.sprite = _currSelectAid._sprite;
        _ItemName.text = _currSelectAid._NameKR;
        _ItemStateText.text = "옵션 : ";
        if (_currSelectAid._Token != 0) _ItemStateText.text += "+" + _currSelectAid._Token + " 단서 토큰";
        else if (_currSelectAid._Restore != 0) _ItemStateText.text += "+" + _currSelectAid._Restore + " 회복";
        else if (_currSelectAid._Money != 0) _ItemStateText.text += "+" + _currSelectAid._Money + " 원";
        else if (_currSelectAid._Fight != 0) _ItemStateText.text += "+" + _currSelectAid._Fight + " 결투 보너스";
        else if (_currSelectAid._Dectective != 0) _ItemStateText.text += "+" + _currSelectAid._Dectective + " 추리 보정";
        else if (_currSelectAid._Move != 0) _ItemStateText.text += "+" + _currSelectAid._Move + " 행동력 보너스";
        if (_currSelectAid._Fight < 0) _ItemStateText.text = "옵션 : " + _currSelectAid._Fight + " 피해 방어";

        _ItemTypeText.text = "처리 : ";

        switch (_currSelectAid._Type)
        {
            case eAidType.Non:
                break;
            case eAidType.Buff:
                _ItemTypeText.text += "보조";
                break;
            case eAidType.Now:
                _ItemTypeText.text += "즉시";
                break;
            default:
                break;
        }

        _ItemText.text = _currSelectAid._Text;
    }

    public void StateMgrUI(int value)
    {
        StateMgrUIOff();
        Color TempColorB = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        switch (value)
        {
            case 0:
                _StateTabButton.GetComponent<Image>().color = TempColorB;
                _currScreen = _StateScreen;
                StateScreenStting();
                _StateScreen.SetActive(true);
                break;
            case 1:
                _EquipTabButton.GetComponent<Image>().color = TempColorB;
                _currScreen = _EquipScreen;
                EquipScreenSetting();
                _EquipScreen.SetActive(true);
                break;
            case 2:
                _ItemTabButton.GetComponent<Image>().color = TempColorB;
                _currScreen = _ItemScreen;
                ItemScreenSetting();
                _ItemScreen.SetActive(true);
                break;
            case 3:
                _CardTabButton.GetComponent<Image>().color = TempColorB;
                _currScreen = _CardScreen;
                AidScreenStting();
                _CardScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ItemScreenSetting()
    {
        if (_playerInfoCS._BoxItemList.Count != 0)
        {
            _currSelectItem = _playerInfoCS._BoxItemList[0];
            GameObject TempScreen = _currScreen;

            for (int i = 2; i < TempScreen.transform.childCount - 2; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(true);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(false);

            _ItemInfoImg = TempScreen.transform.GetChild(2).gameObject.GetComponent<Image>();
            _ItemName = TempScreen.transform.GetChild(3).gameObject.GetComponent<Text>();
            //_ItemTextName = TempScreen.transform.GetChild(4).gameObject.GetComponent<Text>();
            _ItemStateText = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
            _ItemTypeText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
            _ItemText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();

            _ItemInfoImg.sprite = _currSelectItem._sprite;
            _ItemName.text = _currSelectItem._NameKR;
            //_ItemTextName.text = _currSelectItem._Text;
            if (_currSelectItem._Restore != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Restore + " 회복";
            else if (_currSelectItem._Fight != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Fight + " 격투 보너스";
            else if (_currSelectItem._Dectective != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Dectective + " 추리 보정";
            else if (_currSelectItem._Move != 0) _ItemStateText.text = "옵션 : +" + _currSelectItem._Move + " 행동력 보너스";

            _ItemTypeText.text = "타입 : " + _currSelectItem._Type;
            _ItemText.text = _currSelectItem._Text;
        }
        else
        {
            GameObject TempScreen = _currScreen;

            for (int i = 2; i < TempScreen.transform.childCount - 1; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(false);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(true);
        }
    }
    public void EquipScreenSetting()
    {
        if (_playerInfoCS._BoxEquipList.Count != 0)
        {
            _currSelectEquip = _playerInfoCS._BoxEquipList[0];
            GameObject TempScreen = _currScreen;

            for (int i = 2; i < TempScreen.transform.childCount - 2; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(true);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(false);

            _ItemInfoImg = TempScreen.transform.GetChild(2).gameObject.GetComponent<Image>();
            _ItemName = TempScreen.transform.GetChild(3).gameObject.GetComponent<Text>();
            //_ItemTextName = TempScreen.transform.GetChild(4).gameObject.GetComponent<Text>();
            _ItemStateText = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
            _ItemStateBonusText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
            _ItemTypeText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();
            _ItemText = TempScreen.transform.GetChild(8).gameObject.GetComponent<Text>();

            _ItemInfoImg.sprite = _currSelectEquip._sprite;
            _ItemName.text = _currSelectEquip._NameKR;
            _ItemStateText.text = "스타일 : ";
            switch (_currSelectEquip._DuelType)
            {
                case eDuelType.S_InFighter:
                    _ItemStateText.text += "인파이터";
                    break;
                case eDuelType.R_OutFighter:
                    _ItemStateText.text += "아웃파이터";
                    break;
                case eDuelType.P_Grappler:
                    _ItemStateText.text += "그래플러";
                    break;
                case eDuelType.D_Defence:
                    _ItemStateText.text += "방어";
                    break;
                case eDuelType.R_Reasoning:
                    _ItemStateText.text += "통찰";
                    break;
                default:
                    break;
            }

            _ItemTypeText.text = "타입 : ";
            switch (_currSelectEquip._Type)
            {
                case eEquipType.Non:
                    break;
                case eEquipType.Fight:
                    _ItemTypeText.text += "격투";
                    break;
                case eEquipType.Dectective:
                    _ItemTypeText.text += "추리";
                    break;
                default:
                    break;
            }
            _ItemStateBonusText.text = "옵션 : ";
            switch (_currSelectEquip._Type)
            {
                case eEquipType.Non:
                    break;
                case eEquipType.Fight:
                    _ItemStateBonusText.text += "+" + _currSelectEquip._Fight + " 격투 보너스";
                    break;
                case eEquipType.Dectective:
                    _ItemStateBonusText.text += "+" + _currSelectEquip._Dectective + " 추리 보정";
                    break;
                default:
                    break;
            }
            if (_currSelectEquip._Fight < 0) _ItemStateBonusText.text = "옵션 : " + _currSelectEquip._Fight + " 피해 방어";

            _ItemText.text = _currSelectEquip._Text;
        }
        else
        {
            GameObject TempScreen = _currScreen;

            for (int i = 2; i < TempScreen.transform.childCount - 2; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(false);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(true);
        }
    }
    public void AidScreenStting()
    {
        if (_playerInfoCS._BoxAidList.Count != 0)
        {
            _currSelectAid = _playerInfoCS._BoxAidList[0];
            GameObject TempScreen = _currScreen;
            for (int i = 2; i < TempScreen.transform.childCount - 2; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(true);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(false);

            TempScreen.transform.GetChild(4).gameObject.SetActive(true);
            _ItemInfoImg = TempScreen.transform.GetChild(4).gameObject.GetComponent<Image>();
            if (_currSelectAid._isGet) _ItemInfoImg.color = Color.white;
            else _ItemInfoImg.color = Color.black;

            _ItemName = TempScreen.transform.GetChild(5).gameObject.GetComponent<Text>();
            _ItemStateText = TempScreen.transform.GetChild(6).gameObject.GetComponent<Text>();
            _ItemTypeText = TempScreen.transform.GetChild(7).gameObject.GetComponent<Text>();
            _ItemText = TempScreen.transform.GetChild(8).gameObject.GetComponent<Text>();

            _ItemIsGetText = TempScreen.transform.GetChild(9).gameObject.GetComponent<Text>(); ;
            _ItemContractText = TempScreen.transform.GetChild(10).gameObject.GetComponent<Text>(); ;
            _ItemPaymentText = TempScreen.transform.GetChild(11).gameObject.GetComponent<Text>(); ;
            _ItemCoolTimeText = TempScreen.transform.GetChild(12).gameObject.GetComponent<Text>(); ;

            _ItemIsGetText.text = _currSelectAid._isGet ? "계약완료" : "미 계약";
            _ItemContractText.text = _currSelectAid._Contract + "원";
            _ItemPaymentText.text = _currSelectAid._Payment + "원";
            if (!_currSelectAid._isGet) _ItemCoolTimeText.text = "의뢰 불가";
            else if (_currSelectAid._currCoolTime != 0) _ItemCoolTimeText.text = "대기중(" + _currSelectAid._currCoolTime + ")";
            else _ItemCoolTimeText.text = "의뢰 가능";

            _ItemInfoImg.sprite = _currSelectAid._sprite;
            _ItemName.text = _currSelectAid._NameKR;
            _ItemStateText.text = "옵션 : ";
            if (_currSelectAid._Token != 0) _ItemStateText.text += "+" + _currSelectAid._Token + " 단서 토큰";
            else if (_currSelectAid._Restore != 0) _ItemStateText.text += "+" + _currSelectAid._Restore + " 회복";
            else if (_currSelectAid._Money != 0) _ItemStateText.text += "+" + _currSelectAid._Money + " 원";
            else if (_currSelectAid._Fight != 0) _ItemStateText.text += "+" + _currSelectAid._Fight + " 결투 보너스";
            else if (_currSelectAid._Dectective != 0) _ItemStateText.text += "+" + _currSelectAid._Dectective + " 추리 보정";
            else if (_currSelectAid._Move != 0) _ItemStateText.text += "+" + _currSelectAid._Move + " 행동력 보너스";
            if (_currSelectAid._Fight < 0) _ItemStateText.text = "옵션 : " + _currSelectAid._Fight + " 피해 방어";

            _ItemTypeText.text = "처리 : ";

            switch (_currSelectAid._Type)
            {
                case eAidType.Non:
                    break;
                case eAidType.Buff:
                    _ItemTypeText.text += "보조";
                    break;
                case eAidType.Now:
                    _ItemTypeText.text += "즉시";
                    break;
                default:
                    break;
            }

            _ItemText.text = _currSelectAid._Text;
        }
        else
        {
            GameObject TempScreen = _currScreen;
            for (int i = 2; i < TempScreen.transform.childCount - 2; i++)
            {
                TempScreen.transform.GetChild(i).gameObject.SetActive(false);
            }
            TempScreen.transform.GetChild(TempScreen.transform.childCount - 1).gameObject.SetActive(true);
        }
    }
    public void StateScreenStting()
    {
        Sprite tempSp;
        if (_playerInfoCS._eNpcType == eNpcType.gangicon) tempSp = _ChrCgSprites[0];
        else tempSp = _ChrCgSprites[0];
        _currScreen.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = tempSp;

        if (_playerInfoCS._currUseAid._Codex != 0)
        {
            _StateScreen.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.SetActive(true);
            _StateScreen.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = _playerInfoCS._currUseAid._spriteTile;
            _StateScreen.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = _playerInfoCS._currUseAid._NameKR;
        }
        else
        {
            _StateScreen.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.SetActive(false);
            _StateScreen.transform.GetChild(4).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "조력자 없음";
        }

        if (_playerInfoCS._currUseEquipF._Codex != 0)
        {
            _StateScreen.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);
            _StateScreen.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = _playerInfoCS._currUseEquipF._sprite;
            _StateScreen.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = _playerInfoCS._currUseEquipF._NameKR;
        }
        else
        {
            _StateScreen.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(false);
            _StateScreen.transform.GetChild(2).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "보조장비 없음";
        }

        if (_playerInfoCS._currUseEquipD._Codex != 0)
        {
            _StateScreen.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(true);
            _StateScreen.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = _playerInfoCS._currUseEquipD._sprite;
            _StateScreen.transform.GetChild(3).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = _playerInfoCS._currUseEquipD._NameKR;
        }
        else
        {
            _StateScreen.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(false);
            _StateScreen.transform.GetChild(3).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "조사장비 없음";
        }

        GameObject TempStateInfo = _currScreen.transform.GetChild(5).gameObject;
        TempStateInfo.transform.GetChild(0).gameObject.GetComponent<Text>().text = "이름 : " + (_playerInfoCS._eNpcType == eNpcType.gangicon ? "강원진" : "박우주");
        TempStateInfo.transform.GetChild(1).gameObject.GetComponent<Text>().text = "직업 : " + (_playerInfoCS._eNpcType == eNpcType.gangicon ? "형사" : "경한기업 연구원");
        TempStateInfo.transform.GetChild(2).gameObject.GetComponent<Text>().text = "소지금 : " + _playerInfoCS._currMoney.ToString() + "원";
        TempStateInfo.transform.GetChild(3).gameObject.GetComponent<Text>().text = "사건 단서 : " + _playerInfoCS._clueTokenValue.ToString() + "개";
        TempStateInfo.transform.GetChild(4).gameObject.GetComponent<Text>().text = "체력 : " + _playerInfoCS._currHP.ToString() + " / " + _playerInfoCS._MaxHP.ToString();
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
