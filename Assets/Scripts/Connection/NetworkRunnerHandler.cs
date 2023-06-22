using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _networkRunnerPrefab;
    NetworkRunner _currNetRunner;
    public event Action OnJoinedLobby;
    public event Action<List<SessionInfo>> OnSessionListUpdate;
  /*  private void Start()
    {
        JoinLobby();
    }*/

    public void JoinLobby()
    {
        if (_currNetRunner) Destroy(_currNetRunner.gameObject);
        _currNetRunner = Instantiate(_networkRunnerPrefab);
        _currNetRunner.AddCallbacks(this);
        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {
        var res = await _currNetRunner.JoinSessionLobby(SessionLobby.Custom, "Lobby");
        if (!res.Ok)
        {
            Debug.Log("No se puede unir al Lobby");
        }
        else
        {
            Debug.Log("Te uniste al lobby");
            OnJoinedLobby?.Invoke();
        }
    }


    public void CreateSession(string sessionName, string sceneName)
    {
        var clientTask = InitialSession(_currNetRunner, GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public void JoinSession(SessionInfo sessionInfo)
    {
        var clientTask = InitialSession(_currNetRunner, GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex);
    }

    async Task InitialSession(NetworkRunner runner, GameMode gameMode, string sessionName, SceneRef scene)
    {
        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();
        var res = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "Lobby",
            SceneManager = sceneManager
        });
        if (!res.Ok)
        {
            Debug.Log("No puede comenzar el juego");
        }
        else
        {
            Debug.Log("Comienza el juego");
        }
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);
       /* if (sessionList.Count > 0)
        {
            foreach (var session in sessionList)
            {
                if (session.PlayerCount < session.MaxPlayers)
                {
                    JoinSession(session);
                    return;
                }
            }
        }*/

    }

    #region Unused Callbacks

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion

}
