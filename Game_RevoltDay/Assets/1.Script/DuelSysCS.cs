using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eDuelType
{
    S_InFighter = 0,
    R_OutFighter = 1,
    P_Grappler = 2
}

public enum eDuelState
{
    DuelStart, // 결투 시작
    DiceFighterStart, // 결투 시스템 시작
    AttackDelay, // 공격(주사위 던지기) 대기
    DiceDrop, // 주사위 굴리기 연출
    DiceFirst, // 주사위 계산
    BattleDamge, // 전투 결과에 따른 데미지 계산
    BattleEnd // 전투 종료에 따른 계산 처리
}

public class DuelSysCS : MonoBehaviour {

    public GameObject _playerChrImg;
    public GameObject _enemyChrImg;

    public eDuelType _playerType;
    public eDuelType _enemyType;

    public eDuelState _currState;

    public int _FirstWinDice;

    public bool _isNextState;
    public bool _isBackState;

    private GameObject _playerHpText;
    private GameObject _playerAtkText;
    private int _playerHP;
    private int _playerAtk;

    private GameObject _enemyHpText;
    private GameObject _enemyAtkText;
    private int _enemyHP;
    private int _enemyAtk;

    private RectTransform _L_ChrCenterPos;
    private RectTransform _R_ChrCenterPos;

    private RectTransform _L_ChrMovementPos;
    private RectTransform _R_ChrMovementPos;

    private GameObject _duelStartScreen;
    private GameObject _diceFighterStartScreen;
    private GameObject _battleButton;
    private GameObject _skillButton;
    private GameObject _runButton;

    private GameObject _L_ChrType;
    private GameObject _R_ChrType;
    private GameObject _diceDropImg;
    private GameObject _bounsImg;
    private GameObject _bounsImgMovementPos;

    public int _playerDice;
    public int _enemyDice;
    public int _bounsDice;

    private GameObject _playerDiceText;
    private GameObject _enemyDiceText;
    private GameObject _bounsDiceText;

    private bool _isPlayerTypeWin;
    private bool _isEnemyTypeWin;

    private float _currTimer;

    private void Awake()
    {


        _playerChrImg = GameObject.Find("L_ChrImg");
        _enemyChrImg = GameObject.Find("R_ChrImg");

        _duelStartScreen = GameObject.Find("_duelStartScreen");

        _diceFighterStartScreen = GameObject.Find("DiceFighterStartScreen");
        _diceFighterStartScreen.SetActive(false);

        _battleButton = GameObject.Find("BattleButton");
        _skillButton = GameObject.Find("SkillButton");
        _runButton = GameObject.Find("RunButton");

        _L_ChrCenterPos = GameObject.Find("L_ChrCenter").GetComponent<RectTransform>();
        _R_ChrCenterPos = GameObject.Find("R_ChrCenter").GetComponent<RectTransform>();

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
        _diceDropImg.SetActive(false);

        _bounsImg = GameObject.Find("BounsDiceImg");
        _bounsDiceText = GameObject.Find("BounsDiceImg/BounsDice");
        _bounsImg.SetActive(false);

        _bounsImgMovementPos = GameObject.Find("BounsDiceImgMovementPos");

        _currTimer = 0.0f;

        // 테스트용 코드
        _isNextState = _isBackState = false;
        _isPlayerTypeWin = _isEnemyTypeWin = false;
        _playerType = (eDuelType)Random.Range(0 , 2 + 1);
        _enemyType = (eDuelType)Random.Range(0, 2 + 1);

        _playerHP = 10;
        _playerAtk = 2;
        _playerHpText.GetComponent<Text>().text ="체력 : " + _playerHP.ToString() + " / " + 10.ToString();
        _playerAtkText.GetComponent<Text>().text = "공격력 : " + _playerAtk.ToString();

        _enemyHP = 10;
        _enemyAtk = 2;
        _enemyHpText.GetComponent<Text>().text = "체력 : " + _enemyHP.ToString() + " / " + 10.ToString();
        _enemyAtkText.GetComponent<Text>().text = "공격력 : " + _enemyAtk.ToString();

    }

    private void Start()
    {
        _currState = eDuelState.DuelStart;
        Debug.Log("게임 시작" + _currState.ToString());

        DuelStateSet(_currState);
    }

    public void DuelStateSet(eDuelState state)
    {
        StopAllCoroutines(); // 코루틴 예외 처리

        // 예외처리
        _isNextState = _isBackState = false;
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
        _playerChrImg.GetComponent<Image>().sprite = PlayerInfoCS._currPlayerImg.sprite;

        while (true)
        {
            if (_isNextState) { DuelStateSet(eDuelState.DiceFighterStart); }
            yield return null;
        }
    }

    public IEnumerator DiceFighterStart()
    {
        _diceFighterStartScreen.SetActive(true); // UI 켜주기

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
        _skillButton.SetActive(false);
        _runButton.SetActive(false);

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
        while (_currTimer <= 1.0f)
        {
            _currTimer += Time.deltaTime / 3.0f;

            _playerDice = Random.Range(1, 6 + 1);
            _enemyDice = Random.Range(1, 6 + 1);

            _playerDiceText.GetComponent<Text>().text = _playerDice.ToString();
            _enemyDiceText.GetComponent<Text>().text = _enemyDice.ToString();
            yield return new WaitForSeconds(Time.deltaTime / 3.0f);
        }

        DuelStateSet(eDuelState.DiceFirst);
    }

    public IEnumerator DiceFirst()
    {
        if (_isEnemyTypeWin || _isPlayerTypeWin) // 무승부 일 경우 계산 생략
        {

            _currTimer = 0.0f;
            while (_currTimer <= 1.0f)
            {
                _currTimer += Time.deltaTime / 2.0f;

                _bounsDice = Random.Range(1, 6 + 1);
                _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();

                yield return null;
            }


            _currTimer = 0.0f;
            while (_currTimer <= 1.0f)
            {
                if (_isPlayerTypeWin) // 플레이어가 속성 싸움에서 이겼다면
                {
                    if (_bounsDice > 0 && _enemyDice != 0)
                    {
                        _bounsDice -= 1;
                        _enemyDice -= 1;
                        _enemyDiceText.GetComponent<Text>().text = _enemyDice.ToString();
                    }
                    else
                    {
                        _currTimer = 2.0f;
                    }
                }
                else if (_isEnemyTypeWin) // 플레이어가 속성 싸움에서 졌다면
                {
                    if (_bounsDice > 0 && _playerDice != 0)
                    {
                        _bounsDice -= 1;
                        _playerDice -= 1;
                        _playerDiceText.GetComponent<Text>().text = _playerDice.ToString();
                    }
                    else
                    {
                        _currTimer = 2.0f;
                    }
                }
                Debug.Log("_bounsDice : " + _bounsDice.ToString());

                _bounsDiceText.GetComponent<Text>().text = _bounsDice.ToString();
                yield return new WaitForSeconds(1.0f);
            }
        }

        yield return new WaitForSeconds(2.0f);
        DuelStateSet(eDuelState.BattleDamge);
    }

    public IEnumerator BattleDamge()
    {
        if (_playerAtk + _playerDice != 0)
        {
            _enemyHP -= _playerAtk + _playerDice;
            _enemyHpText.GetComponent<Text>().text = _enemyHP.ToString();
        }

        if (_enemyAtk + _enemyDice != 0)
        {
            _playerHP -= _enemyAtk + _enemyDice;
            _playerHpText.GetComponent<Text>().text = _playerHP.ToString();
        }

        yield return new WaitForSeconds(2.0f);
        DuelStateSet(eDuelState.BattleEnd);
    }

    public IEnumerator BattleEnd()
    {
        _isPlayerTypeWin = _isEnemyTypeWin = false;

        yield return new WaitForSeconds(2.0f);
        DuelStateSet(eDuelState.DuelStart);
    }

    public void hitEff()
    {

    }
}
