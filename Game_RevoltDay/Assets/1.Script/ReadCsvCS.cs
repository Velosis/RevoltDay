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
            this.GetComponent<TalkIndexCS>()._fadeIn.Add((string)date[i]["fadeIn_Index"]);
            this.GetComponent<TalkIndexCS>()._fadeOut.Add((string)date[i]["fadeOut_Index"]);
            if ((int)date[i]["fadeIn_quick_Index"] == 1)
            {
                this.GetComponent<TalkIndexCS>()._fadeQIn.Add(true);
            }
            else
            {
                this.GetComponent<TalkIndexCS>()._fadeQIn.Add(false);
            }

            if ((int)date[i]["fadeOut_quick_Index"] == 1)
            {
                this.GetComponent<TalkIndexCS>()._fadeQOut.Add(true);
            }
            else
            {
                this.GetComponent<TalkIndexCS>()._fadeQOut.Add(false);
            }

            //this.GetComponent<TalkIndexCS>()._fadeQIn.Add((int)date[i]["fadeIn_quick_Index"]);
            //this.GetComponent<TalkIndexCS>()._fadeQOut.Add((int)date[i]["fadeOut_quick_Index"]);
        }

        this.GetComponent<TalkIndexCS>().startTalk();
    }
}
