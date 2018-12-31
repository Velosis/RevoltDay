using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkIndexCS : MonoBehaviour {
    public string _sceneID; // 신 ID

    // 대화 옵션
    private bool _TalkSkip;
    private bool _TalkCut;

    // 각종 이펙트 정보
    public List<int> _ShakingEffIndex = new List<int>();
    private UIShakingCS _ShakingCS;

    // 섬광 이펙트 정보
    public List<bool> _RedFlashIndex = new List<bool>();
    public List<bool> _WhiteFlashIndex = new List<bool>();

    // 대화 대기 정보
    public List<float> _Wait = new List<float>();

    // 페이드 전환 정보
    public List<int> _fadeIn = new List<int>();
    public List<int> _fadeOut = new List<int>();
    public List<bool> _fadeQIn = new List<bool>();
    public List<bool> _fadeQOut = new List<bool>();

    // 출력되는 사운드 정보
    public List<string> _SoundIndex = new List<string>();
    public List<string> _BgmIndex = new List<string>();
    private AudioSource _Se;
    private AudioSource _Bgm;

    // 출력되는 이미지 정보
    public List<string> _CGimgIndex = new List<string>();
    public List<int> _ShadowIndex = new List<int>();
    public List<string> _L_imgIndex = new List<string>();
    public List<string> _R_imgIndex = new List<string>();


    private GameObject CGImg;
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
    private FlashEffCS _flasgEffCS;

    private void Awake()
    {
        _ShakingCS = GameObject.Find("TalkMainScreen").GetComponent<UIShakingCS>();


        _Se = GameObject.Find("SoundMgr").GetComponent<AudioSource>();
        _Bgm = GameObject.Find("BgmMgr").GetComponent<AudioSource>();
        _fadeEffCS = GameObject.Find("Fade_Eff").GetComponent<FadeEffCS>();
        _flasgEffCS = GameObject.Find("Flash_Eff").GetComponent<FlashEffCS>();

        CGImg = GameObject.Find("CGImg");
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
            falsgSys(s);
            ShadowSys(s);
            CGImageSetting(s);
            imageSetting(s);
            soundPlay(s);
            bgmPlay(s);
            fadeSys(s);
            fadeQSys(s);
            ShakingSys(s);

            _currText = ""; // 기존 문구 초기화

            if (fullText[s] == "null")
            {
                _textBox.text = _currText;
                if (_Wait[s] != 0) yield return new WaitForSeconds(_Wait[s]);
                else yield return _nextTextDaleyWait;

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

            if (_Wait[s] != 0) yield return new WaitForSeconds(_Wait[s]);
            else yield return _nextTextDaleyWait;
        }

        Debug.Log("대화 종료");
    }

    public void ShadowSys(int talkValue)
    {
        if (_ShadowIndex[talkValue] == 1) { L_chacterImg.GetComponent<Image>().color = new Color(150.0f / 255.0f, 150.0f / 255.0f, 150.0f / 255.0f, L_chacterImg.GetComponent<Image>().color.a); }
        else { L_chacterImg.GetComponent<Image>().color = new Color(1, 1, 1, L_chacterImg.GetComponent<Image>().color.a); }

        if (_ShadowIndex[talkValue] == 2) { R_chacterImg.GetComponent<Image>().color = new Color(150.0f / 255.0f, 150.0f / 255.0f, 150.0f / 255.0f, R_chacterImg.GetComponent<Image>().color.a); }
        else { R_chacterImg.GetComponent<Image>().color = new Color(1, 1, 1, R_chacterImg.GetComponent<Image>().color.a); }

        if (_ShadowIndex[talkValue] == 0) return;
    }

    public void imageSetting(int talkValue)
    {
        GameObject tempImg = null;

        if (_L_imgIndex[talkValue] != "null")
        {
            L_chacterImg.GetComponent<Image>().color = new Color(L_chacterImg.GetComponent<Image>().color.r, L_chacterImg.GetComponent<Image>().color.g, L_chacterImg.GetComponent<Image>().color.b, 1);
            ResourceMgrCS._imgBox.TryGetValue(_L_imgIndex[talkValue], out tempImg);
            L_chacterImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
            L_chacterImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta;
        }
        else
        {
            L_chacterImg.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        //L_chacterImg.GetComponent<RectTransform>().Translate(
        //    L_chacterImg.GetComponent<RectTransform>().rect.width / 2.0f, L_chacterImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);

        if (_R_imgIndex[talkValue] != "null")
        {
            R_chacterImg.GetComponent<Image>().color = new Color(R_chacterImg.GetComponent<Image>().color.r, R_chacterImg.GetComponent<Image>().color.g, R_chacterImg.GetComponent<Image>().color.b, 1);
            ResourceMgrCS._imgBox.TryGetValue(_R_imgIndex[talkValue], out tempImg);
            R_chacterImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
            R_chacterImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta;
        }
        else
        {
            R_chacterImg.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        //R_chacterImg.GetComponent<RectTransform>().Translate(
        //    -R_chacterImg.GetComponent<RectTransform>().rect.width / 2.0f, R_chacterImg.GetComponent<RectTransform>().rect.height / 2.0f, 0);
    }



    public void CGImageSetting(int talkValue)
    {
        if (_CGimgIndex[talkValue] == "null") return;

        GameObject tempImg = null;

        ResourceMgrCS._CGImg.TryGetValue(_CGimgIndex[talkValue], out tempImg);
        CGImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        //CGImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta;
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
        if (_fadeIn[talkValue] == 0 &&
            _fadeOut[talkValue] == 0) return;

        if (_fadeEffCS._isFadeStart)
        {
            Debug.Log("fadeSys : 이미 실행 중 입니다.");
            return;
        }

        if (_fadeIn[talkValue] != 0) StartCoroutine(_fadeEffCS.FadeIn(_fadeIn[talkValue]));
        if (_fadeOut[talkValue] != 0) StartCoroutine(_fadeEffCS.FadeIn(_fadeOut[talkValue]));
    }

    public void ShakingSys(int talkVlaue)
    {
        if (_ShakingEffIndex[talkVlaue] == 0) return;

        if (_ShakingCS._isShaking)
        {
            Debug.Log("ShakingSys : 이미 실행 중 입니다.");
            return;
        }

        StartCoroutine(_ShakingCS.ShakingOn(_ShakingEffIndex[talkVlaue]));
    }

    public void falsgSys(int talkValue)
    {
        if (!_RedFlashIndex[talkValue] &&
            !_WhiteFlashIndex[talkValue]) return;

        if (_flasgEffCS._isFlashStart)
        {
            Debug.Log("falsgSys : 이미 실행 중 입니다.");
            return;
        }

        if (_RedFlashIndex[talkValue]) StartCoroutine(_flasgEffCS.RedFlashIn(1.0f));
        if (_WhiteFlashIndex[talkValue]) StartCoroutine(_flasgEffCS.WhiteFlashIn(1.0f));
    }
}

