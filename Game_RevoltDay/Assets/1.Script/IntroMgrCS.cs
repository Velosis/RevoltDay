using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroMgrCS : MonoBehaviour {
    public GameObject _videoIntro;
    public GameObject _TileUI;
    public bool _isPlay = false;

    private void OnEnable()
    {
        if (GameObject.Find("GameOver") || GameObject.Find("UIMgr") || GameObject.Find("DonTileUI"))
        {
            videoEnd();
            return;
        }

        _videoIntro.GetComponent<VideoPlayer>().Play();
        if (!_TileUI.GetComponent<SceneMgr>()._fristVideo && !_isPlay && _videoIntro.GetComponent<VideoPlayer>().isPlaying)
        {
            
            _isPlay = true;
        }
    }

    private void LateUpdate()
    {
        Invoke("isVideoCheck", 0.1f);
    }

    public void isVideoCheck()
    {
        if (_isPlay && !_videoIntro.GetComponent<VideoPlayer>().isPlaying) _TileUI.SetActive(true);
    }

    public void videoEnd()
    {
        _TileUI.SetActive(true);
        _videoIntro.GetComponent<VideoPlayer>().Stop();
    }
}