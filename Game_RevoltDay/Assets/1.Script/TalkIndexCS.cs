using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkIndexCS : MonoBehaviour {
    public string _sceneID; // 신 ID

    // 출력되는 이미지 정보
    public string[] _imageIndex;

    // 출력되는 대화 정보
    public List<string> _textIndex = new List<string>(); // 입력한 대화 스크립트

    //public string[] _textIndex; // 입력한 대화 스크립트
    private string _currText; // 현재 출력되는 스크립트 내용

    private Text _textBox;

    // 출력 관련
    private List<string> _fullText;
    public float _textDaley; // 스크립트가 출력되는 속도

    private void Awake()
    {
        _textBox = GameObject.Find("TalkText").GetComponent<Text>();
    }

    private void Start()
    {
        // 이미지 위치 셋팅
        //_lChrImg.GetComponent<RectTransform>().Translate(_lChrImg.GetComponent<RectTransform>().rect.width / 2.0f, _lChrImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);
        //_rChrImg.GetComponent<RectTransform>().Translate(-_rChrImg.GetComponent<RectTransform>().rect.width / 2.0f, _rChrImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);

        _fullText = _textIndex;
        StartCoroutine(ShowText(_fullText));
    }

    public IEnumerator ShowText(List<string> fullText)
    {
        for (int s = 0; s < _textIndex.Count; s++)
        {
            if (fullText[s] == "null")
            {
                yield return new WaitForSeconds(_textDaley);
                continue;
            }

            _currText = ""; // 기존 문구 초기화

            // 문장 출력 코드
            #region
            for (int i = 0; i < fullText[s].Length; i++) // for를 통해 한글자씩 출력
            {
                _currText = fullText[s].Substring(0, i + 1); // 글자 길이 늘려서 출력해주기
                _textBox.text = _currText; // 현재 스크립트가 속한 텍스트에 출력(고려)
                yield return new WaitForSeconds(_textDaley); // 코루틴을 통해 딜레이 작동
            }
            #endregion
        }
    }

}

