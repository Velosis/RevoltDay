using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class _JeonMoveRoot
{
    public enum eRootType
    {
        Non,
        A,
        B,
        C,
        D
    }
    public eRootType _currRoot = eRootType.Non;
    public float _rootChangeValue;

    public int _nextRootTile;
    public int _currRootValue = 0;

    public int[] _currList;
    public int[] _jeonMoveTileListA;
    public int[] _jeonMoveTileListB;
    public int[] _jeonMoveTileListC;
    public int[] _jeonMoveTileListD;
}

[System.Serializable]
public class _YoungIconMoveRoot
{
    public int _nextRootTile;
    public int _currRootValue = 0;

    public int[] _YoungMoveTileList;
}

[System.Serializable]
public class _HamMoveRoot
{
    public enum eRootType
    {
        Non,
        A,
        B,
        C
    }
    public eRootType _currRoot = eRootType.Non;
    public float _rootChangeValue;

    public int _nextRootTile;
    public int _currRootValue = 0;

    public int[] _currList;
    public int[] _jeonMoveTileListA;
    public int[] _jeonMoveTileListB;
    public int[] _jeonMoveTileListC;
}

public class NpcSysMgr : MonoBehaviour {
    public GameObject _gameMgr;
    public UIMgr _uIMgrCS;

    public GameObject[] _npcList;
    public List<GameObject> _tileMapList;

    public GameObject _playerIcon;
    public GameObject _HamIcon;
    public GameObject _JeonIcon;
    public GameObject _ParkIcon;
    public GameObject _WishIcon;
    public GameObject _YoungIcon;

    public _JeonMoveRoot _JeonRootBox;
    public _YoungIconMoveRoot _YoungRootBox;
    public _HamMoveRoot _HamRootBox;

    private void Awake()
    {
        _npcList = GameObject.Find("GameMgr").GetComponent<EventSysCS>()._iconList;
        _tileMapList = GameObject.Find("MapTileMgr").GetComponent<TileMakerCS>().TileMapList;

        for (int i = 0; i < _npcList.Length; i++)
        {
            switch (_npcList[i].GetComponent<PlayerInfoCS>()._eNpcType)
            {
                case eNpcType.gangicon:
                    _playerIcon = _npcList[i];
                    break;
                case eNpcType.Hamicon:
                    _HamIcon = _npcList[i];
                    break;
                case eNpcType.Jeonicon:
                    _JeonIcon = _npcList[i];
                    break;
                case eNpcType.Parkicon:
                    _ParkIcon = _npcList[i];
                    break;
                case eNpcType.Wishicon:
                    _WishIcon = _npcList[i];
                    break;
                case eNpcType.Youngicon:
                    _YoungIcon = _npcList[i];
                    break;
                default:
                    break;
            }
        }

    }

    private void Start()
    {
        
        //sttingNPC(5, eNpcType.Hamicon);
        //sttingNPC(5, eNpcType.Jeonicon);
        //sttingNPC(5, eNpcType.Wishicon);
        //sttingNPC(6, eNpcType.Youngicon);
        

        if (_gameMgr.GetComponent<SaveSys>()._saveFile.isSaveData) saveLead();
    }

    public void saveLead()
    {
        SaveSys tempData = _gameMgr.GetComponent<SaveSys>();
        if (tempData._saveFile._HamIcon._isAlive)
        {
            _HamIcon.GetComponent<PlayerInfoCS>()._isTurn = tempData._saveFile._HamIcon._isTurn;
            _HamIcon.GetComponent<PlayerInfoCS>()._currTile = tempData._saveFile._HamIcon._currTile;
            _HamRootBox = tempData._saveFile._HamSaveData;
            _HamIcon.GetComponent<PlayerInfoCS>().setTileValeu(_HamIcon.GetComponent<PlayerInfoCS>()._currTile);
        }

        if (tempData._saveFile._JeonIcon._isAlive)
        {
            _JeonIcon.GetComponent<PlayerInfoCS>()._isTurn = tempData._saveFile._JeonIcon._isTurn;
            _JeonIcon.GetComponent<PlayerInfoCS>()._currTile = tempData._saveFile._JeonIcon._currTile;
            _JeonRootBox = tempData._saveFile._JeonSaveData;
            _JeonIcon.GetComponent<PlayerInfoCS>().setTileValeu(_JeonIcon.GetComponent<PlayerInfoCS>()._currTile);

        }

        if (tempData._saveFile._WishIcon._isAlive)
        {
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = tempData._saveFile._WishIcon._isTurn;
            _WishIcon.GetComponent<PlayerInfoCS>()._currTile = tempData._saveFile._WishIcon._currTile;
            _WishIcon.GetComponent<PlayerInfoCS>().setTileValeu(_WishIcon.GetComponent<PlayerInfoCS>()._currTile);

        }

        if (tempData._saveFile._YoungIcon._isAlive)
        {
            _YoungIcon.GetComponent<PlayerInfoCS>()._isTurn = tempData._saveFile._YoungIcon._isTurn;
            _YoungIcon.GetComponent<PlayerInfoCS>()._currTile = tempData._saveFile._YoungIcon._currTile;
            _YoungRootBox = tempData._saveFile._YoungSaveData;
            _YoungIcon.GetComponent<PlayerInfoCS>().setTileValeu(_YoungIcon.GetComponent<PlayerInfoCS>()._currTile);

        }
    }

    public void npcTurnReset()
    {
        for (int i = 0; i < _npcList.Length; i++)
        {
            _npcList[i].GetComponent<PlayerInfoCS>()._isTurn = false;
        }
    }

    public void NpcActPlay()
    {
        StartCoroutine(NpcAct());
    }

    public IEnumerator NpcAct()
    {
        _playerIcon.GetComponent<PlayerInfoCS>().PlayerMoveNot(false);
        int TempTurnValue = 0;
        for (int i = 0; i < _npcList.Length; i++)
        {
            if (_npcList[i].GetComponent<PlayerInfoCS>()._isAlive &&
                !_npcList[i].GetComponent<PlayerInfoCS>()._isTurn) TempTurnValue++;
        }
        TempTurnValue--; // 플레이어 보정
        if (!_uIMgrCS._DuelMgr.GetComponent<DuelSysCS>()._isDuelEnd &&
            _JeonIcon.GetComponent<PlayerInfoCS>()._isAlive && !_JeonIcon.GetComponent<PlayerInfoCS>()._isTurn)
        {
            JeonAI();

            yield return new WaitForSeconds(4.0f);
            TempTurnValue--;
        }

        if (!_uIMgrCS._DuelMgr.GetComponent<DuelSysCS>()._isDuelEnd &&
            _HamIcon.GetComponent<PlayerInfoCS>()._isAlive && !_HamIcon.GetComponent<PlayerInfoCS>()._isTurn)
        {
            HamAI();

            yield return new WaitForSeconds(4.0f);
            TempTurnValue--;
        }

        if (!_uIMgrCS._DuelMgr.GetComponent<DuelSysCS>()._isDuelEnd &&
            _WishIcon.GetComponent<PlayerInfoCS>()._isAlive && !_WishIcon.GetComponent<PlayerInfoCS>()._isTurn)
        {
            wishAI();

            yield return new WaitForSeconds(4.0f);
            TempTurnValue--;
        }


        if (!_uIMgrCS._DuelMgr.GetComponent<DuelSysCS>()._isDuelEnd &&
            _YoungIcon.GetComponent<PlayerInfoCS>()._isAlive && !_YoungIcon.GetComponent<PlayerInfoCS>()._isTurn)
        {
            YoungAI();

            yield return new WaitForSeconds(4.0f);
            TempTurnValue--;
        }

        if (TempTurnValue == 0)
        {
            _playerIcon.GetComponent<PlayerInfoCS>()._currUseAid = new AidData();
            npcTurnReset();
            UIMgr._sNpeTurnEnd = false;
            _playerIcon.GetComponent<PlayerInfoCS>().PlayerMoveNot(true);

        }
        yield return null;
    }

    public void DieNpc(eNpcType whoNpc)
    {
        switch (whoNpc)
        {
            case eNpcType.Hamicon:
                _HamIcon.GetComponent<PlayerInfoCS>()._isAlive = false;
                _HamIcon.transform.position = Vector2.zero;
                _HamIcon.GetComponent<PlayerInfoCS>()._currTile = -1;
                break;
            case eNpcType.Jeonicon:
                _JeonIcon.GetComponent<PlayerInfoCS>()._isAlive = false;
                _JeonIcon.transform.position = Vector2.zero;
                _JeonIcon.GetComponent<PlayerInfoCS>()._currTile = -1;
                break;
            case eNpcType.Wishicon:
                _WishIcon.GetComponent<PlayerInfoCS>()._isAlive = false;
                _WishIcon.transform.position = Vector2.zero;
                _WishIcon.GetComponent<PlayerInfoCS>()._currTile = -1;
                break;
            case eNpcType.Youngicon:
                _YoungIcon.GetComponent<PlayerInfoCS>()._isAlive = false;
                _YoungIcon.transform.position = Vector2.zero;
                _YoungIcon.GetComponent<PlayerInfoCS>()._currTile = -1;
                break;
            default:
                break;
        }
    }

    public void sttingNPC(int tileMap, eNpcType whoNpc)
    {
        for (int i = 0; i < _npcList.Length; i++)
        {
            if (_npcList[i].GetComponent<PlayerInfoCS>()._eNpcType == whoNpc)
            {
                _npcList[i].GetComponent<PlayerInfoCS>()._isAlive = true;
                _npcList[i].GetComponent<PlayerInfoCS>().setTileValeu(tileMap);
            }
        }
    }

    public void HamAI()
    {
        int tempCurrTile = _HamIcon.GetComponent<PlayerInfoCS>()._currTile;

        if (_HamIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount > 0 &&
            !_HamIcon.GetComponent<PlayerInfoCS>()._isAlive)
        {
            _HamIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount--;
            _HamIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
        }

        // 전민원이 삼성동일 경우 루트 설정
        if (tempCurrTile == 5)
        {
            _HamRootBox._currRootValue = 0;

            int tempRandom = Random.Range(1, 4);
            if (tempRandom == 1) // A
            {
                _HamRootBox._currRoot = _HamMoveRoot.eRootType.A;
                _HamRootBox._currList = _HamRootBox._jeonMoveTileListA;
                _HamRootBox._nextRootTile = _HamRootBox._jeonMoveTileListA[_HamRootBox._currRootValue];
            }
            else if (tempRandom == 2) // B
            {
                _HamRootBox._currRoot = _HamMoveRoot.eRootType.B;
                _HamRootBox._currList = _HamRootBox._jeonMoveTileListB;
                _HamRootBox._nextRootTile = _HamRootBox._jeonMoveTileListB[_HamRootBox._currRootValue];
            }
            else if (tempRandom == 3) // C
            {
                _HamRootBox._currRoot = _HamMoveRoot.eRootType.C;
                _HamRootBox._currList = _HamRootBox._jeonMoveTileListC;
                _HamRootBox._nextRootTile = _HamRootBox._jeonMoveTileListC[_HamRootBox._currRootValue];
            }
        }

        if (_playerIcon.GetComponent<PlayerInfoCS>()._currTile != _HamIcon.GetComponent<PlayerInfoCS>()._currTile)
        {
            if (_HamRootBox._currList.Length != _HamRootBox._currRootValue) _HamRootBox._currRootValue++;
            else
            {
                return;
            }

            _HamRootBox._nextRootTile = tempCurrTile = _HamRootBox._currList[_HamRootBox._currRootValue];
            _HamIcon.GetComponent<PlayerInfoCS>()._tempCurrTile = _HamRootBox._nextRootTile;

            _HamIcon.GetComponent<PlayerInfoCS>().npcMoveSys(_HamRootBox._nextRootTile);

            _HamIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
            return;
        }
    }

    public void YoungAI()
    {
        int tempCurrTile = _YoungIcon.GetComponent<PlayerInfoCS>()._currTile;

        if (_YoungIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount > 0 && !_YoungIcon.GetComponent<PlayerInfoCS>()._isAlive)
        {
            _YoungIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount--;
            _YoungIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
        }

        if (_playerIcon.GetComponent<PlayerInfoCS>()._currTile != _YoungIcon.GetComponent<PlayerInfoCS>()._currTile)
        {
            if (_YoungRootBox._YoungMoveTileList.Length == _YoungRootBox._currRootValue) _YoungRootBox._currRootValue = 0;

            _YoungRootBox._nextRootTile = tempCurrTile = _YoungRootBox._YoungMoveTileList[_YoungRootBox._currRootValue];
            _YoungIcon.GetComponent<PlayerInfoCS>()._tempCurrTile = _YoungRootBox._nextRootTile;

            _YoungIcon.GetComponent<PlayerInfoCS>().npcMoveSys(_YoungRootBox._nextRootTile);

            _YoungIcon.GetComponent<PlayerInfoCS>()._isTurn = true;

            if (_YoungRootBox._YoungMoveTileList.Length != _YoungRootBox._currRootValue) _YoungRootBox._currRootValue++;

            return;
        }
        else
        {
            if (_playerIcon.GetComponent<PlayerInfoCS>()._clueTokenValue > 0) _playerIcon.GetComponent<PlayerInfoCS>()._clueTokenValue -= 1;

            _YoungIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
        }
    }

    public void JeonAI()
    {
        int tempCurrTile = _JeonIcon.GetComponent<PlayerInfoCS>()._currTile;

        if (_JeonIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount > 0 &&
            !_JeonIcon.GetComponent<PlayerInfoCS>()._isAlive)
        {
            _JeonIcon.GetComponent<PlayerInfoCS>()._daleyTurnCount--;
            _JeonIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
        }

        // 전민원이 삼성동일 경우 루트 설정
        if (tempCurrTile == 5 && _JeonRootBox._currRoot != _JeonMoveRoot.eRootType.A)
        {
            _JeonRootBox._currRoot = _JeonMoveRoot.eRootType.A;
            _JeonRootBox._currList = _JeonRootBox._jeonMoveTileListA;
            _JeonRootBox._currRootValue = 0;
            _JeonRootBox._nextRootTile = _JeonRootBox._jeonMoveTileListA[_JeonRootBox._currRootValue];

        }
        else if (tempCurrTile == 12 && _JeonRootBox._currRoot != _JeonMoveRoot.eRootType.B)
        {
            _JeonRootBox._currRoot = _JeonMoveRoot.eRootType.B;
            _JeonRootBox._currList = _JeonRootBox._jeonMoveTileListB;
            _JeonRootBox._currRootValue = 0;
            _JeonRootBox._nextRootTile = _JeonRootBox._jeonMoveTileListB[_JeonRootBox._currRootValue];
        }
        else if (Random.Range(1.0f, 100.0f) <= _JeonRootBox._rootChangeValue &&
            tempCurrTile == 7 && _JeonRootBox._currRoot != _JeonMoveRoot.eRootType.C)
        {
            _JeonRootBox._currRoot = _JeonMoveRoot.eRootType.C;
            _JeonRootBox._currList = _JeonRootBox._jeonMoveTileListC;
            _JeonRootBox._currRootValue = 0;
            _JeonRootBox._nextRootTile = _JeonRootBox._jeonMoveTileListC[_JeonRootBox._currRootValue];
        }
        else if (Random.Range(1.0f, 100.0f) <= _JeonRootBox._rootChangeValue &&
            tempCurrTile == 11 && _JeonRootBox._currRoot != _JeonMoveRoot.eRootType.D)
        {
            _JeonRootBox._currRoot = _JeonMoveRoot.eRootType.D;
            _JeonRootBox._currList = _JeonRootBox._jeonMoveTileListD;
            _JeonRootBox._currRootValue = 0;
            _JeonRootBox._nextRootTile = _JeonRootBox._jeonMoveTileListD[_JeonRootBox._currRootValue];
        }



        if (_playerIcon.GetComponent<PlayerInfoCS>()._currTile != _JeonIcon.GetComponent<PlayerInfoCS>()._currTile)
        {
            if (_JeonRootBox._currList.Length != _JeonRootBox._currRootValue) _JeonRootBox._currRootValue++;
            else
            {
                return;
            }

            _JeonRootBox._nextRootTile = tempCurrTile = _JeonRootBox._currList[_JeonRootBox._currRootValue];
            _JeonIcon.GetComponent<PlayerInfoCS>()._tempCurrTile = _JeonRootBox._nextRootTile;

            _JeonIcon.GetComponent<PlayerInfoCS>().npcMoveSys(_JeonRootBox._nextRootTile);

            _JeonIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
            return;
        }
    }

    public void wishAI()
    {
        int tempCurrTile = _WishIcon.GetComponent<PlayerInfoCS>()._currTile;
        if (Random.Range(1.0f, 100.0f) < 20.0f && _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._isBlockade)
        {
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>().setBlockade(false);
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
            return;
        }
        else if (Random.Range(1.0f, 100.0f) < 10.0f && _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
        {
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>().setSafetyBer(-5.0f);
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = true;

            return;
        }else if (_tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._isBlockade ||
            _tileMapList[tempCurrTile].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
        {
            _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = true;

            return;
        }


        for (int i = 0; i < _tileMapList.Count; i++) // 치안 게이지가 3이상인 곳이 있는지 탐색
        {

            if (_tileMapList[i].GetComponent<TileMapDataCS>()._isBlockade ||
                _tileMapList[i].GetComponent<TileMapDataCS>()._SafetyValue >= (1.0f / 5.0f) * 3.0f)
            {
                _WishIcon.GetComponent<PlayerInfoCS>().npcMoveSys(i);
                _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = true;

                return;
            }
        }

        _WishIcon.GetComponent<PlayerInfoCS>()._isTurn = true;
        return;
    }
}
