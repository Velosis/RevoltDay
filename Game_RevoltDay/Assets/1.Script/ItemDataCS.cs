using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataCS : MonoBehaviour {
    public ItemData _currItemData = null;
    public EquipData _currEquipData = null;
    public AidData _currAidData = null;
    public ShopMgr _shopMgrCS;
    public StateMgrCS _stateMgrCS;
    public int _arrIdex = 0;

    private PlayerInfoCS _playerInfoCS;
    private Color _SelectState;
    private Color _NotSelectState;

    public bool _isSelect;

    private void Awake()
    {
        _playerInfoCS = _stateMgrCS._playerInfoCS;

        _SelectState = new Color(0, 1, 1, 1);
        _NotSelectState = new Color(1, 1, 1, 1);
    }

    private void OnEnable()
    {
        StateMgrCS._isItemSelect += SelectState;
    }

    private void OnDisable()
    {
       StateMgrCS._isItemSelect -= SelectState;
    }

    public void isSelect(bool _is)
    {
        _isSelect = _is;
    }

    // 선택 시 상호작용
    public void TextSetting() {

        if (_currItemData._Codex != 0)
        {
            if (_isSelect)
            {
                if (_currItemData._Nomalprice > _shopMgrCS._playerInfoCS._currMoney) return;

                _shopMgrCS.SetBuyItem(_currItemData);
                _shopMgrCS.BuyPopupSys(true);
                return;
            }

            _shopMgrCS.isItemSeletSys(false);
            _shopMgrCS.itemBottomSetting(_currItemData);
        }
        else if (_currEquipData._Codex != 0)
        {
            if (_isSelect)
            {
                if (_currEquipData._Nomalprice > _shopMgrCS._playerInfoCS._currMoney) return;

                _shopMgrCS.SetBuyEquip(_currEquipData);
                _shopMgrCS.BuyPopupSys(true);
                return;
            }

            _shopMgrCS.isItemSeletSys(false);
            _shopMgrCS.EquipBottomSetting(_currEquipData);
        }else if (_currAidData._Codex != 0)
        {
            // 상점 전용 선택
        }

        _isSelect = true; 
    }

    public void StateSetting() {

        if (_isSelect)
        {
            if (_currItemData._Codex != 0) UseItemSys();
            else if (_currEquipData._Codex != 0) UseEquipSys();
            else if (_currAidData._Codex != 0) UseAidSys();

            return;
        }

        _stateMgrCS.allNotSelect();
        SelectState(true);

        if (_currItemData._Codex != 0) _stateMgrCS.ItemUiSetting(_currItemData);
        else if (_currEquipData._Codex != 0) _stateMgrCS.EquipUiSetting(_currEquipData);
        else if (_currAidData._Codex != 0) _stateMgrCS.AidUiSetting(_currAidData);

        _isSelect = true;
    }

    public void UseItemSys()
    {
        switch (_currItemData._Type)
        {
            case eItemType.Non:
                break;
            case eItemType.Healing:
                if (!_playerInfoCS.setActUseItem(_currItemData)) return;
                Destroy(gameObject);
                _playerInfoCS.UseItem(_arrIdex);
                _stateMgrCS.BoxItemListSetting(_stateMgrCS._currScreen);
                break;
            case eItemType.Buff:
                if (!_playerInfoCS.setActUseItem(_currItemData)) return;
                Destroy(gameObject);
                _playerInfoCS.UseItem(_arrIdex);
                _stateMgrCS.BoxItemListSetting(_stateMgrCS._currScreen);
                break;
            case eItemType.Move:
                Debug.Log("하늘철 열차 사용");
                //_playerInfoCS._currUseItemList.Add(ResourceMgrCS.SettingItemData(_currItemData));
                //Destroy(gameObject);
                //_playerInfoCS.UseItem(_arrIdex);
                //_stateMgrCS.BoxItemListSetting(_stateMgrCS._currScreen);
                break;
            default:
                break;
        }
    }

    public void UseEquipSys()
    {
        // 장착 기능
        if (!_currEquipData._isSet)
        {
            Debug.Log("장착 시도");
            _stateMgrCS.IsUseEquip(true);
            return;
        }

        // 해체 기능
        if (_currEquipData._isSet)
        {
            Debug.Log("해체 시도");

            //_stateMgrCS.IsUseEquip(true);
            return;
        }
    }

    public void UseAidSys()
    {
        // 계약 기능
        if (!_currAidData._isGet)
        {
            _stateMgrCS.IsUseAIdPupupUI(true);
            return;
        }

        // 장착 기능
        if (_currAidData._isGet && _playerInfoCS._currUseAid._Codex == 0)
        {
            _stateMgrCS.UseAidSys(true);
            return;
        }else if (_currAidData._isGet && _playerInfoCS._currUseAid._Codex != 0)
        {
            StartCoroutine(_stateMgrCS.NotUseAid(eAidType.Non));
        }
    }

    public void SelectState(bool _is)
    {
        if (_is)
        {
            GetComponent<Image>().color = _SelectState;
        }
        else
        {
            _isSelect = false;
            GetComponent<Image>().color = _NotSelectState;
        }
    }

}
