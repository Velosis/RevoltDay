using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffMgrCS : MonoBehaviour {

    public GameObject _image;

    public void startEff()
    {
        StartCoroutine(EffSys());
    }

    IEnumerator EffSys()
    {
        _image.SetActive(true);
        float time = 0.0f;
        while (time <= 1.0f)
        {
            time += Time.deltaTime;

            _image.GetComponent<Image>().color = new Color(0,0,0, 1.0f - time);
            yield return null;
        }

        _image.SetActive(false);

    }
}
