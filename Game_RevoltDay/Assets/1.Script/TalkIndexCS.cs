using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkIndexCS : MonoBehaviour {
    public string _sceneID; // 신 ID

    // 대화 옵션
    private bool _TalkSkip;
    private bool _TalkCut;

    // 페이드 전환 정보
    public List<string> _fadeIn = new List<string>();
    public List<string> _fadeOut = new List<string>();
    public List<bool> _fadeQIn = new List<bool>();
    public List<bool> _fadeQOut = new List<bool>();


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
    private WaitForSeconds _textDaleyWait;
    public float _nextTextDaley; // 스크립트가 출력되는 속도
    private WaitForSeconds _nextTextDaleyWait;

    private FadeEffCS _fadeEffCS;

    private void Awake()
    {
        _Se = GameObject.Find("SoundMgr").GetComponent<AudioSource>();
        _Bgm = GameObject.Find("BgmMgr").GetComponent<AudioSource>();
        _fadeEffCS = GameObject.Find("Fade_Eff").GetComponent<FadeEffCS>();

        L_chacterImg = GameObject.Find("L_ChacterImg");
        R_chacterImg = GameObject.Find("R_ChacterImg");
        _textBox = GameObject.Find("TalkText").GetComponent<Text>();
        _talkNameBox = GameObject.Find("TalkName").GetComponent<Text>();

        _textDaleyWait = new WaitForSeconds(_textDaley);
        _nextTextDaleyWait = new WaitForSeconds(_nextTextDaley);
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
            fadeSys(s);
            fadeQSys(s);

            _currText = ""; // 기존 문구 초기화

            if (fullText[s] == "null")
            {
                _textBox.text = _currText;
                yield return _nextTextDaleyWait;
                continue;
            }

            // 문장 출력 코드
            #region
            for (int i = 0; i < fullText[s].Length; i++) // for를 통해 한글자씩 출력
            {
                if (_TalkSkip) { s = _textIndex.Count - 1; }
                if (_TalkCut) { i = fullText[s].Length - 1; }
                _currText = fullText[s].Substring(0, i + 1); // 글자 길이 늘려서 출력해주기
                _textBox.text = _currText; // 현재 스크립트가 속한 텍스트에 출력(고려)
                yield return _textDaleyWait; // 코루틴을 통해 딜레이 작동
            }
            #endregion

            _TalkSkip = false;
            _TalkCut = false;
            yield return _nextTextDaleyWait;

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
        if (_SoundIndex[talkValue] == "null") return;

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

    public void fadeQSys(int talkValue)
    {
        if (!_fadeQIn[talkValue] &&
            !_fadeQOut[talkValue]) return;

        if (_fadeEffCS._isFadeStart)
        {
            _fadeEffCS._isFadeStart = false;
            Debug.Log("fadeQSys : fadeSys이 실행 중임으로 종료하고 fadeQSys을 실행 합니다.");
        }

        if (_fadeQIn[talkValue]) { _fadeEffCS.FadeInQuick(); }
        if (_fadeQOut[talkValue]) { _fadeEffCS.FadeOutQuick(); }
    }

    public void fadeSys(int talkValue)
    {
        if (_fadeIn[talkValue] == "null" &&
            _fadeOut[talkValue] == "null") return;

        if (_fadeEffCS._isFadeStart)
        {
            Debug.Log("fadeSys : 이미 실행 중 입니다.");
            return;
        }

        if (_fadeIn[talkValue] != "null")
        {
            switch (_fadeIn[talkValue])
            {
                case "fadeIn_1s":
                    StartCoroutine(_fadeEffCS.FadeIn(1.0f));
                    break;
                case "fadeIn_2s":
                    StartCoroutine(_fadeEffCS.FadeIn(2.0f));
                    break;
                case "fadeIn_3s":
                    StartCoroutine(_fadeEffCS.FadeIn(3.0f));
                    break;
                default:
                    Debug.Log(talkValue+ "줄_" + "fadeInSys : 정의되지 않은 명령어 입니다.");
                    return;
            }
        }

        if (_fadeOut[talkValue] != "null")
        {
            switch (_fadeOut[talkValue])
            {
                case "fadeOut_1s":
                    StartCoroutine(_fadeEffCS.FadeIn(1.0f));
                    break;
                case "fadeOut_2s":
                    StartCoroutine(_fadeEffCS.FadeIn(2.0f));
                    break;
                case "fadeOut_3s":
                    StartCoroutine(_fadeEffCS.FadeIn(3.0f));
                    break;
                default:
                    Debug.Log(talkValue + "줄_" + "fadeOutSys : 정의되지 않은 명령어 입니다.");
                    return;
            }
        }
    }
}

