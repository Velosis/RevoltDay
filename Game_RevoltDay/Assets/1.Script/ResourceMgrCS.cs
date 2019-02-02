using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ResourceMgrCS : MonoBehaviour {
    static public Dictionary<string, GameObject> _imgBox = new Dictionary<string, GameObject>();
    static public Dictionary<string, GameObject> _CGImg = new Dictionary<string, GameObject>();
    static public Dictionary<string, AudioClip> _SoundBox = new Dictionary<string, AudioClip>();
    static public Dictionary<string, AudioClip> _BgmBox = new Dictionary<string, AudioClip>();
    static public Dictionary<string, Sprite> _IconImg = new Dictionary<string, Sprite>();


    public GameObject _ShopMgrGO;

    // 아이템 생성
    static public ItemData SettingItemData(ItemData _itemData)
    {
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
        TempItemData._TurnOtp = _itemData._TurnOtp;
        TempItemData._currTurnOtp = _itemData._TurnOtp;
        TempItemData._Type = _itemData._Type;

        return TempItemData;
    }
    static public EquipData SettingEquipData(EquipData _equipData)
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
    static public AidData SettingAidData(AidData _AidData)
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

    private void Awake()
    {


        object[] tempResList = null;
        tempResList = Resources.LoadAll("1.ChrImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            GameObject tempGO = tempResList[i] as GameObject;
            if (!_imgBox.ContainsKey(tempGO.name)) _imgBox.Add(tempGO.name, tempGO);

        }

        tempResList = null;
        tempResList = Resources.LoadAll("3.SoundList");
        for (int i = 0; i < tempResList.Length; i++)
        {
            AudioClip tempGO = tempResList[i] as AudioClip;
            if (!_SoundBox.ContainsKey(tempGO.name)) _SoundBox.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll("4.BgmList");
        for (int i = 0; i < tempResList.Length; i++)
        {
            AudioClip tempGO = tempResList[i] as AudioClip;
            if (!_BgmBox.ContainsKey(tempGO.name)) _BgmBox.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll("5.CGImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            GameObject tempGO = tempResList[i] as GameObject;
            if (!_CGImg.ContainsKey(tempGO.name)) _CGImg.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll<Sprite>("6.IconImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            Sprite tempGO = tempResList[i] as Sprite;
            if (!_IconImg.ContainsKey(tempGO.name)) _IconImg.Add(tempGO.name, tempGO);
        }

        _ShopMgrGO.SetActive(true);
        _ShopMgrGO.SetActive(false);
        Debug.Log("리소스 불러오기 종료");

    }
}