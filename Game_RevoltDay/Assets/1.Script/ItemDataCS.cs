using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataCS : MonoBehaviour {
    public ItemData _currItemData;
    public ShopMgr _shopMgrCS;

    public bool _isSelect;

    public void isSelect(bool _is)
    {
        _isSelect = _is;
    }

    public void TextSetting() {

        if (_isSelect)
        {
            if (_currItemData._Nomalprice > _shopMgrCS._playerInfoCS._currMoney) return;

            _shopMgrCS.SetBuyItem(_currItemData);
            _shopMgrCS.BuyPopupSys(true);
            return;
        }

        _shopMgrCS.isItemSeletSys(false);
        _shopMgrCS.itemBottomSetting(_currItemData);
        _isSelect = true; 
    }
}
