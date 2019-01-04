using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCS : MonoBehaviour {

    public enum ePlayerState
    {
        MoveNon,
        MoveReady,
        MoveSet,
        MoveHot
    }

    private TileMakerCS _tileMakerCS;
    public ePlayerState _currMoveState;
    public GameObject _currPlayerGO;
    static public Image _currPlayerImg;

    private List<GameObject> _tileMapList;
    private Vector2 _tilePos = Vector2.zero;
    private int _tileValue = 0;

    private int _playerTileX = 3;
    private int _playerTileY = 1;

    private void Awake()
    {
        _tileMakerCS = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>();
        _currMoveState = ePlayerState.MoveReady;
        _currPlayerImg = _currPlayerGO.GetComponent<Image>();
    }

    void Start () {

        if (GameObject.Find("MapTileMgr"))
        {
            _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;
            transform.GetComponent<RectTransform>().position = _tileMapList[8].GetComponent<RectTransform>().position;
        }
    }

    private void Update()
    {
        if (TouchSysCS._currTouchState == eTouchState.DOWN) _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y);
    }

    public void PlayerMoveSys()
    {
        // 터치를 사용한 움직임 구현
        Debug.Log(_currMoveState.ToString());
        switch (_currMoveState)
        {
            case ePlayerState.MoveNon: // 움직이지 않는 상태
                break;
            case ePlayerState.MoveReady: // 움직임 대기 상태
                break;
            case ePlayerState.MoveSet: // 이동할 타일이 선택된 상태
                break;
            case ePlayerState.MoveHot: // 선택된 타일로 이동
                break;
            default:
                break;
        }
    }

    public void PlayerTilePos(int mapValue)
    {
    }

    public void PlayerIconTouch()
    {
        if (ePlayerState.MoveReady != _currMoveState) return;

        _tileMakerCS.getTileVlaue(TouchSysCS._touchPos.x, TouchSysCS._touchPos.y);
        _currMoveState = ePlayerState.MoveSet;
    }
    

}
