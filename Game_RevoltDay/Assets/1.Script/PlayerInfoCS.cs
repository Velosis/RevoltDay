using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePlayerSceneState
{
    Map,
    Talk,
    Duel
}

public class PlayerInfoCS : MonoBehaviour {

    public enum ePlayerState
    {
        MoveNon,
        MoveReady,
        MoveSet,
        MoveHot
    }

    public ePlayerSceneState _currSceneState;
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
        _currSceneState = ePlayerSceneState.Map;
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
        switch (_currSceneState)
        {
            case ePlayerSceneState.Map:
                if (_currMoveState != ePlayerState.MoveNon) PlayerMoveSys();
                break;
            case ePlayerSceneState.Talk:
                break;
            case ePlayerSceneState.Duel:
                break;
            default:
                break;
        }
    }

    public void PlayerMoveSys()
    {
        // 터치를 사용한 움직임 구현
        switch (_currMoveState)
        {
            case ePlayerState.MoveReady:
                if (TouchSysCS._currTouchState == eTouchState.DOWN) _currMoveState = ePlayerState.MoveSet;
                break;
            case ePlayerState.MoveSet:
                break;
            case ePlayerState.MoveHot:
                if (TouchSysCS._currTouchState == eTouchState.DOWN)
                {
                    transform.GetComponent<RectTransform>().position = _tilePos;
                    _currMoveState = ePlayerState.MoveReady;
                }
                break;
            default:
                break;
        }
    }

    public void PlayerTilePos(int mapValue)
    {
        //if (_currMoveState == )
        _tilePos = _tileMapList[mapValue].GetComponent<RectTransform>().position;
        _tileValue = mapValue;
        _currMoveState = ePlayerState.MoveHot;
    }

    

}
