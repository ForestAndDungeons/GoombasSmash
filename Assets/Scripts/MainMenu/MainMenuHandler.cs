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
    [SerializeField] GameObject _fullLobby;

    [Header("Buttons")]
    [SerializeField] Button _joinLobbyBTN;
    [SerializeField] Button _openCreateLobbyBTN;
    [SerializeField] Button _createLobbyBTN;

    [Header("Text")]
    [SerializeField] Text _statusText;
    [SerializeField] InputField _inputPlayerName; 

    [Header("InputField")]
    [SerializeField] InputField _inputFieldNameLobby;
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            _inputPlayerName.text = PlayerPrefs.GetString("PlayerNickname");
        }
        _joinLobbyBTN.onClick.AddListener(BTN_JoinLobby);
        _openCreateLobbyBTN.onClick.AddListener(BTN_HostPanel);
        _createLobbyBTN.onClick.AddListener(BTN_CreateLobby);
        _networkRunner.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            //_browserPanel.SetActive(true);
            _sessionListH.gameObject.SetActive(true);
        };
        _networkRunner.OnLobbyFull += () => {
            _fullLobby.SetActive(true);
            StartCoroutine(FullLobbyCoroutine());
        };

    }
    void BTN_JoinLobby()
    {
        PlayerPrefs.SetString("PlayerNickname", _inputPlayerName.text);
        PlayerPrefs.Save();
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

    IEnumerator FullLobbyCoroutine()
    {
        yield return new WaitForSeconds(2);
        _fullLobby.SetActive(false);
    }
}
