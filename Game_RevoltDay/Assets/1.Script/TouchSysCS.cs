using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTouchState
{
    UP,
    DOWN,
    MOVED
}

public class TouchSysCS : MonoBehaviour {

    static public Touch _touchValue;
    static public Vector2 _touchPos;
    static public eTouchState _currTouchState;
    public bool _touch_TEST;

    private void Update()
    {
        if (Input.touchCount == 0) return;

        _touchValue = Input.GetTouch(0);
        if (_touchValue.phase == TouchPhase.Began) { touchDown();}
        else if (_touchValue.phase == TouchPhase.Ended) { touchUp(); }
        else if (_touchValue.phase == TouchPhase.Moved) { touchMoved(); }

        _touchPos = touchPos();
    }

    public void touchDown()
    {
        _currTouchState = eTouchState.DOWN;
        if (_touch_TEST) Debug.Log("눌림");
    }

    public void touchUp()
    {
        _currTouchState = eTouchState.UP;
        if (_touch_TEST) Debug.Log("떼었음");
    }

    public void touchMoved()
    {
        _currTouchState = eTouchState.MOVED;
        if (_touch_TEST) Debug.Log("움직임");
    }

    public Vector2 touchPos()
    {
        if (_touch_TEST) Debug.Log("_touchValue.position : " + _touchValue.position.ToString());
        return _touchValue.position;
    }
}
