using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionListHandler : MonoBehaviour
{
    [Header("Network Runner Handler")]
    [SerializeField] NetworkRunnerHandler _networkRunner;
    [Header("Text")]
    [SerializeField] Text _statusText;
    [Header("Prefab")]
    [SerializeField] ItemSessionInfo _itemSessionInfoPrefab;
    [Header("Layout")]
    [SerializeField] VerticalLayoutGroup _verticalLayoutG;

    private void OnEnable()
    {
        _networkRunner.OnSessionListUpdate += RecieveSessionList;
    }

    private void OnDisable()
    {
        _networkRunner.OnSessionListUpdate -= RecieveSessionList;
    }
    void ClearList()
    {
        foreach (Transform vertLayout in _verticalLayoutG.transform)
        {
            Destroy(vertLayout);
        }
        _statusText.gameObject.SetActive(false);
    }
    void RecieveSessionList(List<Fusion.SessionInfo> allSessions) 
    {
        ClearList();
        if (allSessions.Count ==0)
        {
            NoSessionsFound();
        }
        else
        {
            foreach (var session in allSessions)
            {
                AddSessionToList(session);
            }
        }
    }
    void NoSessionsFound()
    {
        _statusText.text = "No se encontro ninguna session.";
        _statusText.gameObject.SetActive(true);
    }

    void AddSessionToList(Fusion.SessionInfo session) 
    {
        var newSessionItem = Instantiate(_itemSessionInfoPrefab, _verticalLayoutG.transform);
        newSessionItem.SetSessionInfo(session);
        newSessionItem.OnJoinSession += (s) => _networkRunner.JoinSession(s);
    }

}
