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

    private void Awake()
    {
        object[] tempResList = null;
        tempResList = Resources.LoadAll("1.ChrImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            GameObject tempGO = tempResList[i] as GameObject;
            _imgBox.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll("3.SoundList");
        for (int i = 0; i < tempResList.Length; i++)
        {
            AudioClip tempGO = tempResList[i] as AudioClip;
            _SoundBox.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll("4.BgmList");
        for (int i = 0; i < tempResList.Length; i++)
        {
            AudioClip tempGO = tempResList[i] as AudioClip;
            _BgmBox.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll("5.CGImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            GameObject tempGO = tempResList[i] as GameObject;
            _CGImg.Add(tempGO.name, tempGO);
        }

        tempResList = null;
        tempResList = Resources.LoadAll<Sprite>("6.IconImg");
        for (int i = 0; i < tempResList.Length; i++)
        {
            Sprite tempGO = tempResList[i] as Sprite;
            _IconImg.Add(tempGO.name, tempGO);
        }
        Debug.Log("불러오기 종료");

    }
}