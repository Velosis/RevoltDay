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
            this.GetComponent<TalkIndexCS>()._L_imgIndex.Add((string)date[i]["L_chrImg_Index"]);
            this.GetComponent<TalkIndexCS>()._R_imgIndex.Add((string)date[i]["R_chrImg_Index"]);
            this.GetComponent<TalkIndexCS>()._talkName.Add((string)date[i]["talk_name_Index"]);
            this.GetComponent<TalkIndexCS>()._CGimgIndex.Add((string)date[i]["CGImg_Index"]);
            this.GetComponent<TalkIndexCS>()._textIndex.Add((string)date[i]["Talk_Index"]);
            this.GetComponent<TalkIndexCS>()._SoundIndex.Add((string)date[i]["soundEff_Index"]);
            this.GetComponent<TalkIndexCS>()._BgmIndex.Add((string)date[i]["BGM_Index"]);
            this.GetComponent<TalkIndexCS>()._ShakingEffIndex.Add((int)date[i]["ShakingEff_Index"]);
            this.GetComponent<TalkIndexCS>()._Wait.Add((float)date[i]["wait_Index"] - 0.1f);
            this.GetComponent<TalkIndexCS>()._fadeIn.Add((int)date[i]["fadeIn_Index"]);
            this.GetComponent<TalkIndexCS>()._fadeOut.Add((int)date[i]["fadeOut_Index"]);
            if ((string)date[i]["fadeIn_quick_Index"] == "TRUE") this.GetComponent<TalkIndexCS>()._fadeQIn.Add(true);
            else this.GetComponent<TalkIndexCS>()._fadeQIn.Add(false);

            if ((string)date[i]["fadeOut_quick_Index"] == "TRUE") this.GetComponent<TalkIndexCS>()._fadeQOut.Add(true);
            else this.GetComponent<TalkIndexCS>()._fadeQOut.Add(false);

            if ((string)date[i]["RedFlash_Index"] == "TRUE") this.GetComponent<TalkIndexCS>()._RedFlashIndex.Add(true);
            else this.GetComponent<TalkIndexCS>()._RedFlashIndex.Add(false);

            if ((string)date[i]["whiteFlash_Index"] == "TRUE") this.GetComponent<TalkIndexCS>()._WhiteFlashIndex.Add(true);
            else this.GetComponent<TalkIndexCS>()._WhiteFlashIndex.Add(false);
        }

        this.GetComponent<TalkIndexCS>().startTalk();
    }
}
