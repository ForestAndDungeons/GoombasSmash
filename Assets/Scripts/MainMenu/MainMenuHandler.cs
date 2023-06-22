using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [Header("Panel")]
    [SerializeField] GameObject _joinLobbyPanel;
    [SerializeField] GameObject _statusPanel;
    //[SerializeField] GameObject _browserPanel;
    [SerializeField] SessionListHandler _sessionListH;
    [SerializeField] GameObject _hostPanel;

    [Header("Buttons")]
    [SerializeField] Button _joinLobbyBTN;
    [SerializeField] Button _openCreateLobbyBTN;
    [SerializeField] Button _createLobbyBTN;

    [Header("Text")]
    [SerializeField] Text _statusText;

    [Header("InputField")]
    [SerializeField] InputField _inputFieldNameLobby;
    void Start()
    {
        _joinLobbyBTN.onClick.AddListener(BTN_JoinLobby);
        _openCreateLobbyBTN.onClick.AddListener(BTN_HostPanel);
        _createLobbyBTN.onClick.AddListener(BTN_CreateLobby);
        _networkRunner.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            //_browserPanel.SetActive(true);
            _sessionListH.gameObject.SetActive(true);
        };
    }
    void BTN_JoinLobby()
    {
        _networkRunner.JoinLobby();
        _joinLobbyPanel.SetActive(false);
        _statusPanel.SetActive(true);
        _statusText.text = "Entrando al Lobby ....";
    }

    void BTN_HostPanel()
    {
        //_browserPanel.SetActive(false);
        _sessionListH.gameObject.SetActive(false);
        _hostPanel.SetActive(true);
    }

    void BTN_CreateLobby()
    {
        _networkRunner.CreateSession(_inputFieldNameLobby.text, "Game");
    }
}
