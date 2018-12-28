using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCsvCS : MonoBehaviour {

    private void Start()
    {
        startDateLoad();
    }

    void startDateLoad()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/" + name);

        for (var i = 0; i < date.Count; i++)
        {
            this.GetComponent<TalkIndexCS>()._textIndex.Add((string)date[i]["Talk_Index"]);
            this.GetComponent<TalkIndexCS>()._L_imgIndex.Add((string)date[i]["L_chrImg_Index"]);
            this.GetComponent<TalkIndexCS>()._R_imgIndex.Add((string)date[i]["R_chrImg_Index"]);
            this.GetComponent<TalkIndexCS>()._talkName.Add((string)date[i]["talk_name_Index"]);
            this.GetComponent<TalkIndexCS>()._SoundIndex.Add((string)date[i]["soundEff_Index"]);
            this.GetComponent<TalkIndexCS>()._BgmIndex.Add((string)date[i]["BGM_Index"]);
        }

        this.GetComponent<TalkIndexCS>().startTalk();
    }
}
