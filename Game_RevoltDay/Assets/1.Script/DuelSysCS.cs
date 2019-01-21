using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eDuelType
{
    S_InFighter = 0,
    R_OutFighter = 1,
    P_Grappler = 2,
    D_Defence = 3,
    R_Reasoning = 4

}

public enum eDuelState
{
    DuelStart, // 결투 시작
    DiceFighterStart, // 결투 시스템 시작
    AttackDelay, // 공격(주사위 던지기) 대기
    DiceDrop, // 주사위 굴리기 연출
    DiceFirst, // 주사위 계산
    BattleDamge, // 전투 결과에 따른 데미지 계산
    BattleEnd, // 전투 종료에 따른 계산 처리
    battleReward // 전투 보상 연출
}

public enum eScreenEffColor
{
    Red,
    Black
}

public class UnitTable
{
    public int _index;
    public int _grade;
    public string _type;
    public string _cgImg;
    public string _nameStr;
    public string _nameEnStr;
    public int _currHp;
    public int _maxCurrHp;
    public int _fight;
    public int _fightDiceMin;
    public int _fightDiceMax;
    public int _infightper;
    public int _outfightper;
    public int _grapplingper;
    public int _inFightPlus;
    public int _outFightPlus;
    public int _grapplingPlus;
}

public class DuelSysCS : MonoBehaviour {

    public PlayerInfoCS _playerInfoCS;
    public NpcSysMgr _npcSysMgrCS;

    public GameObject _playerChrImg;
    public GameObject _enemyChrImg;

    private Vector3 _orginChrImgPos;
    private bool _isHitEff;

    public eDuelType _playerType;
    public eDuelType _enemyOrginType;
    public eDuelType _enemyType;

    public eDuelState _currState;

    public int _FirstWinDice;

    public bool _isNextState;
    public bool _isBackState;

    private eScreenEffColor _flashColor;
    private float _flashValue;
    private float _flashOutValue;
    private bool _isFlashStart;

    private string _playerName;
    public Text _playerNameText;
    private string _playerImgName;
    private GameObject _playerHpText;
    private GameObject _playerAtkText;
    private int _playerHP;
    private int _playerAtk;
    private int _playerDiceValueMin;
    private int _playerDiceValueMax;

    private eNpcType _eEnemyType;
    private string _enemyName;
    private Text _enemyNameText;
    private string _enemyImgName;
    private GameObject _enemyHpText;
    private GameObject _enemyAtkText;
    private int _enemyCurrHP;
    private int _enemyMaxHP;
    private int _enemyAtk;
    private int _enemyDiceValueMin;
    private int _enemyDiceValueMax;
    float[] _enemyAtkTypeList = new float[3];

    public bool _isDuelEnd = false;
    public bool _isCrime = false;

    private RectTransform _L_ChrCenterPos;
    private RectTransform _R_ChrCenterPos;

    private RectTransform _L_ChrMovementPos;
    private RectTransform _R_ChrMovementPos;

    private Vector3 _L_OrginChrPos;
    private Vector3 _R_OrginChrPos;

    private GameObject _duelStartScreen;
    private GameObject _diceFighterStartScreen;
    private GameObject _battleButton;
    private GameObject _runButton;
    private GameObject _ActPointText;

    private GameObject _L_ChrType;
    private GameObject _R_ChrType;
    private GameObject _diceDropImg;
    private Vector3 _OrginDiceImgPos;

    private GameObject _bounsImg;
    private GameObject _bounsImgMovementPos;

    public int _playerDice;
    public int _enemyDice;
    public int _bounsDice;

    private GameObject _playerDiceText;
    private GameObject _enemyDiceText;
    private GameObject _bounsDiceText;

    private GameObject _screenHitEff;
    private GameObject _screenHitShow;
    private GameObject _screenHitText;

    private bool _isPlayerTypeWin;
    private bool _isEnemyTypeWin;

    private float _currTimer;

    public string _currBgmName;
    public string _currHitSeName;

    private AudioSource _seMgr;
    private AudioSource _BgmMgr;

    public UIMgr _uIMgrCS;

    public List<UnitTable> _unitTableList = new List<UnitTable>();

    public Sprite[] _GangSp;
    public Sprite[] _ParkSP;
    public Sprite[] _enemySP;

    public GameObject _ScreenHitEffGO;

    private void Awake()
    {
        _ScreenHitEffGO.SetActive(true);

        _seMgr = GameObject.Find("SoundMgr").GetComponent<AudioSource>();
        _BgmMgr = GameObject.Find("BgmMgr").GetComponent<AudioSource>();

        _playerChrImg = GameObject.Find("L_ChrImg");

        _enemyChrImg = GameObject.Find("R_ChrImg");
        _enemyNameText = _enemyChrImg.transform.GetChild(0).GetComponent<Text>();

        _duelStartScreen = GameObject.Find("_duelStartScreen");

        _diceFighterStartScreen = GameObject.Find("DiceFighterStartScreen");
        _diceFighterStartScreen.SetActive(false);

        _battleButton = GameObject.Find("BattleButton");
        _runButton = GameObject.Find("RunButton");
        _ActPointText = GameObject.Find("ActPointText");

        _L_ChrCenterPos = GameObject.Find("L_ChrCenter").GetComponent<RectTransform>();
        _R_ChrCenterPos = GameObject.Find("R_ChrCenter").GetComponent<RectTransform>();
        _L_OrginChrPos = _L_ChrCenterPos.transform.position;
        _R_OrginChrPos = _R_ChrCenterPos.transform.position;

        _L_ChrMovementPos = GameObject.Find("L_ChrMovementPos").GetComponent<RectTransform>();
        _R_ChrMovementPos = GameObject.Find("R_ChrMovementPos").GetComponent<RectTransform>();

        _playerHpText = GameObject.Find("L_ChrState_Hp");
        _playerAtkText = GameObject.Find("L_ChrState_Atk");

        _enemyHpText = GameObject.Find("R_ChrState_Hp");
        _enemyAtkText = GameObject.Find("R_ChrState_Atk");

        _L_ChrType = GameObject.Find("L_ChrType");
        _L_ChrType.SetActive(false);

        _R_ChrType = GameObject.Find("R_ChrType");
        _R_ChrType.SetActive(false);

        _diceDropImg = GameObject.Find("DiceDropImg");
        _playerDiceText = GameObject.Find("DiceDropImg/playerDice");
        _enemyDiceText = GameObject.Find("DiceDropImg/enemyDice");

        _OrginDiceImgPos = _diceDropImg.transform.position;
        _diceDropImg.SetActive(false);

        _bounsImg = GameObject.Find("BounsDiceImg");
        _bounsDiceText = GameObject.Find("BounsDiceImg/BounsDice");
        _bounsImg.SetActive(false);

        _bounsImgMovementPos = GameObject.Find("BounsDiceImgMovementPos");
        _screenHitEff = GameObject.Find("ScreenHitEff");
        _screenHitEff.SetActive(false);
        _screenHitShow = GameObject.Find("HitEffImg");
        _screenHitShow.SetActive(false);
        _screenHitText = GameObject.Find("HitPointText");
        _screenHitText.SetActive(false);
        _flashColor = eScreenEffColor.Black;

        _currTimer = 0.0f;

        _isNextState = _isBackState = false;
        _isPlayerTypeWin = _isEnemyTypeWin = false;
        _playerType = 0;

        _isDuelEnd = false;
    }

    private void OnEnable()
    {
        AudioClip _currBgm = null;
        ResourceMgrCS._BgmBox.TryGetValue(_currBgmName, out _currBgm);

        _BgmMgr.clip = _currBgm;
        _BgmMgr.loop = true;
        _BgmMgr.volume = 1.0f;
        _BgmMgr.Play();

        AudioClip _currHitSe = null;
        ResourceMgrCS._SoundBox.TryGetValue(_currHitSeName, out _currHitSe);

        _seMgr.clip = _currHitSe;
        _seMgr.loop = false;
        _seMgr.volume = 1.0f;

        _currState = eDuelState.DuelStart;
    }

    public void readUnitTable()
    {
        List<Dictionary<string, object>> date = CSVReader.Read("2.SceneTable/UnitTable");

        for (int i = 0; i < date.Count; i++)
        {
            UnitTable tempUnitTable = new UnitTable();

            tempUnitTable._index = (int)date[i]["Index"];
            tempUnitTable._grade = (int)date[i]["grade"];
            tempUnitTable._type = (string)date[i]["type"];
            tempUnitTable._cgImg = (string)date[i]["cgImg"];
            tempUnitTable._nameStr = (string)date[i]["NameKR"];
            tempUnitTable._nameEnStr = (string)date[i]["NameEN"];
            tempUnitTable._currHp = (int)date[i]["HP"];
            tempUnitTable._maxCurrHp = tempUnitTable._currHp;
            tempUnitTable._fight = (int)date[i]["Fight"];
            tempUnitTable._fightDiceMin = (int)date[i]["FightdiceMin"];
            tempUnitTable._fightDiceMax = (int)date[i]["FightdiceMax"];
            tempUnitTable._infightper = (int)date[i]["Infightper"];
            tempUnitTable._outfightper = (int)date[i]["Outfightper"];
            tempUnitTable._infightper = (int)date[i]["Grapplingper"];
            tempUnitTable._inFightPlus = (int)date[i]["Infightplus"];
            tempUnitTable._outFightPlus = (int)date[i]["Outfightplus"];
            tempUnitTable._grapplingPlus = (int)date[i]["Grapplingplus"];

            _unitTableList.Add(tempUnitTable);
        }
    }

    public void DuelStartSys(eNpcType enemyNpc, int indexValue, bool _isCrime)
    {
        _ActPointText.SetActive(false);
        _runButton.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);

        if (_isCrime || _playerInfoCS._clueTokenValue <= 0)
            _runButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        if (_playerInfoCS._clueTokenValue <= 0) _ActPointText.SetActive(true);

        _isDuelEnd = true;

        int tempHP = 0;
        int tempAtk = 0;

        bool tempCheck = true;
        int tempCheckValue = 0;
        string tempName = "";
        float tempInfight = 0.0f;
        float tempOutfight = 0.0f;
        float tempGrappling = 0.0f;

        _eEnemyType = enemyNpc;
        switch (_playerInfoCS._eNpcType)
        {
            case eNpcType.gangicon:
                _playerName = "강원진";
                break;
            case eNpcType.Parkicon:
                _playerName = "박우주";
                break;
            default:
                break;
        }
        
        _playerHP = _playerInfoCS._currHP;
        _playerAtk = _playerInfoCS._atkPoint;
        

        for (int i = 0; i < _unitTableList.Count; i++)
        {
            if (_playerName == _unitTableList[i]._nameStr)
            {
                _playerImgName = _unitTableList[i]._cgImg;
            }

            if (enemyNpc == eNpcType.normalEnemy)
            {
                if (_unitTableList[i]._index == indexValue)
                {
                    _enemyImgName = _unitTableList[i]._cgImg;
                    _enemyName = _unitTableList[i]._nameStr;
                    tempHP = _unitTableList[i]._maxCurrHp;
                    tempAtk = _unitTableList[tempCheckValue]._fight;
                    tempInfight = _unitTableList[i]._infightper;
                    tempOutfight = _unitTableList[i]._outfightper;
                    tempGrappling = _unitTableList[i]._grapplingper;
                    _enemyDiceValueMin = _unitTableList[i]._fightDiceMin;
                    _enemyDiceValueMax = _unitTableList[i]._fightDiceMax;
                    break;
                }

                while (tempCheck && indexValue == 0)
                {
                    tempCheckValue = Random.Range(0, _unitTableList.Count);
                    tempName = _unitTableList[tempCheckValue]._type;

                    if (tempName != "enemy") tempCheck = true;
                    else
                    {
                        _enemyImgName = _unitTableList[tempCheckValue]._cgImg;
                        _enemyName = _unitTableList[tempCheckValue]._nameStr;
                        tempHP = _unitTableList[tempCheckValue]._maxCurrHp;
                        tempAtk = _unitTableList[tempCheckValue]._fight;
                        tempInfight = _unitTableList[tempCheckValue]._infightper;
                        tempOutfight = _unitTableList[tempCheckValue]._outfightper;
                        tempGrappling = _unitTableList[tempCheckValue]._grapplingper;
                        _enemyDiceValueMin = _unitTableList[i]._fightDiceMin;
                        _enemyDiceValueMax = _unitTableList[i]._fightDiceMax;
                        tempCheck = false;
                        break;
                    }
                }
            }
            else if (enemyNpc == eNpcType.Jeonicon && _unitTableList[i]._nameStr == "전민원")
            {
                _enemyImgName = _unitTableList[i]._cgImg;
                _enemyName = _unitTableList[i]._nameStr;
                tempHP = _unitTableList[i]._maxCurrHp;
                tempAtk = _unitTableList[tempCheckValue]._fight;
                tempInfight = _unitTableList[i]._infightper;
                tempOutfight = _unitTableList[i]._outfightper;
                tempGrappling = _unitTableList[i]._grapplingper;
                _enemyDiceValueMin = _unitTableList[i]._fightDiceMin;
                _enemyDiceValueMax = _unitTableList[i]._fightDiceMax;
                break;
            }
            else if (enemyNpc == eNpcType.Hamicon && _unitTableList[i]._nameStr == "함정임 부하")
            {
                _enemyImgName = _unitTableList[i]._cgImg;
                _enemyName = _unitTableList[i]._nameStr;
                tempHP = _unitTableList[i]._maxCurrHp;
                tempAtk = _unitTableList[tempCheckValue]._fight;
                tempInfight = _unitTableList[i]._infightper;
                tempOutfight = _unitTableList[i]._outfightper;
                tempGrappling = _unitTableList[i]._grapplingper;
                _enemyDiceValueMin = _unitTableList[i]._fightDiceMin;
                _enemyDiceValueMax = _unitTableList[i]._fightDiceMax;
                break;
            }
        }

        _enemyCurrHP = _enemyMaxHP = tempHP;
        _enemyAtk = tempAtk;
        _enemyAtkTypeList[0] = tempInfight;
        _enemyAtkTypeList[1] = tempOutfight;
        _enemyAtkTypeList[2] = tempGrappling;

        SetDuelText();
        DuelStateSet(eDuelState.DuelStart);
    }

    public void SetDuelText()
    {
        GameObject tempImg;
        Vector2 ImgVec2 = new Vector2(Screen.width, Screen.height);

        _playerNameText.text = _playerName;
        ResourceMgrCS._imgBox.TryGetValue(_playerImgName, out tempImg);
        _playerChrImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        _playerChrImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta / 2.0f;

        _playerHpText.GetComponent<Text>().text = "체력 : " + _playerHP.ToString() + " / " + _playerInfoCS._MaxHP.ToString();
        _playerAtkText.GetComponent<Text>().text = "공격력 : " + _playerAtk.ToString();

       ResourceMgrCS._imgBox.TryGetValue(_enemyImgName, out tempImg);
        _enemyChrImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        _enemyNameText.text = _enemyName;


        _enemyChrImg.GetComponent<Image>().sprite = tempImg.GetComponent<Image>().sprite;
        _enemyChrImg.GetComponent<RectTransform>().sizeDelta = tempImg.GetComponent<RectTransform>().sizeDelta / 2.0f;
        _enemyHpText.GetComponent<Text>().text = "체력 : " + _enemyCurrHP.ToString() + " / " + _enemyMaxHP.ToString();
        _enemyAtkText.GetComponent<Text>().text = "공격력 : " + _enemyAtk.ToString();
    }

    public void DuelStateSet(eDuelState state)
    {
        // 예외처리
        _isNextState = _isBackState = _isFlashStart = _isHitEff = false;
        _currTimer = 0.0f;

        switch (state)
        {
            case eDuelState.DuelStart:
                StartCoroutine(DuelStart());
                break;
            case eDuelState.DiceFighterStart:
                StartCoroutine(DiceFighterStart());
                break;
            case eDuelState.AttackDelay:
                StartCoroutine(AttackDelay());
                break;
            case eDuelState.DiceDrop:
                StartCoroutine(DiceDrop());
                break;
            case eDuelState.DiceFirst:
                StartCoroutine(DiceFirst());
                break;
            case eDuelState.BattleDamge:
                StartCoroutine(BattleDamge());
                break;
            case eDuelState.BattleEnd:
                StartCoroutine(BattleEnd());
                break;
            case eDuelState.battleReward:
                RewardSys();
                break;
            default:
                Debug.Log("DuelStateSet : 선택된 전투 상태가 없습니다.");
                break;
        }
    }

    public void isDuelStart() { _isNextState = true; }
    public void isTypeSelect(int isType) { _isNextState = true; _playerType = (eDuelType)isType; }
    public void isBack() { _isBackState = true; }

    public IEnumerator DuelStart()
    {
        StartCoroutine(screenHitEff(0.5f, eScreenEffColor.Black));

        // 초기값 세팅
        _battleButton.SetActive(true);
        _runButton.SetActive(true);
        _L_ChrType.SetActive(false);
        _R_ChrType.SetActive(false);
        _bounsImg.SetActive(false);
        _diceDropImg.SetActive(false);

        _L_ChrCenterPos.transform.position = _L_OrginChrPos;
        _R_ChrCenterPos.transform.position = _R_OrginChrPos;
        _diceDropImg.transform.position = _OrginDiceImgPos;

        while (true)
        {
            if (_isNextState) { DuelStateSet(eDuelState.DiceFighterStart); }
            yield return null;
        }
    }

    public void SetItemOptionImg()
    {
        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false);

        if (_enemyType == eDuelType.S_InFighter) _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
        else if (_enemyType == eDuelType.R_OutFighter) _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
        else if (_enemyType == eDuelType.P_Grappler) _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);



        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
        _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = Color.white;

        if (_playerInfoCS._currUseEquipF._Codex != 0)
        {
            switch (_playerInfoCS._currUseEquipF._DuelType)
            {
                case eDuelType.S_InFighter:
                    _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;

                    break;
                case eDuelType.R_OutFighter:
                    _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.red;

                    break;
                case eDuelType.P_Grappler:
                    _diceFighterStartScreen.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = Color.red;

                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator DiceFighterStart()
    {
        _diceFighterStartScreen.SetActive(true); // UI 켜주기

        SetItemOptionImg();
        while (true)
        {
            if (_isNextState)
            {
                _diceFighterStartScreen.SetActive(false);
                DuelStateSet(eDuelState.AttackDelay);
            }
            else if (_isBackState)
            {
                _diceFighterStartScreen.SetActive(false);
                DuelStateSet(eDuelState.DuelStart);
            }
            yield return null;
        }
    }

    public IEnumerator AttackDelay()
    {
        _battleButton.SetActive(false);
        _runButton.SetActive(false);

        bool tempSort = false;
        float tempSwap = 0.0f;
        int tempSwapCount = 0;

        while (!tempSort)
        {
            tempSwap = 0.0f;
            if (_enemyAtkTypeList[tempSwapCount] < _enemyAtkTypeList[tempSwapCount + 1])
            {
                tempSwap = _enemyAtkTypeList[tempSwapCount];
                _enemyAtkTypeList[tempSwapCount] = _enemyAtkTypeList[tempSwapCount + 1];
                _enemyAtkTypeList[tempSwapCount + 1] = tempSwap;
                tempSwapCount++; 
                if (tempSwapCount == 3) tempSwapCount = 0;
                tempSort = false;
            }
            else
            {
                tempSort = true;
            }
        }

        if (Random.Range(1.0f, 100.0f) <= 100.0f - _enemyAtkTypeList[0]) _enemyType = eDuelType.S_InFighter;
        else if (Random.Range(1.0f, 100.0f) <= 100.0f - _enemyAtkTypeList[1]) _enemyType = eDuelType.R_OutFighter;
        else if (Random.Range(1.0f, 100.0f) <= 100.0f - _enemyAtkTypeList[2]) _enemyType = eDuelType.P_Grappler;

        _currTimer = 0.0f;
        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 3.0f;

            _L_ChrCenterPos.position = Vector3.Lerp(_L_ChrCenterPos.position, _L_ChrMovementPos.position, _currTimer / 3.0f);
            _R_ChrCenterPos.position = Vector3.Lerp(_R_ChrCenterPos.position, _R_ChrMovementPos.position, _currTimer / 3.0f);
            yield return null;
        }

        DuelStateSet(eDuelState.DiceDrop);
    }

    public IEnumerator DiceDrop()
    {
        _playerDiceText.GetComponent<Text>().color = Color.white;
        _enemyDiceText.GetComponent<Text>().color = Color.white;
        _bounsDiceText.GetComponent<Text>().color = Color.white;

        _L_ChrType.SetActive(true);
        _R_ChrType.SetActive(true);

        _L_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _R_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        _currTimer = 0.0f;
        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 1.0f;

            _L_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, _currTimer);
            _R_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, _currTimer);

            yield return null;
        }

        _currTimer = 0.0f;
        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 2.0f;

            if ((_playerType == eDuelType.P_Grappler && _enemyType == eDuelType.S_InFighter) ||
                (_playerType == eDuelType.R_OutFighter && _enemyType == eDuelType.P_Grappler) ||
                (_playerType == eDuelType.S_InFighter && _enemyType == eDuelType.R_OutFighter)) // 플레이어 패배
            {
                _L_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, _L_ChrType.GetComponent<Image>().color.a - _currTimer);
                _isEnemyTypeWin = true;
            }
            else if ((_enemyType == eDuelType.P_Grappler && _playerType == eDuelType.S_InFighter) ||
                    (_enemyType == eDuelType.R_OutFighter && _playerType == eDuelType.P_Grappler) ||
                    (_enemyType == eDuelType.S_InFighter && _playerType == eDuelType.R_OutFighter))  // 상대 패배
            {
                _R_ChrType.GetComponent<Image>().color = new Color(1, 1, 1, _R_ChrType.GetComponent<Image>().color.a - _currTimer);
                _isPlayerTypeWin = true;
            }
            else
            {
                _isPlayerTypeWin = _isEnemyTypeWin = false;
            }

            yield return null;
        }

        if (_isEnemyTypeWin || _isPlayerTypeWin)
        {
            _bounsImg.SetActive(true);
            _bounsImg.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            _currTimer = 0.0f;
            while (_currTimer <= 1.0f)
            {
                _currTimer += Time.deltaTime / 2.0f;
                _bounsImg.transform.position = Vector3.Lerp(_bounsImg.transform.position, _bounsImgMovementPos.transform.position, _currTimer / 3.0f);
                _bounsImg.GetComponent<Image>().color = new Color(1, 1, 1, _currTimer);
                yield return null;
            }

            _currTimer = 0.0f;
            while (_currTimer <= 1.0f)
            {
                _currTimer += Time.deltaTime / 2.0f;

                _bounsDice = Random.Range(1, 6 + 1);
                _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();

                yield return null;
            }
        }

        if (_isPlayerTypeWin && _playerInfoCS._currUseEquipF._Codex != 0 && _playerInfoCS._currUseEquipF._Fight > 0)
        {
            int tempValue = _playerInfoCS._currUseEquipF._Fight;
            _currTimer = 0.0f;
            yield return new WaitForSeconds(1.0f);
            while (_currTimer <= 1.0f)
            {
                if (tempValue != 0)
                {
                    _bounsDice += 1;
                    tempValue -= 1;
                    _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                    _bounsDiceText.GetComponent<Text>().color = Color.green;
                    StartCoroutine(DiceEff(_bounsDiceText, 0.4f));
                }
                else
                {
                    _currTimer = 2.0f;
                }
                _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                yield return new WaitForSeconds(1.0f);
            }
        }

        _diceDropImg.SetActive(true);
        _diceDropImg.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        _currTimer = 0.0f;
        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 2.0f;
            _diceDropImg.GetComponent<Image>().color = new Color(1, 1, 1, _currTimer);
        }

        _currTimer = 0.0f;
        StartCoroutine(DiceEff(_playerDiceText,3.0f));
        StartCoroutine(DiceEff(_enemyDiceText,3.0f));

        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 3.0f;

            _playerDice = Random.Range(1, 6 + 1);
            _enemyDice = Random.Range(_enemyDiceValueMin, _enemyDiceValueMax);

            _playerDiceText.GetComponent<Text>().text = _playerDice.ToString();
            _enemyDiceText.GetComponent<Text>().text = _enemyDice.ToString();

            yield return new WaitForSeconds(Time.deltaTime / 3.0f);
        }

        DuelStateSet(eDuelState.DiceFirst);
    }

    public IEnumerator DiceEff(GameObject _gameObject, float _time)
    {
        Vector3 tempVector3 = new Vector3();
        tempVector3 = _gameObject.GetComponent<RectTransform>().localPosition;

        float TempTime = 0.0f;
        while (TempTime < _time)
        {
            TempTime += Time.deltaTime;
            _gameObject.GetComponent<RectTransform>().localPosition = tempVector3;
            _gameObject.GetComponent<RectTransform>().Translate(0, (Random.Range(-15.0f, + 15.0f)), 0);
            yield return null;
        }

        _gameObject.GetComponent<RectTransform>().localPosition = tempVector3;
        yield break;
    }

    public IEnumerator DiceFirst()
    {
        if (_isEnemyTypeWin || _isPlayerTypeWin) // 무승부 일 경우 계산 생략
        {
            _currTimer = 0.0f;
            yield return new WaitForSeconds(1.0f);
            if (_isPlayerTypeWin) // 플레이어가 속성 싸움에서 이겼다면
            {
                while (_currTimer <= 1.0f)
                {
                    if (_bounsDice > 0 && _enemyDice != 0)
                    {
                        _bounsDice -= 1;
                        _enemyDice -= 1;
                        _enemyDiceText.GetComponent<Text>().text = _enemyDice.ToString();
                        _enemyDiceText.GetComponent<Text>().color = Color.red;
                        StartCoroutine(DiceEff(_enemyDiceText, 0.5f));
                    }
                    else
                    {
                        _currTimer = 2.0f;
                    }
                    _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else if (_isEnemyTypeWin) // 플레이어가 속성 싸움에서 졌다면
            {
                while (_currTimer <= 1.0f)
                {
                    if (_bounsDice > 0 && _playerDice != 0)
                    {
                        _bounsDice -= 1;
                        _playerDice -= 1;
                        _playerDiceText.GetComponent<Text>().text = _playerDice.ToString();
                        _playerDiceText.GetComponent<Text>().color = Color.red;
                        StartCoroutine(DiceEff(_playerDiceText, 0.5f));
                    }
                    else
                    {
                        _currTimer = 2.0f;
                    }
                    _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                    yield return new WaitForSeconds(0.5f);
                }
            }

            if (_bounsDice != 0)
            {
                _currTimer = 0.0f;
                yield return new WaitForSeconds(0.5f);
                if (_isPlayerTypeWin) // 플레이어가 속성 싸움에서 이겼다면
                {
                    while (_currTimer <= 1.0f)
                    {
                        if (_bounsDice > 0)
                        {
                            _bounsDice -= 1;
                            _playerDice += 1;
                            _playerDiceText.GetComponent<Text>().text = _playerDice.ToString();
                            _playerDiceText.GetComponent<Text>().color = Color.green;
                            StartCoroutine(DiceEff(_playerDiceText, 0.5f));
                        }
                        else
                        {
                            _currTimer = 2.0f;
                        }
                        _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                else if (_isEnemyTypeWin) // 플레이어가 속성 싸움에서 졌다면
                {
                    while (_currTimer <= 1.0f)
                    {
                        if (_bounsDice > 0)
                        {
                            _bounsDice -= 1;
                            _enemyDice += 1;
                            _enemyDiceText.GetComponent<Text>().text = _enemyDice.ToString();
                            _enemyDiceText.GetComponent<Text>().color = Color.green;
                            StartCoroutine(DiceEff(_enemyDiceText, 0.5f));
                        }
                        else
                        {
                            _currTimer = 2.0f;
                        }
                        _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }
        }

        yield return new WaitForSeconds(2.0f);
        DuelStateSet(eDuelState.BattleDamge);
    }

    public IEnumerator BattleDamge()
    {
        if (_playerAtk + _playerDice != 0)
        {
            _enemyCurrHP -= _playerAtk + _playerDice;
            if (_enemyCurrHP < 0) _enemyCurrHP = 0;
            SetDuelText();
            StartCoroutine(ChrImgHitEff(false));
        }

        yield return new WaitForSeconds(2.0f);

        if (_enemyAtk + _enemyDice != 0)
        {
            int tempDef = _playerInfoCS._currUseEquipF._Fight < 0 ? _playerInfoCS._currUseEquipF._Fight : 0;
            _playerHP -= (_enemyAtk + tempDef) + _enemyDice;
            if (_playerHP < 0) _playerHP = 0;
            else if (_playerHP > _playerInfoCS._MaxHP) _playerHP = _playerInfoCS._MaxHP;
            SetDuelText();
            StartCoroutine(ChrImgHitEff(true));
        }

        yield return new WaitForSeconds(2.0f);
        DuelStateSet(eDuelState.BattleEnd);
    }

    public IEnumerator BattleEnd()
    {
        _isPlayerTypeWin = _isEnemyTypeWin = false;

        yield return new WaitForSeconds(2.0f);
        if (_enemyCurrHP <= 0 || _playerHP <= 0) DuelStateSet(eDuelState.battleReward);
        else DuelStateSet(eDuelState.DuelStart);
    }

    public IEnumerator screenHitEff(float timer, eScreenEffColor isColor)
    {
        _screenHitEff.SetActive(true);
        switch (isColor)
        {
            case eScreenEffColor.Red:
                _screenHitEff.GetComponent<Image>().color = new Color(1, 0, 0, 0);
                break;
            case eScreenEffColor.Black:
                _screenHitEff.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                break;
            default:
                break;
        }

        _flashValue = 255.0f;
        _flashOutValue = (255.0f / timer) * 0.1f;
        Image tempColor = _screenHitEff.GetComponent<Image>();

        while (!_isFlashStart)
        {
            _flashValue -= _flashOutValue;
            _screenHitEff.GetComponent<Image>().color = new Color(tempColor.color.r, tempColor.color.g, tempColor.color.b, _flashValue / 255.0f);
            if (_flashValue <= 0.0f) _isFlashStart = true;
            yield return new WaitForSeconds(0.1f);
        }
        _flashValue = 0.0f;
        _isFlashStart = false;
        _screenHitEff.SetActive(false);

        yield return null;
    }

    public IEnumerator ChrImgHitEff(bool isWho)
    {
        _screenHitShow.SetActive(false);
        _screenHitText.SetActive(false);

        RectTransform tempRtTf = _screenHitShow.GetComponent<RectTransform>();
        RectTransform tempTextPos = _screenHitText.GetComponent<RectTransform>();
        Image tempPImg = _screenHitShow.transform.GetChild(0).gameObject.GetComponent<Image>();
        Image tempEImg = _screenHitShow.transform.GetChild(1).gameObject.GetComponent<Image>();

        _screenHitShow.transform.GetChild(0).gameObject.SetActive(false);
        _screenHitShow.transform.GetChild(1).gameObject.SetActive(false);

        switch (_playerInfoCS._eNpcType)
        {
            case eNpcType.gangicon:
                switch (_playerType)
                {
                    case eDuelType.S_InFighter:
                        tempPImg.sprite = _GangSp[1];
                        break;
                    case eDuelType.R_OutFighter:
                        tempPImg.sprite = _GangSp[2];
                        break;
                    case eDuelType.P_Grappler:
                        tempPImg.sprite = _GangSp[0];
                        break;
                    default:
                        break;
                }
                break;
            case eNpcType.Parkicon:
                switch (_playerType)
                {
                    case eDuelType.S_InFighter:
                        tempPImg.sprite = _ParkSP[1];
                        break;
                    case eDuelType.R_OutFighter:
                        tempPImg.sprite = _ParkSP[2];

                        break;
                    case eDuelType.P_Grappler:
                        tempPImg.sprite = _ParkSP[0];

                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        switch (_enemyType)
        {
            case eDuelType.S_InFighter:
                tempEImg.sprite = _enemySP[1];

                break;
            case eDuelType.R_OutFighter:
                tempEImg.sprite = _enemySP[2];

                break;
            case eDuelType.P_Grappler:
                tempEImg.sprite = _enemySP[0];

                break;
            default:
                break;
        }

        tempRtTf.localPosition = new Vector2(0, 0);
        tempTextPos.localPosition = new Vector2(0, 0);

        GameObject tempImg;
        Vector2 orginPos = new Vector2(0, 0);
        _currTimer = 0.0f;
        _flashValue = 0.0f;
        _flashOutValue = (255.0f / 1.0f) * 0.1f;

        if (isWho)
        {
            _screenHitShow.transform.GetChild(0).gameObject.SetActive(false);

            _screenHitShow.transform.GetChild(1).gameObject.SetActive(true);
            _orginChrImgPos = _playerChrImg.transform.position;
            tempImg = _playerChrImg;
            tempRtTf.localPosition = new Vector2(tempRtTf.localPosition.x + tempRtTf.sizeDelta.x, 0);
            tempTextPos.localPosition = _playerChrImg.GetComponent<RectTransform>().localPosition;
            _screenHitShow.SetActive(true);
            _screenHitText.SetActive(true);
            _screenHitText.GetComponent<Text>().text = "-" + (_enemyAtk + _enemyDice).ToString();
            while (_currTimer <= 0.5f)
            {
                _currTimer += Time.deltaTime;
                if (tempRtTf.localPosition.x > orginPos.x) tempRtTf.Translate(Vector3.left * (tempRtTf.sizeDelta.x / 5.0f));
                else tempRtTf.localPosition = new Vector2(0, 0);
                yield return null;
            }
            _currTimer = 0.0f;
            while (_currTimer <= 0.3f)
            {
                _currTimer += Time.deltaTime;
                yield return null;
            }
            _seMgr.Play();
        }
        else
        {
            _screenHitShow.transform.GetChild(1).gameObject.SetActive(false);

            _screenHitShow.transform.GetChild(0).gameObject.SetActive(true);
            _orginChrImgPos = _enemyChrImg.transform.position;
            tempImg = _enemyChrImg;
            tempRtTf.localPosition = new Vector2(tempRtTf.localPosition.x - tempRtTf.sizeDelta.x, 0);
            tempTextPos.localPosition = _enemyChrImg.GetComponent<RectTransform>().localPosition;
            _screenHitShow.SetActive(true);
            _screenHitText.SetActive(true);
            _screenHitText.GetComponent<Text>().text = "-" + (_playerAtk + _playerDice).ToString();
            while (_currTimer <= 0.5f)
            {
                _currTimer += Time.deltaTime;
                if (tempRtTf.localPosition.x < orginPos.x) tempRtTf.Translate(Vector3.right * (tempRtTf.sizeDelta.x / 5.0f));
                else tempRtTf.localPosition = new Vector2(0, 0);
                yield return null;
            }
            _currTimer = 0.0f;
            while (_currTimer <= 0.3f)
            {
                _currTimer += Time.deltaTime;
                yield return null;
            }
            _seMgr.Play();
        }
        _screenHitShow.SetActive(false);
        _currTimer = 0.0f;
        while (!_isHitEff && _currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime;
            tempImg.transform.position = _orginChrImgPos;
            tempImg.transform.Translate(Random.Range(-40.0f, +40.0f), Random.Range(-40.0f, +40.0f), 0, Space.Self);

            _flashValue += _flashOutValue;
            tempImg.GetComponent<Image>().color = new Color(1, _flashValue / 255.0f, _flashValue / 255.0f, 1);

            if (_flashValue >= 255.0f) _isHitEff = true;
            yield return null;
        }
        _isHitEff = false;
        tempImg.transform.position = _orginChrImgPos;
        tempImg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        _screenHitText.SetActive(false);

        yield return null;
    }

    public void RewardSys()
    {
        _isDuelEnd = false;
        if (_playerHP <= 0) Debug.Log("게임 오버");
        else if (_enemyCurrHP <= 0)
        {
            if (_enemyName == "전민원" || _enemyName == "함정임 부하")
            {
                Debug.Log("결투 NpcAct 실행");
                _npcSysMgrCS.NpcActPlay();
            }
        }
        _uIMgrCS.EndDuel();

        return;
    }

    public void RunSys()
    {
        if (_isCrime || _playerInfoCS._clueTokenValue <= 0) return;

        if (_playerInfoCS._clueTokenValue > 0) _playerInfoCS._clueTokenValue--;
        _uIMgrCS.EndDuel();
    }
}
