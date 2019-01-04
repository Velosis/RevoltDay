using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapDataCS : MonoBehaviour {
    public int _tileIndex = 0;

    public GameObject _playerData;

    private void Awake()
    {
        _playerData = GameObject.Find("PlayerIcon");
    }

    public void inputPlayer()
    {
        _playerData.GetComponent<PlayerInfoCS>().PlayerTilePos(_tileIndex);
    }
}
