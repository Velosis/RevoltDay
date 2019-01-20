using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct sTemp
{
    public string _playerName;
    public int _hpPoint;
}

public enum eNpcType
{
    gangicon,
    Hamicon,
    Jeonicon,
    Parkicon,
    Wishicon,
    Youngicon,
    normalEnemy
}

public class PlayerInfoCS : MonoBehaviour {
    private GameObject _gameMgr;

    public enum ePlayerState
    {
        MoveNon,
        MoveReady,
        MoveSet,
        MoveHot
    }

    public eNpcType _eNpcType;

    private PlayerInfoCS _playerInfoCS;

    private UIMgr _uIMgrCS;
    private NpcSysMgr _npcSysMgr;
    private TileMakerCS _tileMakerCS;
    public ePlayerState _currMoveState;
    public GameObject _currPlayerGO;
    public Text _currActText;
    static public Image _currPlayerImg;

    // 타일 관련 정보
    private List<GameObject> _tileMapList;
    private TileMapDataCS _tileMapDataCS;
    private Vector2 _tilePos = Vector2.zero;
    public int _tileFirstXZ;


    public int _maxMoney = 9999;
    // 현재 정보
    public List<ItemData> _BoxItemList;
    public List<EquipData> _BoxEquipList;
    public List<AidData> _BoxAidList;

    public List<ItemData> _currUseItemList;
    public EquipData _currUseEquipF;
    public EquipData _currUseEquipD;
    public AidData _currUseAid;

    public int _currMoney = 5;
    public int _clueTokenValue = 0;
    public bool _isAlive = false;
    public bool _isTurn = false;
    public int _daleyTurnCount = 0;
    public int _reasoningValue = 3;

    public int _currTile = 0;
    public int _tempCurrTile = 0;
    public int _currActPoint;
    public int _currTrunPoint;
    private int tempTileXZ;

    public int _currHP;
    public int _MaxHP;
    public int _atkPoint;

    // 기타
    public GameObject _nextTileMark;
    public GameObject _moveHandImg;
    private Vector2 _startVec2Pos;
    private Vector2 _endVec2Pos;

    private void Awake()
    {
        _gameMgr = GameObject.Find("GameMgr");

        if (eNpcType.gangicon == _eNpcType)
        {
            _clueTokenValue += 0;
            _isAlive = true;

            _BoxItemList = new List<ItemData>();
            _BoxEquipList = new List<EquipData>();
            _BoxAidList = new List<AidData>();

            _currUseItemList = new List<ItemData>();
            _currUseEquipF = new EquipData();
            _currUseEquipD = new EquipData();
            _currUseAid = new AidData();
        }

        _currHP = _MaxHP; // 체력 정립

        _npcSysMgr = GameObject.Find("NpcMgr").GetComponent<NpcSysMgr>();
        _playerInfoCS = GameObject.Find("PlayerIcon").GetComponent<PlayerInfoCS>();
        _uIMgrCS = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        _tileMakerCS = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>();
        _currMoveState = ePlayerState.MoveReady;
        _currPlayerImg = _currPlayerGO.GetComponent<Image>();
        _currActText.text = _currActPoint.ToString();

        _nextTileMark.SetActive(false);
        _moveHandImg.SetActive(false);

        _currTrunPoint = 0;

        if (GameObject.Find("MapTileMgr"))
        {
            _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
            if (_eNpcType == eNpcType.gangicon)
            {
                transform.GetComponent<RectTransform>().position = _tileMapList[_tileFirstXZ].GetComponent<RectTransform>().position;
                _currTile = _tileFirstXZ;
            }

        }

        if (eNpcType.gangicon == _eNpcType && 
            _gameMgr.GetComponent<SaveSys>()._saveFile.isSaveData) saveLead();
    }

    public void saveLead()
    {
        Debug.Log(_gameMgr.GetComponent<SaveSys>().name + "  플레이어 데이터 호출");
        SaveSys TextSave = _gameMgr.GetComponent<SaveSys>();
        _clueTokenValue = TextSave._saveFile._clueTokenValue;
        _isAlive = TextSave._saveFile._isAlive;
        _isTurn = TextSave._saveFile._isTurn;
        _daleyTurnCount = TextSave._saveFile._daleyTurnCount;
        _reasoningValue = TextSave._saveFile._reasoningValue;

        _currTile = TextSave._saveFile._currTile;
        setTileValeu(_currTile);

        _tempCurrTile = TextSave._saveFile._tempCurrTile;
        _currActPoint = TextSave._saveFile._currActPoint;
        _currTrunPoint = TextSave._saveFile._currTrunPoint;
    }

    private void Update()
    {
        if (_eNpcType != eNpcType.gangicon) return;

        if (!_uIMgrCS._SearchMgr.activeSelf && Input.touchCount != 0 && !_moveHandImg.activeSelf &&
            Input.GetTouch(0).phase == TouchPhase.Began && _currMoveState != ePlayerState.MoveNon)
            PlayerMoveSys();
    }

    public void setActPoint(int value)
    {
        _currActPoint -= value;
        _currActText.text = _currActPoint.ToString();
    }


    public void PlayerTileXZ()
    {
        _currTrunPoint++;
        _uIMgrCS.tileTurnUpdate();

        _uIMgrCS.isUiOnOff(eUiName.all, false);
        _tileMapDataCS = _tileMapList[_currTile].GetComponent<TileMapDataCS>();

        _uIMgrCS.isUiOnOff(eUiName.SearchButton, true);
        _uIMgrCS.isUiOnOff(eUiName.ItemButton, true);
        _uIMgrCS.isUiOnOff(eUiName.WaitButton, true);
        _uIMgrCS.isUiOnOff(eUiName.SafetyButton, true);

        if (_tileMapDataCS._isBlockade)
        {
            return;
        } else if (_tileMapDataCS._isShop || _tileMapDataCS._isSpShop)
        {
            _uIMgrCS.isUiOnOff(eUiName.ShopButton, true);
            return;
        }
    }

    public void PlayerMoveNot(bool isTrye)
    {
        if (isTrye)
        {
            _currMoveState = ePlayerState.MoveReady;
            _nextTileMark.SetActive(false);

        }
        else
        {
            _currMoveState = ePlayerState.MoveNon;
            _nextTileMark.SetActive(false);
        }

    }

    public void PlayerMoveSys()
    {
        // 터치를 사용한 움직임 구현
        switch (_currMoveState)
        {
            case ePlayerState.MoveNon: // 움직이지 않는 상태
                break;
            case ePlayerState.MoveReady: // 이동할 타일을 선택하는 상태
                tempTileXZ = _currTile;
                _tempCurrTile = _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y);
                if ((tempTileXZ == _tempCurrTile && _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y) != -1) ||
                    (_tempCurrTile == -1) ||
                    (Mathf.Abs(tempTileXZ - _tempCurrTile) > _currActPoint))
                {
                    _tempCurrTile = tempTileXZ;
                    return;
                }
                _nextTileMark.SetActive(true);
                _nextTileMark.transform.position = _tileMapList[_tempCurrTile].transform.position;
                _nextTileMark.transform.GetChild(0).GetComponent<Text>().text = "-" + (Mathf.Abs(tempTileXZ - _tempCurrTile));

                _currMoveState = ePlayerState.MoveSet;
                break;
            case ePlayerState.MoveSet: // 이동할 타일이 선택된 상태
                if (_tempCurrTile == _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y))
                {
                    _currMoveState = ePlayerState.MoveHot;
                    PlayerMoveSys();
                }else if (_tempCurrTile != _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y))
                {
                    _currMoveState = ePlayerState.MoveReady;
                    PlayerMoveSys();
                }
                break;
            case ePlayerState.MoveHot: // 선택된 타일로 이동
                _nextTileMark.SetActive(false);
                _endVec2Pos = _tileMapList[_tempCurrTile].transform.position;
                StartCoroutine(mpvePlayerMove());
                
                break;
            default:
                break;
        }
    }

    public void npcMoveSys(int tileMap)
    {
        _endVec2Pos = _tileMapList[tileMap].transform.position;
        StartCoroutine(mpvePlayerMove());
    }

    public void setTileValeu (int setTile)
    {
        _currTile = setTile;
       transform.position = _tileMapList[setTile].transform.position;
    }

    public IEnumerator mpvePlayerMove()
    {
        float tempTime = 0.0f;
        _startVec2Pos = transform.position;
        _moveHandImg.SetActive(true);
        Vector2 tempPos = _moveHandImg.transform.localPosition;
        Color tempColor = Color.white;
        _moveHandImg.transform.Translate(Vector2.up * 30.0f);
        _moveHandImg.GetComponent<Image>().color = tempColor;

        while (tempTime < 1.0f)
        {
            tempTime += Time.deltaTime;
            _moveHandImg.transform.Translate(Vector2.down * (30.0f * Time.deltaTime));
            tempColor.a = tempTime;
            _moveHandImg.GetComponent<Image>().color = tempColor;
            yield return null;
        }

        tempTime = 0.0f;
        while (tempTime < 1.5f)
        {
            tempTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _endVec2Pos, tempTime);
            yield return null;
        }

        tempTime = 0.0f;
        while (tempTime < 1.0f)
        {
            tempTime += Time.deltaTime;
            _moveHandImg.transform.Translate(Vector2.up * (30.0f * Time.deltaTime));
            tempColor.a = 1.0f - tempTime;
            _moveHandImg.GetComponent<Image>().color = tempColor;
            yield return null;
        }

        _moveHandImg.transform.localPosition = tempPos;
        _moveHandImg.SetActive(false);

        _currActPoint -= Mathf.Abs(tempTileXZ - _currTile);
        //PlayerTileXZ();
        _currActText.text = _currActPoint.ToString();
        _currTile = _tempCurrTile;
        _currMoveState = ePlayerState.MoveReady;

        if (_eNpcType != eNpcType.gangicon) // 플레이어가 아닐 경우
        {
            switch (_eNpcType)
            {
                case eNpcType.gangicon:
                    // 플레이어 캐릭터 생략
                    break;
                case eNpcType.Hamicon:
                    if (_playerInfoCS._currTile == _currTile)
                    {
                        _uIMgrCS.StartDuel();
                        _uIMgrCS._DuelMgr.GetComponent<DuelSysCS>().DuelStartSys(_eNpcType, 0, true);
                    }
                    break;
                case eNpcType.Jeonicon:
                    if (_playerInfoCS._currTile == _currTile)
                    {
                        _uIMgrCS.StartDuel();
                        _uIMgrCS._DuelMgr.GetComponent<DuelSysCS>().DuelStartSys(_eNpcType, 0, true);
                    }
                    break;
                case eNpcType.Parkicon:
                    break;
                case eNpcType.Wishicon:

                    break;
                case eNpcType.Youngicon:
                    if (_playerInfoCS._currTile == _currTile)
                    {
                        Debug.Log("보유 토큰 : "+_playerInfoCS._clueTokenValue);
                        if (_playerInfoCS._clueTokenValue > 0) _playerInfoCS._clueTokenValue -= 1;
                        Debug.Log("보유 토큰 : " + _playerInfoCS._clueTokenValue);
                    }
                    break;
                default:
                    break;
            }
        }

    }

    public void setMoney(int value)
    {
        _currMoney += value;
        if (_currMoney >= _maxMoney) _currMoney = _maxMoney;
    }

    public void setHpPoint(int value)
    {
        _currHP += value;
        if (_currHP > _MaxHP)
        {
            _currHP = _MaxHP;
        }
    }

    public void UseItem(int arrIdex)
    {
        _BoxItemList.RemoveAt(arrIdex);
    }

    public void isDie(bool isAlive)
    {
        _isAlive = isAlive;

        if (!isAlive)
        {
            transform.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        }
        else if (isAlive)
        {
            transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(_endVec2Pos, 20.0f);
    }
}
