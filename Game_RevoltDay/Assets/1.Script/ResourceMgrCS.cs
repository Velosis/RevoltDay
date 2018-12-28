using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceMgrCS : MonoBehaviour {
    static public Dictionary<string, GameObject> _imgBox = new Dictionary<string, GameObject>();
    static public Dictionary<string, AudioClip> _SoundBox = new Dictionary<string, AudioClip>();
    static public Dictionary<string, AudioClip> _BgmBox = new Dictionary<string, AudioClip>();

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
    }
}