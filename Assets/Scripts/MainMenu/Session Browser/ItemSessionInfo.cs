using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSessionInfo : MonoBehaviour
{
    [SerializeField] Text _sessionNameText;
    [SerializeField] Text _playerCountText;
    [SerializeField] Button _joinBTN;

    Fusion.SessionInfo _sessionInfo;

    public event System.Action<Fusion.SessionInfo> OnJoinSession;

    public void SetSessionInfo(Fusion.SessionInfo session) 
    {
        _sessionInfo = session;
        _sessionNameText.text = _sessionInfo.Name;
        _playerCountText.text = $"{_sessionInfo.PlayerCount}/2";
        _joinBTN.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }

    public void OnBTNClick()
    {
        OnJoinSession?.Invoke(_sessionInfo);
    }
}
