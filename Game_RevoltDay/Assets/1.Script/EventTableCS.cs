using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTableCS : MonoBehaviour {
    public TalkIndexCS _TalkSceneID;

    public bool _isEventStart;

    public void EventStart()
    {
        _isEventStart = true;
    }
}
