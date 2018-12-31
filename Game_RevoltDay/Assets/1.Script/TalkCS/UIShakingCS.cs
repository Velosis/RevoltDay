using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShakingCS : MonoBehaviour {

    public bool _isShaking;
    private float _currTime;
    private float _OringTime;
    private RectTransform _OringShakingRt;
    private Vector3 _ShakingVecPos;

    private void Awake()
    {
        _OringShakingRt = GameObject.Find("TalkMainScreen").GetComponent<RectTransform>();
        _isShaking = false;
    }

    public IEnumerator ShakingOn(float timer)
    {
        if (_isShaking) yield return null;

        _OringTime = timer;
        _currTime = 0.0f;
        _isShaking = true;
        _ShakingVecPos = _OringShakingRt.position;
        while (_isShaking)
        {
            _OringShakingRt.position = _ShakingVecPos;
            _OringShakingRt.Translate(Random.insideUnitSphere.x * 50.0f, 0, 0);
            _currTime += Time.deltaTime;

            yield return null;

            if (_currTime >= _OringTime) _isShaking = false;
        }
        _OringShakingRt.position = _ShakingVecPos;
        _isShaking = false;
    }

}
