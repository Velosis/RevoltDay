using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroMgrCS : MonoBehaviour {
    public GameObject _videoIntro;
    public GameObject _TileUI;
    public bool _isPlay = false;

    private void LateUpdate()
    {
        if (!_isPlay && _videoIntro.GetComponent<VideoPlayer>().isPlaying) _isPlay = true;
        if (_isPlay && !_videoIntro.GetComponent<VideoPlayer>().isPlaying) _TileUI.SetActive(true);
    }

    public void videoEnd()
    {
        _videoIntro.GetComponent<VideoPlayer>().Stop();
    }
}