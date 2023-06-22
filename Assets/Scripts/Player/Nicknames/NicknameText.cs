using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    const float OFFSET_POSITION_Y = 2.5f;
    Transform _player;
    Text _myName;
    public NicknameText SetPlayer(NetworkPlayer player)
    {
        _myName = GetComponent<Text>();
        _player = player.transform;
        return this;
    }
    public void UpdateName(string name)
    {
        _myName.text = name;
    }

    public void UpdatePosition()
    {
        transform.position = _player.position + Vector3.up * OFFSET_POSITION_Y;
    }
}
