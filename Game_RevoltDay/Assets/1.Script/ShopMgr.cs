using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData
{
    public string _itemName = "";
    public string _itemContent = "";
    public int _buyValue = 0;
}

public class ShopMgr : MonoBehaviour {
    public List<ItemData> itemDatas = new List<ItemData>();

    private void Start()
    {
    }

}
