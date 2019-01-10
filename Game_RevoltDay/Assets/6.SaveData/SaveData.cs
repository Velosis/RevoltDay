using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class npcSaveData
{
    public int _currTile;
    public bool _isAlive;
    public bool _isTurn;
}

[System.Serializable]
public class tileMapDate
{
    public int _tileIndex = 0;
    public float _SafetyValue;

    public bool _isSafetyEff;
    public bool _isBlockade;
    public bool _isIssueIcon;

    public bool _isShop;
    public bool _isSpShop;
    public bool _isIssue;
    public bool _isCrime;
}


namespace SaveDateNamespace
{
    [CreateAssetMenu(menuName = "Game_RevoltDay/SaveFile")]
    public class SaveData : ScriptableObject
    {
        public bool isSaveData;

        // 맵 타일 저장
        public tileMapDate[] _tileMapList;

        // 스토리 진행 관련
        public int _currEventID;

        // 현재 정보
        public int _clueTokenValue;
        public bool _isAlive;
        public bool _isTurn;
        public int _daleyTurnCount;
        public int _reasoningValue;

        public int _currTile;
        public int _tempCurrTile;
        public int _currActPoint;
        public int _currTrunPoint;

        public int _currHP;
        public int _MaxHP;
        public int _atkPoint;

        public npcSaveData _WishIcon;

        public npcSaveData _HamIcon;
        public _HamMoveRoot _HamSaveData;

        public npcSaveData _JeonIcon;
        public _JeonMoveRoot _JeonSaveData;

        public npcSaveData _YoungIcon;
        public _YoungIconMoveRoot _YoungSaveData;

    }
}


