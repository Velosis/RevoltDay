using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkIndexCS : MonoBehaviour {
    public string _sceneID; // 신 ID

    // 대화 옵션
    private bool _TalkSkip;
    private bool _TalkCut;

    // 출력되는 사운드 정보
    public List<string> _SoundIndex = new List<string>();
    public List<string> _BgmIndex = new List<string>();
    private AudioSource _Se;
    private AudioSource _Bgm;

    // 출력되는 이미지 정보
    public List<string> _L_imgIndex = new List<string>();
    public List<string> _R_imgIndex = new List<string>();

    private GameObject L_chacterImg;
    private GameObject R_chacterImg;

    // 출력되는 대화 정보
    public List<string> _talkName = new List<string>();
    public List<string> _textIndex = new List<string>(); // 입력한 대화 스크립트

    //public string[] _textIndex; // 입력한 대화 스크립트
    private string _currText; // 현재 출력되는 스크립트 내용

    private Text _textBox;
    private Text _talkNameBox;

    // 출력 관련
    private List<string> _fullText;
    public float _textDaley; // 스크립트가 출력되는 속도

    private void Awake()
    {
        _Se = GameObject.Find("SoundMgr").GetComponent<AudioSource>();
        _Bgm = GameObject.Find("BgmMgr").GetComponent<AudioSource>();

        L_chacterImg = GameObject.Find("L_ChacterImg");
        R_chacterImg = GameObject.Find("R_ChacterImg");
        _textBox = GameObject.Find("TalkText").GetComponent<Text>();
        _talkNameBox = GameObject.Find("TalkName").GetComponent<Text>();
    }

    public void isSkipInput() { _TalkSkip = true; }
    public void isCutInput() { _TalkCut = true; }

    public void startTalk()
    {
        _TalkSkip = false;
        _TalkCut = false;
        _fullText = _textIndex;
        StartCoroutine(ShowText(_fullText));
    }

    public IEnumerator ShowText(List<string> fullText)
    {
        for (int s = 0; s < _textIndex.Count; s++)
        {
            _talkNameBox.text = _talkName[s];
            imageSetting(s);
            soundPlay(s);
            bgmPlay(s);

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
                if (_TalkSkip) { s = _textIndex.Count - 1; }
                if (_TalkCut) { i = fullText[s].Length - 1; }
                _currText = fullText[s].Substring(0, i + 1); // 글자 길이 늘려서 출력해주기
                _textBox.text = _currText; // 현재 스크립트가 속한 텍스트에 출력(고려)
                yield return new WaitForSeconds(_textDaley); // 코루틴을 통해 딜레이 작동
            }
            #endregion

            _TalkSkip = false;
            _TalkCut = false;
        }

        Debug.Log("대화 종료");
    }

    public void imageSetting(int talkValue)
    {
        GameObject tempImg = null;

        ResourceMgrCS._imgBox.TryGetValue(_L_imgIndex[talkValue], out tempImg);
        L_chacterImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        L_chacterImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta;

        //L_chacterImg.GetComponent<RectTransform>().Translate(
        //    L_chacterImg.GetComponent<RectTransform>().rect.width / 2.0f, L_chacterImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);

        ResourceMgrCS._imgBox.TryGetValue(_R_imgIndex[talkValue], out tempImg);
        R_chacterImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        R_chacterImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta;

        //R_chacterImg.GetComponent<RectTransform>().Translate(
        //    -R_chacterImg.GetComponent<RectTransform>().rect.width / 2.0f, R_chacterImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);
    }
    
    public void soundPlay(int talkValue)
    {
        AudioClip tempAudio = null;
        ResourceMgrCS._SoundBox.TryGetValue(_SoundIndex[talkValue], out tempAudio);

        _Se.clip = tempAudio;
        _Se.loop = false;
        _Se.volume = 1.0f; // 0.0f ~ 1.0f == 0% ~ 100%
        _Se.Play();
    }

    public void bgmPlay(int talkValue)
    {
        if (_BgmIndex[talkValue] == "null") return;

        AudioClip tempAudio = null;
        ResourceMgrCS._BgmBox.TryGetValue(_BgmIndex[talkValue], out tempAudio);

        _Bgm.clip = tempAudio;
        _Bgm.loop = false;
        _Bgm.volume = 1.0f; // 0.0f ~ 1.0f == 0% ~ 100%
        _Bgm.Play();
    }
}

