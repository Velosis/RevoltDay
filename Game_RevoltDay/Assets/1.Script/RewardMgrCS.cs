using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct sRewardBox
{
    public ItemData _itemData;
    public EquipData _equipData;
    public AidData _aidData;
}

public class RewardMgrCS : MonoBehaviour {
    public GameObject _RewardBoxGO;
    public GameObject[] _RewardBoxGOArr = new GameObject[4];

    public List<sRewardBox> sRewardBoxes;

    private void Awake()
    {
        _RewardBoxGO.SetActive(false);

        for (int i = 0; i < _RewardBoxGOArr.Length; i++)
        {
            _RewardBoxGOArr[i] = Instantiate(_RewardBoxGO, transform.GetChild(1).gameObject.transform);
        }
        Destroy(_RewardBoxGO);

        SettingRewardBox(2);
    }

    public void SettingRewardBox(int value)
    {
        float tempSizeW = _RewardBoxGOArr[0].GetComponent<RectTransform>().rect.width + 50.0f;
        tempSizeW *= value;
        for (int i = 0; i < value; i++)
        {
            _RewardBoxGOArr[i].transform.Translate(-tempSizeW / 2.0f * i, 0, 0);
            _RewardBoxGOArr[i].transform.Translate((tempSizeW / value) * i,0,0);
        }
    }
}
