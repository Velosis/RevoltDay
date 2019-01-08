using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eNpcType
{
    gangicon,
    Hamicon,
    Jeonicon,
    Parkicon,
    Wishicon,
    Youngicon
}

public class PlayerInfoCS : MonoBehaviour {
    public enum ePlayerState
    {
        MoveNon,
        MoveReady,
        MoveSet,
        MoveHot
    }

    public eNpcType _eNpcType;

    private UIMgr _uIMgrCS;
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

    // 현재 정보
    public bool _isAlive = false;
    public bool _isTurn = false;

    public int _currTile = 0;
    public int _tempCurrTile = 0;
    public int _currActPoint;
    public int _currTrunPoint;
    private int tempTileXZ;

    // 기타
    public GameObject _nextTileMark;
    public GameObject _moveHandImg;
    private Vector2 _startVec2Pos;
    private Vector2 _endVec2Pos;

    private void Awake()
    {
        if (eNpcType.gangicon == _eNpcType) _isAlive = true;

        _uIMgrCS = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        _tileMakerCS = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>();
        _currMoveState = ePlayerState.MoveReady;
        _currPlayerImg = _currPlayerGO.GetComponent<Image>();
        _currActText.text = _currActPoint.ToString();

        _nextTileMark.SetActive(false);
        _moveHandImg.SetActive(false);

        _currTrunPoint = 0;
    }

    void Start() {

        if (GameObject.Find("MapTileMgr"))
        {
            _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
            if (_eNpcType == eNpcType.gangicon)
            {
                transform.GetComponent<RectTransform>().position = _tileMapList[_tileFirstXZ].GetComponent<RectTransform>().position;
                _currTile = _tileFirstXZ;
            }

        }
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

        if (_tileMapDataCS._isBlockade)
        {
            return;
        } else if (_tileMapDataCS._isShop || _tileMapDataCS._isSpShop)
        {
            _uIMgrCS.isUiOnOff(eUiName.ShopButton, true);
            return;
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
                    break;
                case eNpcType.Jeonicon:
                    break;
                case eNpcType.Parkicon:
                    break;
                case eNpcType.Wishicon:

                    break;
                case eNpcType.Youngicon:
                    break;
                default:
                    break;
            }
        }

    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(_endVec2Pos, 20.0f);
    }
}
