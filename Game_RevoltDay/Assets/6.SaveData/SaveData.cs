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


namespace SaveDateNamespace
{
    [CreateAssetMenu(menuName = "Game_RevoltDay/SaveFile")]
    public class SaveData : ScriptableObject
    {
        public bool isSaveData;

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

        public npcSaveData _HamIcon;
        public npcSaveData _JeonIcon;
        public npcSaveData _ParkIcon;
        public npcSaveData _WishIcon;
        public npcSaveData _YoungIcon;
    }
}


