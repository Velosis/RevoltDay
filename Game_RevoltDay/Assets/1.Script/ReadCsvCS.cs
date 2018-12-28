using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCsvCS : MonoBehaviour {

    void Start()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/" + name);

        for (var i = 0; i < date.Count; i++)
        {
            this.GetComponent<TalkIndexCS>()._textIndex.Add((string)date[i]["Talk_Index"]);

            //Debug.Log("InDex " + (i).ToString() + " : " + date[i]["chrImg_Index"] + "  " + date[i]["CGImg_Index"] + "  " + date[i]["Talk_Index"] + "  " + date[i]["eff_Index"] + "  " + date[i]["soundEff_Index"]);
        }
    }
}
