using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffCS : MonoBehaviour {

    public bool _isFlashStart;
    public float _currTime;

    private float _flashOutValue;
    private float _flashValue;

    private void Awake()
    {
        _isFlashStart = false;
    }

    public IEnumerator RedFlashIn(float timer)
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _flashValue = 255.0f;
        _flashOutValue = (255.0f / timer) * 0.1f;

        while (!_isFlashStart)
        {
            _flashValue -= _flashOutValue;
            this.GetComponent<Image>().color = new Color(_flashValue / 255.0f, 0, 0, _flashValue / 255.0f);
            if (_flashValue <= 0.0f) _isFlashStart = true;
            yield return new WaitForSeconds(0.1f);
        }
        _flashValue = 0.0f;
        _isFlashStart = false;
        yield return null;
    }

    public IEnumerator WhiteFlashIn(float timer)
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _flashValue = 255.0f;
        _flashOutValue = (255.0f / timer) * 0.1f;

        while (!_isFlashStart)
        {
            _flashValue -= _flashOutValue;
            this.GetComponent<Image>().color = new Color(1, 1, 1, _flashValue / 255.0f);
            if (_flashValue <= 0.0f) _isFlashStart = true;
            yield return new WaitForSeconds(0.1f);
        }
        _flashValue = 0.0f;
        _isFlashStart = false;
        yield return null;
    }

}
