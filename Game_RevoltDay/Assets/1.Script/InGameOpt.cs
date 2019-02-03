using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameOpt : MonoBehaviour {
    public AudioSource _audioSource;

    private void OnEnable()
    {
        transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)OptionMgrCS.getOptionInfo()._SeValue;
        transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = (float)OptionMgrCS.getOptionInfo()._BgmValue;
    }

    private void LateUpdate()
    {
        if (_audioSource && transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value != _audioSource.volume)
        {
            _audioSource.volume = transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value;
        }
    }

    private void OnDisable()
    {
        OptionInfo optionInfo = new OptionInfo();
        optionInfo._SeValue = transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value;
        optionInfo._BgmValue = transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value; 

        OptionMgrCS.OptionSave(optionInfo);
    }
}
