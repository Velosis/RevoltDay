using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct NewsData
{
    public int _index;
    public string _Text;
}

public class NewsMgrCS : MonoBehaviour {

    public GameObject _TextGO;
    public List<NewsData> _newsDatas = new List<NewsData>();
    public float _moveSpeed;

    private void Awake()
    {
        NewsTableLoad();
        StartNews();
    }

    public void NewsTableLoad()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/NewsTable");

        for (int i = 0; i < date.Count; i++)
        {
            NewsData TempNewsData = new NewsData();

            TempNewsData._index = (int)date[i]["radio_Index"];
            TempNewsData._Text = (string)date[i]["text"];

            _newsDatas.Add(TempNewsData);
        }
    }


    public void StartNews()
    {
        string tempText = _newsDatas[Random.Range(0, _newsDatas.Count)]._Text;
        _TextGO.GetComponent<RectTransform>().sizeDelta = new Vector2(tempText.Length * _TextGO.GetComponent<Text>().fontSize, 50);
        _TextGO.GetComponent<RectTransform>().localPosition = new Vector2(_TextGO.GetComponent<RectTransform>().sizeDelta.x, 0);
        _TextGO.GetComponent<Text>().text = tempText;
        StartCoroutine(TextMove());
    }

    public void ResetNewsSys()
    {
        StopAllCoroutines();
        StartNews();
    }

    public IEnumerator TextMove()
    {
        while (_TextGO.GetComponent<RectTransform>().localPosition.x > -(_TextGO.GetComponent<RectTransform>().sizeDelta.x))
        {
            _TextGO.GetComponent<RectTransform>().Translate(-_moveSpeed, 0, 0);
            yield return null;
        }
        ResetNewsSys();

    }
}
