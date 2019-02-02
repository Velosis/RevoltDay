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
    public List<ItemData> _BoxItemList = new List<ItemData>();
    public List<EquipData> _BoxEquipList = new List<EquipData>();
    public List<AidData> _BoxAidList = new List<AidData>();

    public List<ItemData> _currUseItemList = new List<ItemData>();
    public EquipData _currUseEquipF = new EquipData();
    public EquipData _currUseEquipD = new EquipData();
    public AidData _currUseAid = new AidData();

    public int _buffAtk = 0;
    public int _buffDectective = 0;

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

        if (eNpcType.gangicon == _eNpcType ||
            eNpcType.Parkicon == _eNpcType)
        {
            _clueTokenValue += 0;
            _isAlive = true;
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
            if (_eNpcType == eNpcType.gangicon ||
                eNpcType.Parkicon == _eNpcType)
            {
                transform.GetComponent<RectTransform>().position = _tileMapList[_tileFirstXZ].GetComponent<RectTransform>().position;
                _currTile = _tempCurrTile = _tileFirstXZ;
            }

        }


        if ((eNpcType.gangicon == _eNpcType || eNpcType.Parkicon == _eNpcType) && 
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

    private void Start()
    {
        if (_eNpcType == eNpcType.gangicon || eNpcType.Parkicon == _eNpcType) PlayerTileXZ();

    }

    private void Update()
    {
        if (_eNpcType != eNpcType.gangicon ||
            eNpcType.Parkicon == _eNpcType) return;

        if (!_uIMgrCS._SearchMgr.activeSelf && Input.touchCount != 0 && !_moveHandImg.activeSelf &&
            Input.GetTouch(0).phase == TouchPhase.Began && _currMoveState != ePlayerState.MoveNon)
            PlayerMoveSys();
    }

    public void ResetUseItemList()
    {
        for (int i = 0; i < _BoxAidList.Count; i++)
        {
            if (_BoxAidList[i]._currCoolTime > 0) _BoxAidList[i]._currCoolTime--;
        }

        _currUseAid._isSet = false;
        _currUseAid = new AidData();

        for (int i = 0; i < _currUseItemList.Count; i++)
        {
            if (_currUseItemList[i]._currTurnOtp > 0) _currUseItemList[i]._currTurnOtp--;
            if (_currUseItemList[i]._currTurnOtp <= 0) _currUseItemList.RemoveAt(i);
        }

        

        _buffAtk = 0;
        _buffDectective = 0;


    }

    public bool setAidUse(AidData _aidData)
    {
        // 현재 사용중인 조력자가 없는 경우 실행
        if (_currUseAid._Codex != 0) return false;

        // 없는 경우 생으로 넣는다(주소는 1개)
        _currUseAid = _aidData;

        if (_currUseAid._Fight != 0) _buffAtk += _currUseAid._Fight;
        else if (_currUseAid._Money != 0) setMoney(_currUseAid._Money);
        else if (_currUseAid._Move != 0) _currActPoint += _currUseAid._Move;
        else if (_currUseAid._Token != 0) _clueTokenValue += _currUseAid._Token;

        _currUseAid._currCoolTime = _currUseAid._CoolTime;
        _currUseAid._isSet = true;
        return true;
    }

    public bool setActUseItem(ItemData _itemData)
    {
        // 이미 적용 중인게 있고 더 높을 경우를 체크
        for (int i = 0; i < _currUseItemList.Count; i++)
        {
            if (_currUseItemList[i]._Fight > _itemData._Fight) return false;
        }

        _currUseItemList.Add(ResourceMgrCS.SettingItemData(_itemData));

        if (_currUseItemList[_currUseItemList.Count - 1]._Fight != 0 &&
            _currUseItemList[_currUseItemList.Count - 1]._TurnOtp != 0)
        {
            _buffAtk += _currUseItemList[_currUseItemList.Count - 1]._Fight;
        }
        else if (_currUseItemList[_currUseItemList.Count - 1]._Dectective != 0 &&
            _currUseItemList[_currUseItemList.Count - 1]._TurnOtp != 0) 
        {
            _buffDectective += _currUseItemList[_currUseItemList.Count - 1]._Dectective;
        }
        else if (_currUseItemList[_currUseItemList.Count - 1]._Move != 0 &&
            _currUseItemList[_currUseItemList.Count - 1]._TurnOtp != 0)
        {
            _currActPoint += _currUseItemList[_currUseItemList.Count - 1]._Move;
        }
        else if (_currUseItemList[_currUseItemList.Count - 1]._Restore != 0)
        {
            setHpPoint(_currUseItemList[_currUseItemList.Count - 1]._Restore);
            _currUseItemList.RemoveAt(_currUseItemList.Count - 1);
        }

        return true;
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
        _tileMapDataCS = _tileMapList[_tempCurrTile].GetComponent<TileMapDataCS>();

        _uIMgrCS.isUiOnOff(eUiName.SearchButton, true);
        _uIMgrCS.isUiOnOff(eUiName.ItemButton, true);
        _uIMgrCS.isUiOnOff(eUiName.WaitButton, true);
        _uIMgrCS.isUiOnOff(eUiName.SafetyButton, true);

        if (_tileMapDataCS._isShop || _tileMapDataCS._isSpShop)
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
        if (!_uIMgrCS.TileCheck()) return;

        // 터치를 사용한 움직임 구현
        switch (_currMoveState)
        {
            case ePlayerState.MoveNon: // 움직이지 않는 상태
                break;
            case ePlayerState.MoveReady: // 이동할 타일을 선택하는 상태
                tempTileXZ = _currTile;
                _tempCurrTile = _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y);
                if ((tempTileXZ == _tempCurrTile && _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y) != -1) ||
                (_tempCurrTile == -1) || (_currActPoint <= 0))
                {
                    _tempCurrTile = tempTileXZ;
                    return;
                }
                _nextTileMark.SetActive(true);
                _nextTileMark.transform.position = _tileMapList[_tempCurrTile].transform.position;
                _nextTileMark.transform.GetChild(0).GetComponent<Text>().text = "-1";
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
                setActPoint(1);
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
        if (_eNpcType == eNpcType.gangicon ||
            _eNpcType == eNpcType.Parkicon) PlayerTileXZ();
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
                        _uIMgrCS._DuelMgr.GetComponent<DuelSysCS>().DuelStartSys(_eNpcType, 0, true, eTableType.Non);
                    }
                    break;
                case eNpcType.Jeonicon:
                    if (_playerInfoCS._currTile == _currTile)
                    {
                        _uIMgrCS.StartDuel();
                        _uIMgrCS._DuelMgr.GetComponent<DuelSysCS>().DuelStartSys(_eNpcType, 0, true, eTableType.Non);
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

    public void setClue(int value)
    {
        _clueTokenValue += value;
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
