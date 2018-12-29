using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffCS : MonoBehaviour {
    public bool _isFadeStart;

    // 어둡기 조정
    private float _fadeOutValue;
    private float _fadeValue;

    private void Awake()
    {
        _isFadeStart = false;
    }

    public IEnumerator FadeIn(float timer)
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _fadeValue = 255.0f;
        _fadeOutValue = (255.0f / timer) * 0.1f;

        while (!_isFadeStart)
        {
            _fadeValue -= _fadeOutValue;
            this.GetComponent<Image>().color = new Color(0, 0, 0, _fadeValue / 255.0f);
            if (_fadeValue <= 1.0f) _isFadeStart = true;
            yield return new WaitForSeconds(0.1f);
        }

        _fadeValue = 0.0f;
        _isFadeStart = false;
        yield return null;
    }

    public IEnumerator FadeOut(float timer)
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _fadeValue = 0.0f;
        _fadeOutValue = (255.0f / timer) * 0.1f;

        while (!_isFadeStart)
        {
            _fadeValue += _fadeOutValue;
            this.GetComponent<Image>().color = new Color(0, 0, 0, _fadeValue / 255.0f);
            if (_fadeValue >= 255.0f) _isFadeStart = true;
            yield return new WaitForSeconds(0.1f);
        }

        _fadeValue = 255.0f;
        _isFadeStart = false;
        yield return null;
    }

    public void FadeInQuick()  { this.GetComponent<Image>().color = new Color(0, 0, 0, 1); }
    public void FadeOutQuick() { this.GetComponent<Image>().color = new Color(0, 0, 0, 0); }
}
